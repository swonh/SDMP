﻿// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Constants;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.LogHelper;
using Nodez.Sdmp.Scheduling.DataModel;
using Nodez.Sdmp.Scheduling.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.SchedulingTemplate.Controls
{
    public class UserLogControl : LogControl
    {
        private static readonly Lazy<UserLogControl> lazy = new Lazy<UserLogControl>(() => new UserLogControl());

        public static new UserLogControl Instance { get { return lazy.Value; } }

        public override void WriteSolution(Solution solution)
        {
            // Default Logic
            SchedulingDataManager manager = SchedulingDataManager.Instance;

            IOrderedEnumerable<KeyValuePair<int, State>> states = solution.States.OrderByDescending(x => x.Key);
            SchedulingState lastState =  states.FirstOrDefault().Value as SchedulingState;

            Dictionary<ValueTuple<int, int, double>, string> logs = new Dictionary<ValueTuple<int, int, double>, string>();
            for (int i = 0; i < lastState.JobAssignedEqp.Length; i++) 
            {
                int jobIdx = i;
                int eqpIdx = lastState.JobAssignedEqp[i];

                Job job = manager.GetJob(jobIdx);
                Equipment eqp = manager.GetEqp(eqpIdx);

                double startTime = lastState.JobStartTime[jobIdx];
                double procTime = manager.GetProcTime(job, eqp);
                double endTime = startTime + procTime;

                logs.Add((eqp.Index, job.Index, startTime), string.Format("Eqp:{0} | Job:{1}, StartTime:{2}, EndTime:{3}", eqp.Name, job.JobName, startTime, endTime));
            }

            var items = logs.OrderBy(x => x.Key.Item1).ThenBy(x => x.Key.Item3);

            Job lastJob = null;
            foreach (var item in items)
            {
                Job job = manager.GetJob(item.Key.Item2);
                Equipment eqp = manager.GetEqp(item.Key.Item1);

                double setupTime = manager.GetSetupTime(eqp, lastJob, job);
                double procTime = manager.GetProcTime(job, eqp);

                LogWriter.WriteLine("{0} (SETUP:{1}, PROC:{2})", item.Value, setupTime, procTime);

                lastJob = job;
            }
        }

        public override void WritePruneLog(State state)
        {
            //LogWriter.WriteLine("Prune => StateIndex:{0}, State:{1}, Stage:{2}, DualBound:{3}, BestValue:{4}, BestPrimalBound:{5}", state.Index, state.ToString(), state.Stage.Index, state.DualBound, state.BestValue, BoundManager.Instance.BestPrimalBound);
        }
    }
}
