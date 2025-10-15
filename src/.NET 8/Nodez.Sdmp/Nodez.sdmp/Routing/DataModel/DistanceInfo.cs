// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Nodez.Sdmp.Routing.DataModel
{
    public class DistanceInfo
    {
        public string FromNodeID { get; set; }

        public int FromNodeIndex { get; set; }

        public string ToNodeID { get; set; }

        public int ToNodeIndex { get; set; }

        public double Distance { get; set; }

        public double Time { get; set; }
    }
}
