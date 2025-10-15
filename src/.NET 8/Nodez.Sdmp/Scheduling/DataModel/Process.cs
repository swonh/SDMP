// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class Process
    {
        public string ProcessID { get; set; }

        public Dictionary<int, ValueTuple<Equipment, double>> TaskList { get; set; }

        public IProcessData ProcessData { get; set; }

        public Process()
        {
            this.TaskList = new Dictionary<int, ValueTuple<Equipment, double>>();
        }
    }
}
