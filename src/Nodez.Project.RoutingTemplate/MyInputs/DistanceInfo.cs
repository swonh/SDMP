﻿// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data;
using Nodez.Data.Interface;
using Nodez.Sdmp.Routing.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nodez.Project.RoutingTemplate.MyInputs
{
    public class DistanceInfo : IInputRow, IDistanceInfoData
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)

        public string FROM_NODE_ID { get; private set; }

        public string TO_NODE_ID { get; private set; }

        public double DISTANCE { get; private set; }

        public double TIME { get; private set; }

        public DistanceInfo()
        {
            // Define keys here (You can search data with the key defined here. Allow multiple keys)

            HashSet<string> key = new HashSet<string>();
            key.Add("FROM_NODE_ID");
            key.Add("TO_NODE_ID");

            this.KeyMappings.Add(1, key);
        }
    }
}
