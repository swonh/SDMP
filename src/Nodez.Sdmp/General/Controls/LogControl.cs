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

        public virtual int GetStateLogPeriod()
        {
            return 5000;
        }

        public virtual void WriteSolution(Solution solution) 
        {
            
        }

        public virtual bool IsExportStateLog() 
        {
            return false;
        }

        public StateLog GetStateLog(State state, TimeSpan elapsedTime) 
        {
            string relativeDualityGap = BoundManager.Instance.RelativeDualityGap < 1 ? string.Format("{0:F2}%", Math.Round(BoundManager.Instance.RelativeDualityGap * 100, 2)) : string.Empty;
            string bestSolution = BoundManager.Instance.BestPrimalBound != Double.PositiveInfinity && BoundManager.Instance.BestPrimalBound != Double.NegativeInfinity ? string.Format("{0:F6}", BoundManager.Instance.BestPrimalBound) : string.Empty;
            string bestDualBound = BoundManager.Instance.BestDualBound != Double.PositiveInfinity && BoundManager.Instance.BestDualBound != Double.NegativeInfinity ? string.Format("{0:F6}", BoundManager.Instance.BestDualBound) : string.Empty;
            string numSolutions = string.Format("{0}", SolutionManager.Instance.Solutions.Count);
            string time = elapsedTime.TotalSeconds.ToString();

            StateLog log = new StateLog();

            log.STATE_INDEX = state == null ? "Global dual bound update" : state.Index.ToString();
            log.STAGE_INDEX = state == null ? "Global dual bound update" : state.Stage.Index.ToString();
            log.BEST_SOLUTION = bestSolution;
            log.BEST_DUAL_BOUND = bestDualBound;
            log.SOLUTION_COUNT = numSolutions;
            log.RELATIVE_DUALITY_GAP = relativeDualityGap;
            log.TIME = time;

            return log;
        }

        public void WritePrimalBoundUpdateLog(State state, double primalBound, TimeSpan elapsedTime) 
        {
            if (primalBound == 0)
                return;

            string relativeDualityGap = BoundManager.Instance.RelativeDualityGap < 1 ? string.Format("{0:F2}%", Math.Round(BoundManager.Instance.RelativeDualityGap * 100, 2)) : string.Empty;
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

            string relativeDualityGap = BoundManager.Instance.RelativeDualityGap < 1 ? string.Format("{0:F2}%", Math.Round(BoundManager.Instance.RelativeDualityGap * 100, 2)) : string.Empty;
            string bestSolution = BoundManager.Instance.BestPrimalBound != Double.PositiveInfinity && BoundManager.Instance.BestPrimalBound != Double.NegativeInfinity ? string.Format("{0:F6}", BoundManager.Instance.BestPrimalBound) : string.Empty;
            string bestDualBound = BoundManager.Instance.BestDualBound != Double.PositiveInfinity && BoundManager.Instance.BestDualBound != Double.NegativeInfinity ? string.Format("{0:F6}", BoundManager.Instance.BestDualBound) : string.Empty;
            string numSolutions = string.Format("{0}", SolutionManager.Instance.Solutions.Count);
            string time = elapsedTime.ToString("hh\\:mm\\:ss");

            string log = string.Format(" d {0,8} {1,9} {2,16} {3,16} {4,8} {5,7} {6,10}", state.Index, state.Stage.Index, bestSolution, bestDualBound, numSolutions, relativeDualityGap, time);
            Console.WriteLine(log);
        }

        public void WriteStateLog(State state, TimeSpan elapsedTime)
        {
            string relativeDualityGap = BoundManager.Instance.RelativeDualityGap < 1 ? string.Format("{0:F2}%", Math.Round(BoundManager.Instance.RelativeDualityGap * 100, 2)) : string.Empty;
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
            Console.WriteLine(string.Format("Stop Random Solution Generation..."));
        }

        public virtual void WriteStateClusteringStartLog(int totalStateCount) 
        {
            int clusterCount = MachineLearningControl.Instance.GetClusterCount();
            int clusterTransitionCount = ApproximationControl.Instance.GetClusterTransitionCount();
            Console.WriteLine(string.Format("Start State Clustering... (Total State Count:{0}, Cluster Count:{1}, Cluster Transition Count:{2})", totalStateCount, clusterCount, clusterTransitionCount));
        }

        public virtual void WriteStateClusteringEndLog()
        {
            Console.WriteLine(string.Format("Stop State Clustering..."));
        }

        public void WriteEndLog(string reason)
        {
            Console.WriteLine("Solver Ended...(Reason:{0})", reason);
            Console.WriteLine(string.Format("End Solver...{0}", DateTime.Now));
        }
    }
}
