// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.SchedulingTemplate.Controls
{
    public class UserApproximationControl : ApproximationControl
    {
        private static readonly Lazy<UserApproximationControl> lazy = new Lazy<UserApproximationControl>(() => new UserApproximationControl());

        public static new UserApproximationControl Instance { get { return lazy.Value; } }

        public override bool IsApplyStateFiltering()
        {
            return true;
        }

        public override bool IsApplyApproximation()
        {
            return false;
        }

        public override bool IsUseValueFunctionEstimate()
        {
            return false;
        }

        public override int GetValueFunctionEstimateUpdatePeriod()
        {
            return 1;
        }

        public override int GetValueFunctionEstimateStopStageIndex()
        {
            return Int32.MaxValue;
        }

        public override StateFilteringType GetStateFilteringType()
        {
            return StateFilteringType.Global;
        }

        public override int GetGlobalTransitionCount()
        {
            return 1000;
        }

        public override int GetLocalTransitionCount()
        {
            return 1000;
        }

        public override int GetApproximationTransitionCount()
        {
            return 1;
        }

        public override int GetClusterTransitionCount()
        {
            return 3;
        }

        public override int GetGlobalFilteringStartStageIndex()
        {
            return 1;
        }

        public override int GetLocalFilteringStartStageIndex()
        {
            return 0;
        }

        public override int GetApproximationStartStageIndex()
        {
            return 0;
        }

        public override double GetMinimumTransitionCost()
        {
            return 1;
        }

        public override double GetMultiplier()
        {
            return 0.1;
        }

        public override double GetValueFunctionEstimate(State state)
        {
            double estimatedValue = 0;
            estimatedValue = state.CurrentBestValue + state.DualBound;
            state.SetIsValueFunctionCalculated(true);

            return estimatedValue;
        }

        public override List<State> FilterGlobalStates(List<State> states, int maxTransitionCount, ObjectiveFunctionType objectiveFunctionType, double pruneTolerance, bool isApplyStateClustering)
        {
            List<State> filtered = new List<State>();

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

                        filtered.Add(st);

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

                    LogControl.Instance.ShowProgress(current, total, isLast);
                    current++;
                }

                if (objectiveFunctionType == ObjectiveFunctionType.Minimize)
                    states = states.OrderBy(x => x.ValueFunctionEstimate).ToList();
                else if (objectiveFunctionType == ObjectiveFunctionType.Maximize)
                    states = states.OrderByDescending(x => x.ValueFunctionEstimate).ToList();

                int count = 0;
                foreach (State state in states)
                {
                    if (maxTransitionCount <= count)
                        break;

                    filtered.Add(state);

                    count++;
                }
            }

            return filtered;
        }

        public override List<State> FilterLocalStates(State currentState, List<State> states, int maxTransitionCount)
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

        public override bool CanPruneByApproximation(State state, ObjectiveFunctionType objFuncType, double minEstimationValue, double minTransitionCost, double multiplier, double pruneTolerance)
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
