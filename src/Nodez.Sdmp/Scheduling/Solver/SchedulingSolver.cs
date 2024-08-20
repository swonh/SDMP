// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Priority_Queue;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.Solver
{
    public class SchedulingSolver : ISolver
    {
        public SchedulingSolver(IRunConfig runConfig)
        {
            this.RunConfig = runConfig;
            this.StopWatch = new Stopwatch();
            this.VisitedStates = new Dictionary<string, State>();
            this.TransitionQueue = new FastPriorityQueue<State>(Convert.ToInt32(9 * Math.Pow(10, 7)));
            //FastPriorityQueue - Convert.ToInt32(9 * Math.Pow(10, 7)
        }
    }
}
