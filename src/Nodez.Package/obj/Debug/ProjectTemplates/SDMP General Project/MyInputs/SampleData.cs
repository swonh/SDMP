using Nodez.Data;
using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace $safeprojectname$.MyInputs
{
    public class SampleData : IInputRow
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)

        public string COL_1 { get; private set; }

        public string COL_2 { get; private set; }

        public SampleData()
        {
            // Define keys here (You can search data with the key defined here. Allow multiple keys)
      
            HashSet<string> key = new HashSet<string>();
            key.Add("COL_1");

            this.KeyMappings.Add(1, key);

            HashSet<string> key2 = new HashSet<string>();
            key2.Add("COL_1");
            key2.Add("COL_2");

            this.KeyMappings.Add(2, key);
        }
    }
}
