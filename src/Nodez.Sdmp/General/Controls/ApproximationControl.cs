// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Routing.Interfaces;
using Nodez.Sdmp.Routing.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.General.Controls
{
    public class ApproximationControl
    {
        private static readonly Lazy<ApproximationControl> lazy = new Lazy<ApproximationControl>(() => new ApproximationControl());

        public static ApproximationControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.ApproximationControl.ToString(), out object control))
                {
                    return (ApproximationControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual bool IsApplyStateFiltering() 
        {
            return false;
        }

        public virtual bool IsApplyApproximation()
        {
            return false;
        }

        public virtual bool IsUseValueFunctionEstimate()
        {
            return false;
        }

        public virtual int GetValueFunctionEstimateUpdatePeriod()
        {
            return 1;
        }

        public virtual int GetValueFunctionEstimateStopStageIndex()
        {
            return Int32.MaxValue;
        }

        public virtual StateFilteringType GetStateFilteringType() 
        {
            return StateFilteringType.Global;
        }

        public virtual int GetGlobalTransitionCount()
        {
            return 10;
        }

        public virtual int GetApproximationTransitionCount() 
        {
            return 10;
        }

        public virtual int GetClusterTransitionCount() 
        {
            return 2;
        }

        public virtual int GetLocalTransitionCount() 
        {
            return 1;
        }

        public virtual int GetGlobalFilteringStartStageIndex()
        {
            return 0;
        }

        public virtual int GetLocalFilteringStartStageIndex()
        {
            return 0;
        }

        public virtual int GetApproximationStartStageIndex() 
        {
            return 0;
        }

        public virtual double GetMinimumTransitionCost() 
        {
            return RoutingDataManager.Instance.DistanceInfoTable.Rows().Cast<IDistanceInfoData>().Where(x => x.DISTANCE > 0).Min(x => x.DISTANCE);
        }

        public virtual double GetMultiplier() 
        {
            return 2;
        }

        public virtual double GetValueFunctionEstimate(State state) 
        {
            double dualBound = BoundControl.Instance.GetDualBound(state);
            state.SetDualBound(dualBound);
            state.SetIsValueFunctionCalculated(true);

            return state.CurrentBestValue + state.DualBound;
        }

        public virtual List<State> FilterGlobalStates(List<State> states, int maxTransitionCount, ObjectiveFunctionType objectiveFunctionType, double pruneTolerance, bool isApplyStateClustering)
        {
            SolutionManager solutionManager = SolutionManager.Instance;

            List<State> selectedStateList = new List<State>();

            if (isApplyStateClustering)
            {
                Dictionary<int, List<State>> clusters = new Dictionary<int, List<State>>();
                foreach (State state in states)
                {
                    if (clusters.TryGetValue(state.ClusterID, out List<State> list) == false)
                    {
                        clusters.Add(state.ClusterID, new List<State>() { state });
                    }
                    else
                    {
                        list.Add(state);
                    }
                }

                int clusterTransitionCount = this.GetClusterTransitionCount();
                foreach (KeyValuePair<int, List<State>> item in clusters)
                {
                    List<State> list = item.Value.OrderBy(x => x.ClusterDistance).ToList();

                    int maxCount = clusterTransitionCount;
                    int count = 0;
                    foreach (State st in list)
                    {
                        if (count > maxCount)
                            break;

                        selectedStateList.Add(st);

                        count++;
                    }
                }
            }
            else
            {
                int total = states.Count;
                int current = 1;
                bool isLast = false;
                foreach (State state in states)
                {
                    if (state.IsFinal)
                        continue;

                    if (total == current)
                        isLast = true;

                    double valueFunctionEstimate = GetValueFunctionEstimate(state);
                    state.SetValueFunctionEstimate(valueFunctionEstimate);

                    if (solutionManager.CheckOptimalityCondition())
                        break;

                    LogControl.Instance.ShowProgress(current, total, isLast);
                    current++;
                }

                if (objectiveFunctionType == ObjectiveFunctionType.Minimize)
                    states = states.Where(x => x.IsSetValueFunctionEstimate).OrderBy(x => x.ValueFunctionEstimate).ToList();
                else if (objectiveFunctionType == ObjectiveFunctionType.Maximize)
                    states = states.Where(x => x.IsSetValueFunctionEstimate).OrderByDescending(x => x.ValueFunctionEstimate).ToList();

                int count = 0;
                foreach (State state in states)
                {
                    if (maxTransitionCount <= count)
                        break;

                    selectedStateList.Add(state);

                    count++;
                }
            }

            return selectedStateList;
        }

        public virtual List<State> FilterLocalStates(State currentState, List<State> states, int maxTransitionCount)
        {
            states = states.OrderBy(x => x.PrevBestState.DualBound + (x.CurrentBestValue - x.PrevBestState.CurrentBestValue) + x.CurrentBestValue).ToList();

            List<State> selectedStateList = new List<State>();

            int count = 0;
            foreach (State state in states)
            {
                if (maxTransitionCount <= count)
                    break;

                selectedStateList.Add(state);
                count++;
            }

            int totalStateCount = states != null ? states.Count : 0;
            int selectedStateCount = selectedStateList != null ? selectedStateList.Count : 0;
            int filteredStateCount = totalStateCount - selectedStateCount;

            StateManager.Instance.SetFilteredStateCount(currentState.Stage.Index, filteredStateCount);

            return selectedStateList;
        }

        public virtual bool CanPruneByApproximation(State state, ObjectiveFunctionType objFuncType, double minEstimationValue, double minTransitionCost, double multiplier, double pruneTolerance)
        {
            if (state.IsFinal)
                return false;

            if (objFuncType == ObjectiveFunctionType.Minimize)
            {
                if (state.ValueFunctionEstimate + pruneTolerance > minEstimationValue + (minTransitionCost * multiplier))
                    return true;
                else
                    return false;
            }
            else
            {
                if (state.ValueFunctionEstimate + pruneTolerance < minEstimationValue + (minTransitionCost * multiplier))
                    return true;
                else
                    return false;
            }
        }
    }
}
