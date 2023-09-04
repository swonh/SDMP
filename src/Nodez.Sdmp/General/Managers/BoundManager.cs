// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nodez.Sdmp.General.Managers
{

    public class BoundManager
    {
        private static Lazy<BoundManager> lazy = new Lazy<BoundManager>(() => new BoundManager());

        public static BoundManager Instance { get { return lazy.Value; } }

        public void Reset() { lazy = new Lazy<BoundManager>(); }

        public double BestPrimalBound { get; private set; }

        public double BestDualBound { get; private set; }

        public double RootDualBound { get; private set; }

        public double RootPrimalBound { get; private set; }

        public double RelativeDualityGap { get { return Math.Round(Math.Abs(this.BestPrimalBound - this.BestDualBound) / this.BestPrimalBound, 6); } }

        public void SetPrimalBound(double value) 
        {
            this.BestPrimalBound = value;
        }

        public void SetDualBound(double value) 
        {
            this.BestDualBound = value;
        }

        public void UpdateBestPrimalBound(State state, double primalBound, ObjectiveFunctionType objFuncType, TimeSpan elapsedTime)
        {
            if (objFuncType == ObjectiveFunctionType.Minimize)
            {
                if (this.BestPrimalBound > primalBound)
                {
                    SolverManager.Instance.SetBestSolutionDateTime(SolverManager.Instance.CurrentSolverName, DateTime.Now);

                    this.BestPrimalBound = primalBound;
                    LogControl.Instance.WritePrimalBoundUpdateLog(state, primalBound, elapsedTime);

                    if (LogControl.Instance.IsExportStateLog())
                    {
                        StateLog stateLog = LogControl.Instance.GetStateLog(state, elapsedTime);
                        SolverManager.Instance.AddStateLog(stateLog);
                    }
                }
            }
            else
            {
                if (this.BestPrimalBound < primalBound)
                {
                    SolverManager.Instance.SetBestSolutionDateTime(SolverManager.Instance.CurrentSolverName, DateTime.Now);

                    this.BestPrimalBound = primalBound;
                    LogControl.Instance.WritePrimalBoundUpdateLog(state, primalBound, elapsedTime);

                    if (LogControl.Instance.IsExportStateLog())
                    {
                        StateLog stateLog = LogControl.Instance.GetStateLog(state, elapsedTime);
                        SolverManager.Instance.AddStateLog(stateLog);
                    }
                }
            }
        }

        public void UpdateBestDualBound(State state, double dualBound, ObjectiveFunctionType objFuncType, TimeSpan elapsedTime)
        {
            if (objFuncType == ObjectiveFunctionType.Minimize)
            {
                if (this.BestDualBound < dualBound)
                {
                    this.BestDualBound = dualBound;
                    LogControl.Instance.WriteDualBoundUpdateLog(state, dualBound, elapsedTime);

                    if (LogControl.Instance.IsExportStateLog())
                    {
                        StateLog stateLog = LogControl.Instance.GetStateLog(state, elapsedTime);
                        SolverManager.Instance.AddStateLog(stateLog);
                    }
                }
            }
            else 
            {
                if (this.BestDualBound > dualBound)
                {
                    this.BestDualBound = dualBound;
                    LogControl.Instance.WriteDualBoundUpdateLog(state, dualBound, elapsedTime);

                    if (LogControl.Instance.IsExportStateLog())
                    {
                        StateLog stateLog = LogControl.Instance.GetStateLog(state, elapsedTime);
                        SolverManager.Instance.AddStateLog(stateLog);
                    }
                }
            }
        }

        public void SetRootDualBound(double rootDualBound)
        {
            this.RootDualBound = rootDualBound;
        }

        public void SetRootPrimalBound(double rootPrimalBound) 
        {
            this.RootPrimalBound = rootPrimalBound;
        }

        public bool IsCalculatePrimalBound(bool isUsePrimalBound, int primalSolutionUpdatePeriod, int stageIndex, int loopCount)
        {
            if (isUsePrimalBound == false)
                return false;

            if (loopCount % primalSolutionUpdatePeriod != 0)
                return false;

            int stopStageIndex = BoundControl.Instance.GetPrimalBoundStopStageIndex();
            double stopRelativeGap = BoundControl.Instance.GetPrimalBoundStopRelativeGap();

            if (stageIndex >= stopStageIndex)
                return false;

            if (this.RelativeDualityGap <= stopRelativeGap)
                return false;

            return true;
        }

        public bool IsCalculateDualBound(bool isUseDualBound, int dualBoundUpdatePeriod, int stageIndex, int loopCount)
        {
            if (isUseDualBound == false)
                return false;

            if (loopCount % dualBoundUpdatePeriod != 0)
                return false;

            int stopStageIndex = BoundControl.Instance.GetDualBoundStopStageIndex();
            double stopRelativeGap = BoundControl.Instance.GetDualBoundStopRelativeGap();

            if (stageIndex >= stopStageIndex)
                return false;

            if (this.RelativeDualityGap <= stopRelativeGap)
                return false;

            return true;
        }

    }
}
