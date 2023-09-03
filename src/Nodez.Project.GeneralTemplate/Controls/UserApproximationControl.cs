using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.GeneralTemplate.Controls
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

        public override bool IsUseEstimationValue()
        {
            return false;
        }

        public override int GetEstimationValueUpdatePeriod()
        {
            return 1;
        }

        public override int GetEstimationValueStopStageIndex()
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

        public override double GetEstimatedValue(State state)
        {
            double estimatedValue = 0;
            estimatedValue = state.BestValue + state.DualBound;

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
                        if (count >= maxCount)
                            break;

                        filtered.Add(st);

                        count++;
                    }
                }
            }
            else
            {
                //states = states.OrderBy(x => 0 + (x.BestValue - x.PrevBestState.BestValue) + x.BestValue).ToList();
                //states = states.OrderBy(x => x.PrevBestState.DualBound + (x.BestValue - x.PrevBestState.BestValue) + x.BestValue).ToList();
                //states = states.OrderBy(x => x.PrevBestState.EstimationValue + (x.BestValue - x.PrevBestState.BestValue) + x.BestValue).ToList();
                //states = states.OrderBy(x => x.EstimationValue).ToList();
                foreach (State state in states)
                {
                    if (state.IsFinal)
                        continue;

                    double estimatedValue = GetEstimatedValue(state);
                    state.EstimationValue = estimatedValue;
                }

                if (objectiveFunctionType == ObjectiveFunctionType.Minimize)
                    states = states.OrderBy(x => x.EstimationValue).ToList();
                else if (objectiveFunctionType == ObjectiveFunctionType.Maximize)
                    states = states.OrderByDescending(x => x.EstimationValue).ToList();

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

        public override List<State> FilterLocalStates(List<State> states, int maxTransitionCount)
        {
            states = states.OrderBy(x => x.PrevBestState.DualBound + (x.BestValue - x.PrevBestState.BestValue) + x.BestValue).ToList();

            List<State> filtered = new List<State>();

            int count = 0;
            foreach (State state in states)
            {
                if (maxTransitionCount <= count)
                    break;

                filtered.Add(state);
                count++;
            }

            return filtered;
        }

        public override bool CanPruneByApproximation(State state, ObjectiveFunctionType objFuncType, double minEstimationValue, double minTransitionCost, double multiplier, double pruneTolerance)
        {
            if (state.IsFinal)
                return false;

            if (objFuncType == ObjectiveFunctionType.Minimize)
            {
                if (state.EstimationValue + pruneTolerance > minEstimationValue + (minTransitionCost * multiplier))
                    return true;
                else
                    return false;
            }
            else
            {
                if (state.EstimationValue + pruneTolerance < minEstimationValue + (minTransitionCost * multiplier))
                    return true;
                else
                    return false;
            }
        }
    }
}
