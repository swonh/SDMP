using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nodez.Project.GeneralTemplate.Controls
{
    public class UserBoundControl : BoundControl
    {
        private static readonly Lazy<UserBoundControl> lazy = new Lazy<UserBoundControl>(() => new UserBoundControl());

        public static new UserBoundControl Instance { get { return lazy.Value; } }

        public override bool IsUsePrimalBound()
        {
            return true;
        }

        public override bool IsUseDualBound()
        {
            return true;
        }

        public override double GetPruneTolerance()
        {
            return Math.Pow(10, -4);
        }

        public override int GetPrimalBoundStopStageIndex()
        {
            return Int32.MaxValue;
        }

        public override double GetPrimalBoundStopRelativeGap()
        {
            return 0;
        }

        public override int GetDualBoundStopStageIndex()
        {
            return Int32.MaxValue;
        }

        public override double GetDualBoundStopRelativeGap()
        {
            return 0;
        }

        public override bool UseAbsoluteOptimalityGap()
        {
            return false;
        }

        public override double GetRelativeOptimalityGap()
        {
            return 0;
        }

        public override double GetAbsoluteOptimalityGap()
        {
            return 0;
        }

        public override double GetDualBound(State state)
        {
            return 0;
        }

        public override double GetPrimalBound(Solution solution)
        {
            if (solution != null)
                return solution.Value;

            return 0;
        }

        public override int GetPrimalSolutionUpdatePeriod()
        {
            return 1000;
        }

        public override int GetDualBoundUpdatePeriod()
        {
            return 1;
        }

    }
}
