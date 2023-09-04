using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDMP.General.CRP.MyOutputs
{
    public class Result : IOutputRow
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)

        public int SEQUENCE { get; set; }

        public int CONVEYOR { get; set; }

        public int JOB { get; set; }

        public int COLOR { get; set; }

    }
}
