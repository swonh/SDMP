// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Combinatorics.Collections;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.Scheduling.Controls;
using Nodez.Sdmp.Scheduling.DataModel;
using Nodez.Sdmp.Scheduling.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.Managers
{
    public class SchedulingActionManager
    {
        private static readonly Lazy<SchedulingActionManager> lazy = new Lazy<SchedulingActionManager>(() => new SchedulingActionManager());

        public static SchedulingActionManager Instance { get { return lazy.Value; } }

        public List<General.DataModel.StateActionMap> GetStateActionMaps(SchedulingState state, ScheduleType scheduleType)
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;
            List<General.DataModel.StateActionMap> transitions = new List<StateActionMap>();

            if (scheduleType == ScheduleType.JobShop)
                transitions = this.GetJobShopActionMaps(state);
            else if (scheduleType == ScheduleType.EqpScheduling)
                transitions = this.GetEqpSchedulingActionMaps(state);

            return transitions;
        }

        private List<Equipment> GetAvailableEqps(SchedulingState state, int currentPeriod)
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;
            List<Equipment> availList = new List<Equipment>();

            for (int i = 0; i < state.EqpAvailableTime.Count(); i++)
            {
                if (state.EqpAvailableTime[i] <= currentPeriod)
                {
                    Equipment eqp = dataManager.GetEqp(i);
                    availList.Add(eqp);
                }
            }

            return availList;
        }

        private List<General.DataModel.StateActionMap> GetEqpSchedulingActionMaps(SchedulingState state)
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            ProcessControl processControl = ProcessControl.Instance;

            List<StateActionMap> transitions = new List<StateActionMap>();

            SchedulingProblem prob = dataManager.SchedulingProblem;

            foreach (Equipment eqp in prob.EqpList)
            {
                double eqpAvailTime = state.EqpAvailableTime[eqp.Index];

                EqpGroup eqpGroup = eqp.EqpGroup;

                for (int i = 0; i < state.JobProcessStatus.Count(); i++)
                {
                    Job job = dataManager.GetJob(i);

                    int currentProcessedTaskCount = state.JobProcessStatus.ElementAt(i);

                    if (currentProcessedTaskCount >= 1)
                        continue;

                    if (job.ReleaseTime > eqpAvailTime)
                        continue;

                    double procTime = dataManager.GetProcTime(job, eqp);

                    Job lastJob = dataManager.GetJob(state.EqpLastJob[eqp.Index]);

                    double setupTime = dataManager.GetSetupTime(eqp, lastJob, job);

                    double jobStartTime = eqpAvailTime + setupTime;

                    StateActionMap tran = new StateActionMap();
                    tran.PreActionState = state;

                    SchedulingState toState = new SchedulingState();
                    toState.CopyStateInfo(state);

                    toState.JobProcessStatus[i] += 1;
                    toState.EqpAvailableTime[eqp.Index] = jobStartTime + procTime;
                    toState.NextMinStartTime[i] = jobStartTime + procTime;
                    toState.JobStartTime[i] = jobStartTime;
                    toState.JobAssignedEqp[i] = eqp.Index;
                    toState.LastEqpIndex = eqp.Index;
                    toState.LastJobIndex = i;
                    toState.EqpLastJob[eqp.Index] = i;
                    toState.ParentJobEndCount[job.ParentJob.Index] += 1;
                    toState.ParentJobAssignedEqp[job.ParentJob.Index] = eqp.Index;

                    tran.PostActionState = toState;

                    toState.SetMakeSpan();

                    tran.Cost = toState.Makespan - state.Makespan;

                    if (state.Stage.Index == prob.JobList.Count() - 1)
                        toState.IsLastStage = true;

                    transitions.Add(tran);
                }

            }

            return transitions;
        }

        private List<General.DataModel.StateActionMap> GetJobShopActionMaps(SchedulingState state) 
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            List<General.DataModel.StateActionMap> transitions = new List<General.DataModel.StateActionMap>();

            int planHorizon = dataManager.SchedulingProblem.PlanInfo.PlanningHorizon;

            for (int i = 0; i < state.JobProcessStatus.Count(); i++)
            {
                Job job = dataManager.GetJob(i);
                int taskCount = job.Process.TaskList.Count;
                int currentProcessedTaskCount = state.JobProcessStatus.ElementAt(i);
                double nextMinStartTime = state.NextMinStartTime.ElementAt(i);
                int remainTaskCount = taskCount - currentProcessedTaskCount;

                if (remainTaskCount <= 0)
                    continue;

                if (job.ReleaseTime > nextMinStartTime)
                    continue;

                int taskSeq = currentProcessedTaskCount + 1;

                double procTime = dataManager.GetProcTime(job.Process.ProcessID, taskSeq);
                Equipment eqp = dataManager.GetEqp(job.Process.ProcessID, taskSeq);

                double eqpAvailTime = state.EqpAvailableTime.ElementAt(eqp.Index);

                double availableTime = eqpAvailTime;
                int comp = nextMinStartTime.CompareTo(eqpAvailTime);
                if (comp > 0)
                    availableTime = nextMinStartTime;

                if (job.ReleaseTime > availableTime)
                    continue;

                General.DataModel.StateActionMap tran = new General.DataModel.StateActionMap();
                tran.PreActionState = state;

                SchedulingState toState = new SchedulingState();
                toState.CopyStateInfo(state);

                toState.JobProcessStatus[i] += 1;
                toState.EqpAvailableTime[eqp.Index] = availableTime + procTime;
                toState.NextMinStartTime[i] = availableTime + procTime;
                toState.LastEqpIndex = eqp.Index;
                toState.LastJobIndex = i;

                tran.PostActionState = toState;

                toState.SetMakeSpan();

                tran.Cost = toState.Makespan - state.Makespan;

                if (state.Stage.Index == dataManager.SchedulingProblem.TotalTaskCount - 1)
                    toState.IsLastStage = true;

                transitions.Add(tran);
            }

            return transitions;
        }
    }
}
