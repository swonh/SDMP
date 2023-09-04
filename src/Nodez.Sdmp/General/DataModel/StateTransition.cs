// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.General.DataModel
{
    public class StateTransition
    {
        public int Index { get; set; }

        public State FromState { get; set; }

        public State ToState { get; set; }

        public double Cost { get; set; }

        public void SetIndex(int index)
        {
            this.Index = index;
        }
    }
}
