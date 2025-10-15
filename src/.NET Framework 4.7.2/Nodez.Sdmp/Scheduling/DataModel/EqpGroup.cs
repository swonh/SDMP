// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Scheduling.Interfaces;
using System.Collections.Generic;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class EqpGroup
    {
        public int Index { get; set; }

        public string GroupID { get; set; }

        public string GroupName { get; set; }

        public List<Equipment> EqpList { get; set; }

        public IEqpGroupData EqpGroupData { get; set; }

        public List<Arrange> ArrangeList { get; set; }
    }
}
