// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data;
using Nodez.Data.Interface;
using Nodez.Sdmp.Routing.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace $safeprojectname$.MyInputs
{
    public class Node : IInputRow, INodeData
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)

        public string ID { get; private set; }

        public string NAME { get; private set; }

        public string ORDER_ID { get; private set; }

        public double START_TIME_WINDOW { get; private set; }

        public double END_TIME_WINDOW { get; private set; }

        public double X_COORDINATE { get; private set; }

        public double Y_COORDINATE { get; private set; }

        public string IS_DELIVERY { get; private set; }

        public string IS_DEPOT { get; private set; }

        public Node()
        {
            // Define keys here (You can search data with the key defined here. Allow multiple keys)

            HashSet<string> key = new HashSet<string>();
            key.Add("ID");

            this.KeyMappings.Add(1, key);
        }
    }
}
