// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Routing.DataModel;
using Nodez.Sdmp.Routing.Logic;
using System;

namespace Nodez.Sdmp.Routing.Managers
{
    public class RoutingBoundManager
    {
        private static readonly Lazy<RoutingBoundManager> lazy = new Lazy<RoutingBoundManager>(() => new RoutingBoundManager());

        public static RoutingBoundManager Instance { get { return lazy.Value; } }

        public double GetDualBound(RoutingState state)
        {
            PrimAlgorithm prim = new PrimAlgorithm(state);
            prim.Run();

            double mstValue = prim.GetMSTValue();

            double bound = mstValue - state.CurrentBestValue;

            return bound;
        }
    }
}
