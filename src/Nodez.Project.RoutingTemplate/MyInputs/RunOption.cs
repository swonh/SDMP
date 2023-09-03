using Nodez.Data;
using Nodez.Data.Interface;
using Nodez.Sdmp.Routing.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nodez.Project.RoutingTemplate.MyInputs
{
    public class RunOption : IInputRow, IRunOptionData
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)

        public string OPTION_NAME { get; private set; }

        public string OPTION_VALUE { get; private set; }

        public RunOption()
        {
            // Define keys here (You can search data with the key defined here. Allow multiple keys)

            HashSet<string> key = new HashSet<string>();
            key.Add("ID");

            this.KeyMappings.Add(1, key);
        }
    }
}
