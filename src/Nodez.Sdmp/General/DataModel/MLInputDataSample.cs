using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
