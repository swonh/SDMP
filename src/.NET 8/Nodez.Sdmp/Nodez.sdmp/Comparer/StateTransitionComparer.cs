// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace Nodez.Sdmp.Comparer
{
    public class StateTransitionComparer : IComparer<General.DataModel.StateActionMap>
    {
        public int Compare(General.DataModel.StateActionMap x, General.DataModel.StateActionMap y)
        {
            int cmp = 0;

            cmp = x.PostActionState.CurrentBestValue.CompareTo(y.PostActionState.CurrentBestValue);

            if (cmp == 0)
                cmp = x.Index.CompareTo(y.Index);

            return cmp;
        }

        public StateTransitionComparer()
        {
        }
    }
}
