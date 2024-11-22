// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Constants;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nodez.Sdmp.General.Controls
{
    public class BoundControl
    {
        private static readonly Lazy<BoundControl> lazy = new Lazy<BoundControl>(() => new BoundControl());

        public static BoundControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.BoundControl.ToString(), out object control))
                {
                    return (BoundControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual bool IsUsePrimalBound() 
        {
            return true;
        }

        public virtual bool IsUseDualBound()
        {
            return false;
        }

        public virtual double GetPruneTolerance() 
        {
            return Math.Pow(10, -4);
        }

        public virtual int GetPrimalBoundStopStageIndex() 
        {
            return 0;
        }

        public virtual double GetPrimalBoundStopRelativeGap() 
        {
            return 0;
        }

        public virtual int GetDualBoundStopStageIndex()
        {
            return 0;
        }

        public virtual double GetDualBoundStopRelativeGap()
        {
            return 0;
        }

        public virtual double GetInitialPrimalBound(ObjectiveFunctionType objectiveFunctionType) 
        {
            double primalBound = 0;

            if (objectiveFunctionType == ObjectiveFunctionType.Maximize)
            {
                primalBound = Double.NegativeInfinity;
            }
            else if (objectiveFunctionType == ObjectiveFunctionType.Minimize) 
            {
                primalBound = Double.PositiveInfinity;
            }

            return primalBound;
        }

        public virtual double GetInitialDualBound(ObjectiveFunctionType objectiveFunctionType)
        {
            double dualBound = 0;

            if (objectiveFunctionType == ObjectiveFunctionType.Maximize)
            {
                dualBound = Double.PositiveInfinity;
            }
            else if (objectiveFunctionType == ObjectiveFunctionType.Minimize)
            {
                dualBound = Double.NegativeInfinity;
            }

            return dualBound;
        }

        public virtual bool UseAbsoluteOptimalityGap()
        {
            return false;
        }

        public virtual double GetRelativeOptimalityGap()
        {
            return 0;
        }

        public virtual double GetAbsoluteOptimalityGap()
        {
            return 0;
        }

        public virtual double GetDualBound(State state)
        {
            return 0;
        }

        public virtual double GetPrimalBound(Solution solution)
        {
            return 0;  
        }

        public virtual int GetPrimalSolutionUpdatePeriod()
        {
            return 1000;
        }

        public virtual int GetDualBoundUpdatePeriod()
        {
            return 1;
        }

    }
}
