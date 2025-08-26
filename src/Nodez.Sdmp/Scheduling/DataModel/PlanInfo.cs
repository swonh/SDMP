// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Scheduling.Interfaces;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class PlanInfo
    {
        public int PlanningHorizon { get; set; }

        public int PriorityLookahead { get; set; }

        public IPlanInfoData PlanInfoData { get; set; }
    }
}
