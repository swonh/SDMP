﻿using Nodez.Sdmp.General.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.General.Managers
{
    public class ApproximationManager
    {
        private static Lazy<ApproximationManager> lazy = new Lazy<ApproximationManager>(() => new ApproximationManager());

        public static ApproximationManager Instance { get { return lazy.Value; } }

        public void Reset() { lazy = new Lazy<ApproximationManager>(); }

        public bool IsCalculateEstimationValue(bool isUseEstimationValue, int estimationValueUpdatePeriod, int stageIndex, int loopCount)
        {
            if (isUseEstimationValue == false)
                return false;

            if (loopCount % estimationValueUpdatePeriod != 0)
                return false;

            int stopStageIndex = ApproximationControl.Instance.GetEstimationValueStopStageIndex();

            if (stageIndex >= stopStageIndex)
                return false;

            return true;
        }
    }
}