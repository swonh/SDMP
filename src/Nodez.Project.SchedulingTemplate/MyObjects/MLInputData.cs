// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Microsoft.ML.Data;
using Nodez.Sdmp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.SchedulingTemplate.MyObjects
{
    public class MLInputData
    { 
        public float StageIndex;

        public float[] JobProcessStatus;

        public float Makespan;

    }
}
