// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Scheduling.DataModel;
using System;

namespace Nodez.Sdmp.Scheduling.Managers
{
    public class SchedulingBoundManager
    {
        private static readonly Lazy<SchedulingBoundManager> lazy = new Lazy<SchedulingBoundManager>(() => new SchedulingBoundManager());

        public static SchedulingBoundManager Instance { get { return lazy.Value; } }

        public double GetDualBound(SchedulingState state)
        {
            return 0;
        }
    }
}
