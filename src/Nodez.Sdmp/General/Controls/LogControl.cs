// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.DataModel;
using Nodez.Sdmp.Constants;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.General.Solver;
using Nodez.Sdmp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nodez.Sdmp.General.Controls
{
    public class LogControl
    {
        private static readonly Lazy<LogControl> lazy = new Lazy<LogControl>(() => new LogControl());

        public static LogControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.LogControl.ToString(), out object control))
                {
                    return (LogControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual int GetLogPeriod()
        {
            return 5000;
        }

        public virtual void WriteSolution(Solution solution) 
        {
            
        }

        public virtual bool IsWriteStatusLog() 
        {
            return false;
        }

        public virtual bool IsWriteStateInfoLog() 
        {
            return false;
        }

        public StatusLog GetStatusLog(State state, TimeSpan elapsedTime) 
        {
            string relativeDualityGap = string.Format("{0:F2}%", Math.Round(BoundManager.Instance.RelativeDualityGap * 100, 2));
            string bestSolution = BoundManager.Instance.BestPrimalBound != Double.PositiveInfinity && BoundManager.Instance.BestPrimalBound != Double.NegativeInfinity ? string.Format("{0:F6}", BoundManager.Instance.BestPrimalBound) : string.Empty;
            string bestDualBound = BoundManager.Instance.BestDualBound != Double.PositiveInfinity && BoundManager.Instance.BestDualBound != Double.NegativeInfinity ? string.Format("{0:F6}", BoundManager.Instance.BestDualBound) : string.Empty;
            string numSolutions = string.Format("{0}", SolutionManager.Instance.Solutions.Count);
            string time = elapsedTime.TotalSeconds.ToString();

            StatusLog log = new StatusLog();

            log.STATE_INDEX = state == null ? "Global dual bound update" : state.Index.ToString();
            log.STAGE_INDEX = state == null ? "Global dual bound update" : state.Stage.Index.ToString();
            log.BEST_SOLUTION = bestSolution;
            log.BEST_DUAL_BOUND = bestDualBound;
            log.SOLUTION_COUNT = numSolutions;
            log.RELATIVE_DUALITY_GAP = relativeDualityGap;
            log.TIME = time;

            return log;
        }

        public StateInfoLog GetStateInfoLog(State state, bool afterFiltering)
        {
            StateInfoLog log = new StateInfoLog();

            log.STATE_INDEX = state.Index;
            log.STATE_KEY = state.Key;
            log.STAGE_INDEX = state.Stage.Index;
            log.CURRENT_BEST_VALUE = state.CurrentBestValue;
            log.DUAL_BOUND = state.DualBound;
            log.SUM_CURRENT_DUAL = state.CurrentBestValue + state.DualBound;
            log.PRIMAL_BOUND = state.PrimalBound;
            log.VALUE_FUNCTION_ESTIMATE = state.ValueFunctionEstimate;
            log.IS_AFTER_FILTERING = afterFiltering;

            return log;
        }

        public List<StateInfoLog> GetStateInfoLogs(List<State> states, bool afterFiltering) 
        {
            List<StateInfoLog> logs = new List<StateInfoLog>();

            foreach (State state in states) 
            {
                StateInfoLog log = new StateInfoLog();
                log.STATE_INDEX = state.Index;
                log.STATE_KEY = state.Key;
                log.STAGE_INDEX = state.Stage.Index;
                log.CURRENT_BEST_VALUE = state.CurrentBestValue;
                log.DUAL_BOUND = state.DualBound;
                log.SUM_CURRENT_DUAL = state.CurrentBestValue + state.DualBound;
                log.PRIMAL_BOUND = state.PrimalBound;
                log.VALUE_FUNCTION_ESTIMATE = state.ValueFunctionEstimate;
                log.IS_AFTER_FILTERING = afterFiltering;

                logs.Add(log);
            }

            return logs;
        }

        public void ShowProgress(int current, int total, bool isLast)
        {
            const int updateInterval = 10;
            if (current % updateInterval == 0 || current == total)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Progress: {current}/{total} ({(current * 100) / total}%)");

                if (isLast)
                {
                    Console.WriteLine();
                }
            }
        }

        public void WritePrimalBoundUpdateLog(State state, double primalBound, TimeSpan elapsedTime) 
        {
            if (primalBound == 0)
                return;

            string relativeDualityGap = string.Format("{0:F2}%", Math.Round(BoundManager.Instance.RelativeDualityGap * 100, 2));
            string bestSolution = BoundManager.Instance.BestPrimalBound != Double.PositiveInfinity && BoundManager.Instance.BestPrimalBound != Double.NegativeInfinity ? string.Format("{0:F6}", BoundManager.Instance.BestPrimalBound) : string.Empty;
            string bestDualBound = BoundManager.Instance.BestDualBound != Double.PositiveInfinity && BoundManager.Instance.BestDualBound != Double.NegativeInfinity ? string.Format("{0:F6}", BoundManager.Instance.BestDualBound) : string.Empty;
            string numSolutions = string.Format("{0}", SolutionManager.Instance.Solutions.Count);
            string time = elapsedTime.ToString("hh\\:mm\\:ss");

            string log = string.Format(" * {0,8} {1,9} {2,16} {3,16} {4,8} {5,7} {6,10}", state.Index, state.Stage.Index, bestSolution, bestDualBound, numSolutions, relativeDualityGap, time);
            Console.WriteLine(log);
        }

        public void WriteDualBoundUpdateLog(State state, double dualBound, TimeSpan elapsedTime)
        {
            if (dualBound == 0)
                return;

            if (state == null)
                return;

            string relativeDualityGap = string.Format("{0:F2}%", Math.Round(BoundManager.Instance.RelativeDualityGap * 100, 2));
            string bestSolution = BoundManager.Instance.BestPrimalBound != Double.PositiveInfinity && BoundManager.Instance.BestPrimalBound != Double.NegativeInfinity ? string.Format("{0:F6}", BoundManager.Instance.BestPrimalBound) : string.Empty;
            string bestDualBound = BoundManager.Instance.BestDualBound != Double.PositiveInfinity && BoundManager.Instance.BestDualBound != Double.NegativeInfinity ? string.Format("{0:F6}", BoundManager.Instance.BestDualBound) : string.Empty;
            string numSolutions = string.Format("{0}", SolutionManager.Instance.Solutions.Count);
            string time = elapsedTime.ToString("hh\\:mm\\:ss");

            string log = string.Format(" d {0,8} {1,9} {2,16} {3,16} {4,8} {5,7} {6,10}", state.Index, state.Stage.Index, bestSolution, bestDualBound, numSolutions, relativeDualityGap, time);
            Console.WriteLine(log);
        }

        public void WriteStatusLog(State state, TimeSpan elapsedTime)
        {
            string relativeDualityGap = string.Format("{0:F2}%", Math.Round(BoundManager.Instance.RelativeDualityGap * 100, 2));
            string bestSolution = BoundManager.Instance.BestPrimalBound != Double.PositiveInfinity && BoundManager.Instance.BestPrimalBound != Double.NegativeInfinity ? string.Format("{0:F6}", BoundManager.Instance.BestPrimalBound) : string.Empty;
            string bestDualBound = BoundManager.Instance.BestDualBound != Double.PositiveInfinity && BoundManager.Instance.BestDualBound != Double.NegativeInfinity ? string.Format("{0:F6}", BoundManager.Instance.BestDualBound) : string.Empty;
            string numSolutions = string.Format("{0}", SolutionManager.Instance.Solutions.Count);
            string time =  elapsedTime.ToString("hh\\:mm\\:ss");

            string log = string.Format("   {0,8} {1,9} {2,16} {3,16} {4,8} {5,7} {6,10}", state.Index, state.Stage.Index, bestSolution, bestDualBound, numSolutions, relativeDualityGap, time);
            Console.WriteLine(log);
        }

        public void WriteStartLog(ISolver solver)
        {
            Console.WriteLine(Constants.Constants.LINE);
            Console.WriteLine(string.Format("Start Solver...{0}", DateTime.Now));
            Console.WriteLine(string.Format("Solver Name : {0}", solver.Name));
            Console.WriteLine(string.Format("Objective Function Type : {0}", solver.ObjectiveFunctionType.ToString()));
            Console.WriteLine(string.Format("Run Seq : {0}", solver.RunConfig.RUN_SEQ));

            string col1 = string.Format("Node");
            string col2 = string.Format("Stage");
            string col3 = string.Format("BestSolution");
            string col4 = string.Format("BestBound");
            string col5 = string.Format("Sols");
            string col6 = string.Format("Gap");
            string col7 = string.Format("Time");

            Console.WriteLine("   {0,8} {1,9} {2,16} {3,16} {4,8} {5,7} {6,10}", col1, col2, col3, col4, col5, col6, col7);
        }

        public virtual void WriteBestSolutionLog() 
        {
            SolutionManager solutionManager = SolutionManager.Instance;
            Solution bestSol = solutionManager.BestSolution;

            Console.WriteLine(string.Format("Best Objective Value: {0}", bestSol.Value));
        }

        public virtual void WriteOptimalLog()
        {
            SolutionManager solutionManager = SolutionManager.Instance;
            Solution optSol = solutionManager.OptimalSolution;

            Console.WriteLine(string.Format("Optimal Objective Value: {0}", optSol.Value));
        }

        public virtual void WritePruneLog(State state)
        {
            //Console.WriteLine("Prune => StateIndex:{0}, State:{1}, Stage:{2}, DualBound:{3}, BestValue:{4}, BestPrimalBound:{5}", state.Index, state.ToString(), state.Stage.Index, state.DualBound, state.BestValue, BoundManager.Instance.BestPrimalBound);
        }

        public virtual void WriteRandomSolutionGenerationStartLog() 
        {
            int count = MachineLearningControl.Instance.GetRandomSolutionGenerationCount();
            Console.WriteLine(string.Format("Start Random Solution Generation... ({0} Solutions)", count));
        }

        public virtual void WriteRandomSolutionGenerationEndLog()
        {
            Console.WriteLine(string.Format("End Random Solution Generation"));
        }

        public virtual void WriteStateClusteringStartLog(int totalStateCount) 
        {
            int clusterCount = MachineLearningControl.Instance.GetClusterCount();
            int clusterTransitionCount = ApproximationControl.Instance.GetClusterTransitionCount();
            Console.WriteLine(string.Format("Start State Clustering... (Total State Count:{0}, Cluster Count:{1}, Cluster Transition Count:{2})", totalStateCount, clusterCount, clusterTransitionCount));
        }

        public virtual void WriteStateClusteringEndLog()
        {
            Console.WriteLine(string.Format("End State Clustering"));
        }

        public virtual void WriteGlobalFilteringStartLog(int stageIndex, int totalStateCount)
        {
            int filterCount = ApproximationControl.Instance.GetGlobalTransitionCount();
            Console.WriteLine(string.Format("Start Global Filtering... (Stage:{0}, Total State Count:{1}, Filtering Count:{2})", stageIndex, totalStateCount, filterCount));
        }

        public virtual void WriteGlobalFilteringEndLog()
        {
            Console.WriteLine(string.Format("End Global Filtering"));
        }

        public void WriteEndLog(string reason)
        {
            Console.WriteLine("Solver Ended (Reason:{0})", reason);
            Console.WriteLine(string.Format("End Solver {0}", DateTime.Now));
        }
    }
}
