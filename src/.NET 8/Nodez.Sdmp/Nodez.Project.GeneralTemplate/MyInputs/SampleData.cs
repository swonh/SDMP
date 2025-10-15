// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.Interface;
using System.Collections.Generic;

namespace Nodez.Project.GeneralTemplate.MyInputs
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
