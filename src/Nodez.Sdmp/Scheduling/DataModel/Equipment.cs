// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class Equipment
    {
        public int Index { get; set; }

        public string EqpID { get; set; }

        public string Name { get; set; }

        public int AvailableTime { get; set; }

        public Job LastJob { get; set; }

        public EqpGroup EqpGroup { get; set; }

        public List<PMSchedule> PMSchedules { get; set; }

        public bool IsBlocked { get; set; }

        public IEqpData EqpData { get; set; }
    }
}
