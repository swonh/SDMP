using Nodez.Data;
using Nodez.Data.Interface;
using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace $safeprojectname$.MyInputs
{
    public class Equipment : IInputRow, IEqpData
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)

        public string EQP_ID { get; private set; }

        public string EQP_NAME { get; private set; }

        public string GROUP_ID { get; private set; }

        public Equipment()
        {
            // Define keys here (You can search data with the key defined here. Allow multiple keys)

            HashSet<string> key = new HashSet<string>();
            key.Add("EQP_ID");

            this.KeyMappings.Add(1, key);
        }
    }
}
