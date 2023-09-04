// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Priority_Queue;
using Nodez.Data.Interface;
using Nodez.Data.Managers;
using Nodez.Sdmp.Comparer;
using Nodez.Sdmp.Constants;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Managers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Nodez.Sdmp.Interfaces;

namespace Nodez.Sdmp.General.Solver
{
    public class GeneralSolver : ISolver
    {
        public GeneralSolver(IRunConfig runConfig)
        {
            this.RunConfig = runConfig;
            this.StopWatch = new Stopwatch();
            this.VisitedStates = new Dictionary<string, State>();
            this.TransitionQueue = new FastPriorityQueue<State>(Convert.ToInt32(9 * Math.Pow(10, 7)));
            //FastPriorityQueue - Convert.ToInt32(9 * Math.Pow(10, 7)
        }

    }
}
