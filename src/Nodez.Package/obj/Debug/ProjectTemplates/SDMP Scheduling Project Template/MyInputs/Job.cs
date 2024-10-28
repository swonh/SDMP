// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data;
using Nodez.Data.Interface;
using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace $safeprojectname$.MyInputs
{
    public class Job : IInputRow, IJobData
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)

        public string JOB_ID { get; private set; }

        public string NAME { get; private set; }

        public string PRODUCT_ID { get; private set; }

        public string PROCESS_ID { get; private set; }

        public string STEP_SEQ { get; private set; }

        public double RELEASE_TIME { get; private set; }

        public int SPLIT_COUNT { get; private set; }

        public int QTY { get; private set; }

        public string PARENT_JOB_ID { get; private set; }

        public int PRIORITY { get; private set; }

        public string PROPERTY_ID { get; private set; }

        public bool IS_ACT { get; private set; }


        public Job()
        {
            // Define keys here (You can search data with the key defined here. Allow multiple keys)

            HashSet<string> key = new HashSet<string>();
            key.Add("JOB_ID");

            this.KeyMappings.Add(1, key);
        }
    }
}
