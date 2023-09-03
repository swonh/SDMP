using Nodez.Data;
using Nodez.Data.Interface;
using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nodez.Project.SchedulingTemplate.MyInputs
{
    public class Process : IInputRow, IProcessData
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)

        public string PROCESS_ID { get; private set; }

        public int SEQUENCE { get; private set; }

        public string EQP_ID { get; private set; }

        public double PROC_TIME { get; private set; }

        public Process()
        {
            // Define keys here (You can search data with the key defined here. Allow multiple keys)

            HashSet<string> key = new HashSet<string>();
            key.Add("PROCESS_ID");

            this.KeyMappings.Add(1, key);
        }
    }
}
