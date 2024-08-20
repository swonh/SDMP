// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class SchedulingProblem
    {
        public int TotalTaskCount { get; private set; }

        public PlanInfo PlanInfo { get; private set; }

        public List<Job> JobList { get; private set; }

        public List<Job> PrioritySortedJobList { get; private set; }

        public List<Job> ParentJobList { get; private set; }

        public List<Equipment> EqpList { get; private set; }

        public List<EqpGroup> EqpGroupList { get; private set; }

        public List<Process> ProcessList { get; private set; }

        public List<Arrange> ArrangeList { get; private set; }

        public List<SetupInfo> SetupInfoList { get; private set; }

        public List<TargetInfo> TargetInfoList { get; private set; }

        public List<PMSchedule> PMScheduleList { get; private set; }

        public Dictionary<string, Job> JobMappings { get; private set; }

        public Dictionary<string, Job> ParentJobMappings { get; private set; }

        public Dictionary<string, Equipment> EqpMappings { get; private set; }

        public Dictionary<string, EqpGroup> EqpGroupMappings { get; private set; }

        public Dictionary<string, Process> ProcessMappings { get; private set; }

        public Dictionary<Tuple<string, string>, Arrange> ArrangeMappings { get; private set; }

        public Dictionary<Tuple<string ,string, string>, SetupInfo> SetupInfoMappings { get; private set; }

        public Dictionary<Tuple<string, string>, TargetInfo> TargetInfoMappings { get; private set; }

        public Dictionary<Tuple<string, string>, PMSchedule> PMScheduleMappings { get; private set; }

        public Dictionary<int, List<Job>> PriorityJobGroups { get; private set; }

        public SchedulingProblem() 
        {
            this.JobList = new List<Job>();
            this.PrioritySortedJobList = new List<Job>();
            this.ParentJobList = new List<Job>();
            this.EqpList = new List<Equipment>();
            this.EqpGroupList = new List<EqpGroup>();
            this.ProcessList = new List<Process>();
            this.ArrangeList = new List<Arrange>();
            this.SetupInfoList = new List<SetupInfo>();
            this.TargetInfoList = new List<TargetInfo>();
            this.PMScheduleList = new List<PMSchedule>();

            this.JobMappings = new Dictionary<string, Job>();
            this.ParentJobMappings = new Dictionary<string, Job>();
            this.EqpMappings = new Dictionary<string, Equipment>();
            this.EqpGroupMappings = new Dictionary<string, EqpGroup>();
            this.ProcessMappings = new Dictionary<string, Process>();
            this.ArrangeMappings = new Dictionary<Tuple<string, string>, Arrange>();
            this.SetupInfoMappings = new Dictionary<Tuple<string, string, string>, SetupInfo>();
            this.TargetInfoMappings = new Dictionary<Tuple<string, string>, TargetInfo>();
            this.PMScheduleMappings = new Dictionary<Tuple<string, string>, PMSchedule>();
            this.PriorityJobGroups = new Dictionary<int, List<Job>>();
        }

        public void SetTotalTaskCount()
        {
            int taskCount = 0;
            foreach (Job job in this.JobList)
            {
                if (job.Process == null)
                    continue;

                taskCount += job.Process.TaskList.Count;
            }

            this.TotalTaskCount = taskCount;
        }

        public void SetEqpTotalAvailableTime()
        {
            double planTime = this.PlanInfo.PlanningHorizon;

            foreach (Equipment eqp in this.EqpList)
            {
                double pmTime = 0;
                foreach (PMSchedule pm in eqp.PMSchedules)
                {
                    pmTime += (pm.PmEndTime - pm.PmStartTime);
                }

                if (pmTime >= planTime)
                    eqp.IsBlocked = true;
            }
        }

        public void SetPlanInfoObjects(List<PlanInfo> planInfoList) 
        {
            this.PlanInfo = planInfoList.FirstOrDefault();
        }

        public void SetJobObjects(List<Job> jobList)
        {
            this.JobList = jobList;
            this.SetJobMappings();

            this.PrioritySortedJobList = jobList.OrderBy(x => x.Priority).ToList();
        }

        public void SetParentJobObjects(List<Job> parentJobList) 
        {
            this.ParentJobList = parentJobList;
            this.SetParentJobMappings();

            this.SetPriorityJobGroups();
        }

        public void SetEquipmentObjects(List<Equipment> eqpList) 
        {
            this.EqpList = eqpList;
            this.SetEqpMappings();
        }

        public void SetEqpGroupObjects(List<EqpGroup> eqpGroupList)
        {
            this.EqpGroupList = eqpGroupList;
            this.SetEqpGroupMappings();
        }

        public void SetProcessObjects(List<Process> processList) 
        {
            this.ProcessList = processList;
            this.SetProcessMappings();
        }

        public void SetArrangeObjects(List<Arrange> arrangeList)
        {
            this.ArrangeList = arrangeList;
            this.SetArrangeMappings();
        }

        public void SetSetupInfoObjects(List<SetupInfo> setupInfoList) 
        {
            this.SetupInfoList = setupInfoList;
            this.SetSetupInfoMappings();
        }

        public void SetPMScheduleObjects(List<PMSchedule> pmScheduleList)
        {
            this.PMScheduleList = pmScheduleList;
            this.SetPMScheduleMappings();
        }

        public void SetTargetInfoObjects(List<TargetInfo> targetInfoList)
        {
            this.TargetInfoList = targetInfoList;
            this.SetTargetInfoMappings();
        }

        public void SetTargetInfoMappings()
        {
            foreach (TargetInfo targetInfo in this.TargetInfoList)
            {
                Tuple<string, string> key = Tuple.Create(targetInfo.ProcessID, targetInfo.StepSeq);
                if (this.TargetInfoMappings.ContainsKey(key))
                    continue;

                this.TargetInfoMappings.Add(key, targetInfo);
            }
        }

        private void SetPriorityJobGroups() 
        {
            Dictionary<int, List<Job>> group = new Dictionary<int, List<Job>>();

            int lookAhead = PlanInfo.PriorityLookahead;
            foreach (Job parentJob in this.ParentJobList)
            {
                double releaseTime = parentJob.ReleaseTime;

                foreach (Job otherJob in this.ParentJobList) 
                {
                    if (parentJob.Index == otherJob.Index)
                        continue;

                    if (otherJob.ReleaseTime < releaseTime - lookAhead)
                        continue;

                    if (otherJob.ReleaseTime > releaseTime + lookAhead)
                        continue;

                    if (group.TryGetValue(parentJob.Index, out List<Job> list))
                    {
                        list.Add(otherJob);
                    }
                    else
                    {
                        group.Add(parentJob.Index, new List<Job>() { otherJob });
                    }
                }
            }

            this.PriorityJobGroups = group;
        }

        public void SetSetupInfoMappings() 
        {
            foreach (SetupInfo setupInfo in this.SetupInfoList) 
            {
                if (this.SetupInfoMappings.ContainsKey(Tuple.Create(setupInfo.EqpID, setupInfo.FromPropertyID, setupInfo.ToPropertyID)))
                    continue;

                this.SetupInfoMappings.Add(Tuple.Create(setupInfo.EqpID, setupInfo.FromPropertyID, setupInfo.ToPropertyID), setupInfo);
            }
        }

        public void SetPMScheduleMappings()
        {
            foreach (PMSchedule pmSchedule in this.PMScheduleList)
            {
                if (this.PMScheduleMappings.ContainsKey(Tuple.Create(pmSchedule.EqpID, pmSchedule.ModuleID)))
                    continue;

                this.PMScheduleMappings.Add(Tuple.Create(pmSchedule.EqpID, pmSchedule.ModuleID), pmSchedule);
            }
        }

        public void SetArrangeMappings() 
        {
            foreach (Arrange arrange in this.ArrangeList) 
            {
                if (this.ArrangeMappings.ContainsKey(Tuple.Create(arrange.PropertyID, arrange.EqpID)))
                    continue;

                this.ArrangeMappings.Add(Tuple.Create(arrange.PropertyID, arrange.EqpID), arrange);
            }
        }

        public void SetJobMappings() 
        {
            foreach (Job job in this.JobList)
            {
                if (this.JobMappings.ContainsKey(job.JobID))
                    continue;

                this.JobMappings.Add(job.JobID, job);
            }
        }

        public void SetParentJobMappings()
        {
            foreach (Job job in this.ParentJobList)
            {
                if (this.ParentJobMappings.ContainsKey(job.JobID))
                    continue;

                this.ParentJobMappings.Add(job.JobID, job);
            }
        }

        public void SetEqpGroupMappings()
        {
            foreach (EqpGroup eqpGroup in this.EqpGroupList)
            {
                if (this.EqpGroupMappings.ContainsKey(eqpGroup.GroupID))
                    continue;

                this.EqpGroupMappings.Add(eqpGroup.GroupID, eqpGroup);
            }
        }

        public void SetEqpMappings()
        {
            foreach (Equipment eqp in this.EqpList)
            {
                if (this.EqpMappings.ContainsKey(eqp.EqpID))
                    continue;

                this.EqpMappings.Add(eqp.EqpID, eqp);
            }
        }

        public void SetProcessMappings()
        {
            foreach (Process process in this.ProcessList)
            {
                if (this.ProcessMappings.ContainsKey(process.ProcessID))
                    continue;

                this.ProcessMappings.Add(process.ProcessID, process);
            }
        }

    }
}
