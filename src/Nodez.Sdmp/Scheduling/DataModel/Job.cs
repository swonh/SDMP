// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class Job
    {
        public int Index { get; set; }

        public string JobID { get; set; }

        public string Name { get; set; }

        public string ProductID { get; set; }

        public string ProcessID { get; set; }

        public string StepSeq { get; set; }

        public double ReleaseTime { get; set; }

        public Process Process { get; set; }

        public Job ParentJob { get; set; }

        public List<Job> SameParentJobs { get; set; }

        public List<Job> ChildJobs { get; set; }

        public TargetInfo TargetInfo { get; set; }

        public int SplitCount { get; set; }

        public int Qty { get; set; }

        public int Priority { get; set; }

        public double MinProcTime { get; set; }

        public string PropertyID { get; set; }

        public bool IsAct { get; set; }

        public IJobData JobData { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
