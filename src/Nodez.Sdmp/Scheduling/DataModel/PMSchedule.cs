// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class PMSchedule
    {
        public string PMID { get; set; }

        public string EqpID { get; set; }

        public string ModuleID { get; set; }

        public double PmStartTime { get; set; }

        public double PmEndTime { get; set; }
    }
}
