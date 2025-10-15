// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.General.Controls;
using System;

namespace Nodez.Sdmp.General.Managers
{
    public class ApproximationManager
    {
        private static Lazy<ApproximationManager> lazy = new Lazy<ApproximationManager>(() => new ApproximationManager());

        public static ApproximationManager Instance { get { return lazy.Value; } }

        public void Reset() { lazy = new Lazy<ApproximationManager>(); }

        public bool IsCalculateValueFunctionEstimate(bool isUseValueFunctionEstimate, int valueFunctionEstimateUpdatePeriod, int stageIndex, int loopCount)
        {
            if (isUseValueFunctionEstimate == false)
                return false;

            if (loopCount % valueFunctionEstimateUpdatePeriod != 0)
                return false;

            int stopStageIndex = ApproximationControl.Instance.GetValueFunctionEstimateStopStageIndex();

            if (stageIndex >= stopStageIndex)
                return false;

            return true;
        }
    }
}
