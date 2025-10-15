// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Microsoft.ML.Data;

namespace Nodez.Sdmp.General.DataModel
{
    public class MLInputDataSample
    {
        [LoadColumn(0)]
        public float StageIndex;

        [LoadColumn(1, 30)]
        [VectorType(30)]
        public float[] JobProcessStatus;

        [LoadColumn(31)]
        public float Makespan;

    }
}
