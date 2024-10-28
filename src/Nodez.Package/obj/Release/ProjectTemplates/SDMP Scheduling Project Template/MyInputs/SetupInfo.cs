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
    public class SetupInfo : IInputRow, ISetupInfoData
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)
        public string EQP_ID { get; private set; }

        public string FROM_PROPERTY_ID { get; private set; }

        public string TO_PROPERTY_ID { get; private set; }

        public double SETUP_TIME { get; private set; }

        public SetupInfo()
        {
            // Define keys here (You can search data with the key defined here. Allow multiple keys)

            HashSet<string> key = new HashSet<string>();
            key.Add("EQP_ID");
            key.Add("FROM_PROPERTY_ID");
            key.Add("TO_PROPERTY_ID");

            this.KeyMappings.Add(1, key);
        }
    }
}
