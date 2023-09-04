using Nodez.Data;
using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SDMP.General.CRP.MyInputs
{
    public class JobColorInfo : IInputRow
    {
        public int JOB_NUM { get; private set; }

        public int COLOR { get; private set; }


        public JobColorInfo()
        {
            HashSet<string> key = new HashSet<string>();
            key.Add("JOB_NUM");

            this.KeyMappings.Add(1, key);
        }
    }
}
