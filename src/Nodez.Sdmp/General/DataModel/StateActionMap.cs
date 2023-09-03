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
