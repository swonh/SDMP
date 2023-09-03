using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Comparer
{
    public class StateTransitionComparer : IComparer<General.DataModel.StateActionMap>
    {
        public int Compare(General.DataModel.StateActionMap x, General.DataModel.StateActionMap y)
        {
            int cmp = 0;

            cmp = x.PostActionState.BestValue.CompareTo(y.PostActionState.BestValue);

            if (cmp == 0)
                cmp = x.Index.CompareTo(y.Index);

            return cmp;
        }

        public StateTransitionComparer()
        {
        }
    }
}
