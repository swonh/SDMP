// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Text;

namespace Nodez.Sdmp.General.DataModel
{
    public class StateActionMap
    {
        public int Index { get; set; }

        public State PreActionState { get; set; }

        public State PostActionState { get; set; }

        public double Cost { get; set; }

        public void SetIndex(int index) 
        {
            this.Index = index;
        }

    }
}
