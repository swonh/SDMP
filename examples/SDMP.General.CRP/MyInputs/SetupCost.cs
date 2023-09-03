using Nodez.Data;
using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SDMP.General.CRP.MyInputs
{
    public class SetupCost : IInputRow
    {
        public int FROM_JOB { get; private set; }

        public int TO_JOB { get; private set; }

        public double SETUP_COST { get; private set; }


        public SetupCost()
        {
            HashSet<string> key = new HashSet<string>();
            key.Add("FROM_JOB");
            key.Add("TO_JOB");

            this.KeyMappings.Add(1, key);

            HashSet<string> key2 = new HashSet<string>();
            key2.Add("FROM_JOB");

            this.KeyMappings.Add(2, key2);
        }
    }
}
