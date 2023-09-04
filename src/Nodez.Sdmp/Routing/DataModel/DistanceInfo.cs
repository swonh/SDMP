// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.DataModel
{
    public class DistanceInfo
    {
        public string FromCustomerID { get; set; }

        public int FromCustomerIndex { get; set; }

        public string ToCustomerID { get; set; }

        public int ToCustomerIndex { get; set; }

        public double Distance { get; set; }

        public double Time { get; set; }
    }
}
