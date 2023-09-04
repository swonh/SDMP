// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.DataModel;
using Nodez.Data.Managers;
using Nodez.Sdmp.Scheduling.DataModel;
using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nodez.Sdmp.Scheduling.Managers
{
    public class SchedulingDataManager
    {
        private static readonly Lazy<SchedulingDataManager> lazy = new Lazy<SchedulingDataManager>(() => new SchedulingDataManager());

        public static SchedulingDataManager Instance { get { return lazy.Value; } }

        public SchedulingData SchedulingData { get; private set; }

        public SchedulingProblem SchedulingProblem { get; private set; }

        public InputTable PlanInfoTable { get; private set; }

        public InputTable JobTable { get; private set; }

        public InputTable EqpTable { get; private set; }

        public InputTable EqpGroupTable { get; private set; }

        public InputTable ProcessTable { get; private set; }

        public InputTable ArrangeTable { get; private set; }

        public InputTable SetupInfoTable { get; private set; }

        public InputTable TargetInfoTable { get; private set; }

        public InputTable PMScheduleTable { get; private set; }

        public void InitializeSchedulingData()
        {
            InputManager inputManager = InputManager.Instance;

            this.PlanInfoTable = inputManager.GetInput(Constants.Constants.PLAN_INFO);
            this.JobTable = inputManager.GetInput(Constants.Constants.JOB);
            this.EqpTable = inputManager.GetInput(Constants.Constants.EQUIPMENT);
            this.EqpGroupTable = inputManager.GetInput(Constants.Constants.EQUIPMENT_GROUP);
            this.ProcessTable = inputManager.GetInput(Constants.Constants.PROCESS);
            this.ArrangeTable = inputManager.GetInput(Constants.Constants.ARRANGE);
            this.SetupInfoTable = inputManager.GetInput(Constants.Constants.SETUP_INFO);
            this.TargetInfoTable = inputManager.GetInput(Constants.Constants.TARGET_INFO);
            this.PMScheduleTable = inputManager.GetInput(Constants.Constants.PM_SCHEDULE);

            SchedulingData schedulingData = new SchedulingData();

            if (this.PlanInfoTable != null)
            {
                schedulingData.SetPlanInfoData(PlanInfoTable.Rows().Cast<IPlanInfoData>().ToList());
            }

            if (this.JobTable != null)
            {
                schedulingData.SetJobDataList(JobTable.Rows().Cast<IJobData>().ToList());
            }

            if (this.EqpTable != null)
            {
                schedulingData.SetEqpDataList(EqpTable.Rows().Cast<IEqpData>().ToList());
            }

            if (this.EqpGroupTable != null)
            {
                schedulingData.SetEqpGroupDataList(EqpGroupTable.Rows().Cast<IEqpGroupData>().ToList());
            }

            if (this.ProcessTable != null)
            {
                schedulingData.SetProcessDataList(ProcessTable.Rows().Cast<IProcessData>().ToList());
            }

            if (this.ArrangeTable != null)
            {
                schedulingData.SetArrangeDataList(ArrangeTable.Rows().Cast<IArrangeData>().ToList());
            }

            if (this.SetupInfoTable != null)
            {
                schedulingData.SetSetupInfoDataList(SetupInfoTable.Rows().Cast<ISetupInfoData>().ToList());
            }

            if (this.TargetInfoTable != null)
            {
                schedulingData.SetTargetInfoDataList(TargetInfoTable.Rows().Cast<ITargetInfoData>().ToList());
            }

            if (this.PMScheduleTable != null && this.PMScheduleTable.Rows() != null)
            {
                schedulingData.SetPMScheduleDataList(PMScheduleTable.Rows().Cast<IPMScheduleData>().ToList());
            }

            this.SchedulingData = schedulingData;
        }

        public void InitializeSchedulingProblem()
        {
            if (this.SchedulingData == null)
                return;

            if (this.SchedulingData.PlanInfoDataList == null)
                return;

            if (this.SchedulingData.JobDataList == null)
                return;

            if (this.SchedulingData.EqpDataList == null)
                return;

            if (this.SchedulingData.EqpGroupDataList == null)
                return;

            if (this.SchedulingData.ProcessDataList == null)
                return;

            if (this.SchedulingData.ArrangeDataList == null)
                return;

            if (this.SchedulingData.SetupInfoDataList == null)
                return;

            this.SchedulingProblem = new SchedulingProblem();

            this.SchedulingProblem.SetPlanInfoObjects(this.CreatePlanInfos(this.SchedulingData.PlanInfoDataList));
            this.SchedulingProblem.SetTargetInfoObjects(this.CreateTargetInfos(this.SchedulingData.TargetInfoDataList));
            this.SchedulingProblem.SetArrangeObjects(this.CreateArranges(this.SchedulingData.ArrangeDataList));
            this.SchedulingProblem.SetEquipmentObjects(this.CreateEqps(this.SchedulingData.EqpDataList));
            this.SchedulingProblem.SetEqpGroupObjects(this.CreateEqpGroups(this.SchedulingData.EqpGroupDataList));
            this.SchedulingProblem.SetProcessObjects(this.CreateProcesses(this.SchedulingData.ProcessDataList));
            this.SchedulingProblem.SetJobObjects(this.CreateJobs(this.SchedulingData.JobDataList));
            this.SchedulingProblem.SetParentJobObjects(this.CreateParentJobs(this.SchedulingData.JobDataList));
            this.SchedulingProblem.SetSetupInfoObjects(this.CreateSetupInfos(this.SchedulingData.SetupInfoDataList));
            this.SchedulingProblem.SetPMScheduleObjects(this.CreatePMSchedules(this.SchedulingData.PMScheduleDataList));

            this.SchedulingProblem.SetEqpTotalAvailableTime();
            this.SchedulingProblem.SetTotalTaskCount();
        }

        private List<TargetInfo> CreateTargetInfos(List<ITargetInfoData> targetInfoDataList)
        {
            List<TargetInfo> infos = new List<TargetInfo>();

            if (targetInfoDataList == null)
                return infos;

            int index = 0;
            foreach (ITargetInfoData targetInfoData in targetInfoDataList)
            {
                TargetInfo targetInfo = new TargetInfo();

                targetInfo.Index = index;
                targetInfo.TargetID = targetInfoData.TARGET_ID;
                targetInfo.ProcessID = targetInfoData.PROCESS_ID;
                targetInfo.StepSeq = targetInfoData.STEP_SEQ;
                targetInfo.TargetQty = targetInfoData.TARGET_QTY;
                targetInfo.TargetInfoData = targetInfoData;

                infos.Add(targetInfo);

                index++;
            }

            return infos;
        }

        private List<SetupInfo> CreateSetupInfos(List<ISetupInfoData> setupInfoDataList)
        {
            List<SetupInfo> infos = new List<SetupInfo>();

            foreach (ISetupInfoData setupInfoData in setupInfoDataList)
            {
                SetupInfo setupInfo = new SetupInfo();

                setupInfo.EqpID = setupInfoData.EQP_ID;
                setupInfo.FromPropertyID = setupInfoData.FROM_PROPERTY_ID;
                setupInfo.ToPropertyID = setupInfoData.TO_PROPERTY_ID;
                setupInfo.SetupTime = setupInfoData.SETUP_TIME;
                setupInfo.SetupInfoData = setupInfoData;

                infos.Add(setupInfo);
            }

            return infos;
        }

        private List<PMSchedule> CreatePMSchedules(List<IPMScheduleData> pmScheduleDataList)
        {
            List<PMSchedule> pmScheds = new List<PMSchedule>();

            if (pmScheduleDataList == null)
                return pmScheds;

            IOrderedEnumerable<IPMScheduleData> orderedList = pmScheduleDataList.OrderBy(x => x.EQP_ID).ThenBy(x => x.PM_START_TIME);
            foreach (IPMScheduleData pmData in orderedList)
            {
                PMSchedule pmSched = new PMSchedule();

                pmSched.EqpID = pmData.EQP_ID;
                pmSched.ModuleID = pmData.MODULE_ID;
                pmSched.PmEndTime = pmData.PM_END_TIME;
                pmSched.PMID = pmData.PM_ID;
                pmSched.PmStartTime = pmData.PM_START_TIME;

                Equipment eqp = this.GetEqp(pmData.MODULE_ID);

                if (eqp != null)
                {
                    eqp.PMSchedules.Add(pmSched);
                }

                pmScheds.Add(pmSched);
            }

            return pmScheds;
        }

        private List<PlanInfo> CreatePlanInfos(List<IPlanInfoData> planInfoDataList)
        {
            List<PlanInfo> plans = new List<PlanInfo>();

            foreach (IPlanInfoData planData in planInfoDataList)
            {
                PlanInfo planInfo = new PlanInfo();

                planInfo.PlanningHorizon = planData.PLANNING_HORIZON;
                planInfo.PriorityLookahead = planData.PRIORITY_LOOKAHEAD;

                plans.Add(planInfo);
            }

            return plans;
        }

        private List<Arrange> CreateArranges(List<IArrangeData> arrangeDataList)
        {
            List<Arrange> arranges = new List<Arrange>();

            int index = 0;
            foreach (IArrangeData arrangeData in arrangeDataList)
            {
                Arrange arrange = new Arrange();

                arrange.Index = index;
                arrange.ProductID = arrangeData.PRODUCT_ID;
                arrange.PropertyID = arrangeData.PROPERTY_ID;
                arrange.EqpID = arrangeData.EQP_ID;
                arrange.ProcTime = arrangeData.PROC_TIME;

                arranges.Add(arrange);

                index++;
            }

            return arranges;
        }

        private List<Job> CreateJobs(List<IJobData> jobDataList)
        {
            List<Job> jobs = new List<Job>();

            Dictionary<string, Job> parentJobs = new Dictionary<string, Job>();

            int parentJobIndex = 0;
            foreach (IJobData jobData in jobDataList)
            {
                if (parentJobs.ContainsKey(jobData.PARENT_JOB_ID) == false)
                {
                    Job parentJob = new Job();

                    parentJob.JobID = jobData.PARENT_JOB_ID;
                    parentJob.Name = jobData.PARENT_JOB_ID;
                    parentJob.Index = parentJobIndex;
                    parentJob.Process = this.GetProcess(jobData.PROCESS_ID);
                    parentJob.TargetInfo = this.GetTargetInfo(jobData.PROCESS_ID, jobData.STEP_SEQ);
                    parentJob.ReleaseTime = jobData.RELEASE_TIME;
                    parentJob.ProductID = jobData.PRODUCT_ID;
                    parentJob.ProcessID = jobData.PROCESS_ID;
                    parentJob.StepSeq = jobData.STEP_SEQ;
                    parentJob.ParentJob = parentJob;
                    parentJob.SplitCount = jobData.SPLIT_COUNT;
                    parentJob.Priority = jobData.PRIORITY;
                    parentJob.PropertyID = jobData.PROPERTY_ID;
                    parentJob.IsAct = jobData.IS_ACT;
                    parentJob.JobData = jobData;

                    parentJobs.Add(jobData.PARENT_JOB_ID, parentJob);

                    parentJobIndex++;
                }
            }

            int index = 0;
            foreach (IJobData jobData in jobDataList)
            {
                Job job = new Job();

                job.JobID = jobData.JOB_ID;
                job.Name = jobData.NAME;
                job.Index = index;
                job.Process = this.GetProcess(jobData.PROCESS_ID);
                job.TargetInfo = this.GetTargetInfo(jobData.PROCESS_ID, jobData.STEP_SEQ);
                job.ReleaseTime = jobData.RELEASE_TIME;
                job.ProductID = jobData.PRODUCT_ID;
                job.ProcessID = jobData.PROCESS_ID;
                job.StepSeq = jobData.STEP_SEQ;
                job.SplitCount = jobData.SPLIT_COUNT;
                job.Qty = jobData.QTY;
                job.Priority = jobData.PRIORITY;
                job.PropertyID = jobData.PROPERTY_ID;
                job.IsAct = jobData.IS_ACT;
                job.JobData = jobData;

                if (parentJobs.TryGetValue(jobData.PARENT_JOB_ID, out Job parentJob))
                {
                    job.ParentJob = parentJob;
                    parentJob.Qty += jobData.QTY;
                }

                jobs.Add(job);

                index++;
            }

            return jobs;
        }

        private List<Job> CreateParentJobs(List<IJobData> jobDataList)
        {
            List<Job> parentJobs = new List<Job>();

            Dictionary<string, Job> parentJobMappings = new Dictionary<string, Job>();

            int parentJobIndex = 0;
            foreach (IJobData jobData in jobDataList)
            {
                if (parentJobMappings.ContainsKey(jobData.PARENT_JOB_ID) == false)
                {
                    Job job = SchedulingDataManager.Instance.GetJob(jobData.JOB_ID);
                    Job parentJob = job.ParentJob;

                    parentJobMappings.Add(jobData.PARENT_JOB_ID, parentJob);
                    parentJobs.Add(parentJob);

                    parentJobIndex++;
                }
            }

            return parentJobs;
        }

        private List<Equipment> CreateEqps(List<IEqpData> eqpDataList)
        {
            List<Equipment> eqps = new List<Equipment>();

            int index = 0;
            foreach (IEqpData eqpData in eqpDataList)
            {
                Equipment eqp = new Equipment();

                eqp.EqpID = eqpData.EQP_ID;
                eqp.Name = eqpData.EQP_NAME;
                eqp.AvailableTime = 0;
                eqp.Index = index;
                eqp.PMSchedules = new List<PMSchedule>();

                eqps.Add(eqp);

                index++;
            }

            return eqps;
        }

        private List<EqpGroup> CreateEqpGroups(List<IEqpGroupData> eqpGroupDataList)
        {
            List<EqpGroup> eqpGroups = new List<EqpGroup>();

            Dictionary<string, List<Equipment>> groups = new Dictionary<string, List<Equipment>>();
            foreach (IEqpData eqpData in this.SchedulingData.EqpDataList)
            {
                Equipment eqp = GetEqp(eqpData.EQP_ID);
                if (groups.TryGetValue(eqpData.GROUP_ID, out List<Equipment> eqpList))
                    eqpList.Add(eqp);
                else
                    groups.Add(eqpData.GROUP_ID, new List<Equipment>() { eqp });
            }

            int index = 0;
            foreach (IEqpGroupData eqpGroupData in eqpGroupDataList)
            {
                EqpGroup eqpGroup = new EqpGroup();

                eqpGroup.Index = index;
                eqpGroup.GroupID = eqpGroupData.GROUP_ID;
                eqpGroup.GroupName = eqpGroupData.GROUP_NAME;
                eqpGroup.EqpGroupData = eqpGroupData;
                eqpGroup.ArrangeList = new List<Arrange>();

                if (groups.ContainsKey(eqpGroup.GroupID))
                {
                    eqpGroup.EqpList = groups[eqpGroup.GroupID];

                    foreach (Equipment eqp in eqpGroup.EqpList)
                        eqp.EqpGroup = eqpGroup;
                }

                eqpGroups.Add(eqpGroup);

                index++;
            }

            foreach (EqpGroup eqpGroup in eqpGroups)
            {
                foreach (Arrange arr in this.SchedulingProblem.ArrangeList)
                {
                    Equipment eqp = this.GetEqp(arr.EqpID);

                    if (eqp.EqpGroup.GroupID != eqpGroup.GroupID)
                        continue;

                    eqpGroup.ArrangeList.Add(arr);
                }
            }

            return eqpGroups;
        }

        private List<Process> CreateProcesses(List<IProcessData> processDataList)
        {
            List<Process> processes = new List<Process>();

            Dictionary<string, List<Tuple<string, double>>> taskLists = new Dictionary<string, List<Tuple<string, double>>>();

            processDataList = processDataList.OrderBy(x => x.SEQUENCE).ToList();

            foreach (IProcessData processData in processDataList)
            {
                if (taskLists.TryGetValue(processData.PROCESS_ID, out List<Tuple<string, double>> value))
                {
                    value.Add(Tuple.Create(processData.EQP_ID, processData.PROC_TIME));
                }
                else
                {
                    taskLists.Add(processData.PROCESS_ID, new List<Tuple<string, double>>() { Tuple.Create(processData.EQP_ID, processData.PROC_TIME) });
                }
            }

            foreach (KeyValuePair<string, List<Tuple<string, double>>> item in taskLists)
            {
                Process process = new Process();

                process.ProcessID = item.Key;

                for (int i = 0; i < item.Value.Count; i++)
                {
                    Tuple<string, double> val = item.Value.ElementAt(i);

                    if (val.Item1 == null)
                        continue;

                    process.TaskList.Add(i + 1, Tuple.Create(GetEqp(val.Item1), val.Item2));
                }

                processes.Add(process);
            }

            return processes;
        }

        public Job GetJob(int index)
        {
            if (index < 0)
                return null;

            return this.SchedulingProblem.JobList[index];
        }

        public Job GetParentJob(int index)
        {
            if (index < 0)
                return null;

            return this.SchedulingProblem.ParentJobList[index];
        }

        public Job GetParentJob(string jobID)
        {
            this.SchedulingProblem.ParentJobMappings.TryGetValue(jobID, out Job parentJob);

            return parentJob;
        }

        public Job GetJob(string jobID)
        {
            this.SchedulingProblem.JobMappings.TryGetValue(jobID, out Job job);

            return job;
        }

        public Process GetProcess(string processID)
        {
            this.SchedulingProblem.ProcessMappings.TryGetValue(processID, out Process process);

            return process;
        }

        public Equipment GetEqp(int index)
        {
            if (index < 0)
                return null;

            return this.SchedulingProblem.EqpList[index];
        }

        public Equipment GetEqp(string eqpID)
        {
            this.SchedulingProblem.EqpMappings.TryGetValue(eqpID, out Equipment eqp);

            return eqp;
        }

        public Equipment GetEqp(string processID, int sequence)
        {
            if (this.SchedulingProblem.ProcessMappings.TryGetValue(processID, out Process process))
            {
                process.TaskList.TryGetValue(sequence, out Tuple<Equipment, double> task);

                return task.Item1;
            }

            return null;
        }

        public EqpGroup GetEqpGroup(string groupID)
        {
            this.SchedulingProblem.EqpGroupMappings.TryGetValue(groupID, out EqpGroup eqpGroup);

            return eqpGroup;
        }

        public EqpGroup GetEqpGroup(int index)
        {
            if (index < 0)
                return null;

            return this.SchedulingProblem.EqpGroupList[index];
        }

        public TargetInfo GetTargetInfo(int index)
        {
            if (index < 0)
                return null;

            return this.SchedulingProblem.TargetInfoList[index];
        }

        public TargetInfo GetTargetInfo(string processID, string stepSeq)
        {
            Tuple<string, string> key = Tuple.Create(processID, stepSeq);
            if (this.SchedulingProblem.TargetInfoMappings.TryGetValue(key, out TargetInfo target))
            {
                return target;
            }

            return null;
        }

        public double GetProcTime(string processID, int sequence)
        {
            if (this.SchedulingProblem.ProcessMappings.TryGetValue(processID, out Process process))
            {
                process.TaskList.TryGetValue(sequence, out Tuple<Equipment, double> task);

                return task.Item2;
            }

            return 0;
        }

        public double GetProcTime(Job job, Equipment eqp)
        {
            if (job == null)
                return 0;

            if (this.SchedulingProblem.ArrangeMappings.TryGetValue(Tuple.Create(job.PropertyID, eqp.EqpID), out Arrange arrange))
            {
                double procTime = arrange.ProcTime * job.Qty;
                return procTime;
            }

            return 0;
        }

        public double GetMinProcTime(Job job)
        {
            if (job == null)
                return 0;

            double minProcTime = Double.MaxValue;
            foreach (Equipment eqp in this.SchedulingProblem.EqpList)
            {
                if (this.SchedulingProblem.ArrangeMappings.TryGetValue(Tuple.Create(job.PropertyID, eqp.EqpID), out Arrange arrange))
                {
                    double procTime = arrange.ProcTime * job.Qty;
                    if (minProcTime > procTime)
                    {
                        minProcTime = procTime;
                    }
                }
            }

            return minProcTime;
        }

        public double GetSetupTime(Equipment eqp, Job fromJob, Job toJob)
        {
            if (eqp == null)
                return 0;

            if (fromJob == null)
                return 0;

            if (toJob == null)
                return 0;

            if (this.SchedulingProblem.SetupInfoMappings.TryGetValue(Tuple.Create(eqp.EqpID, fromJob.PropertyID, toJob.PropertyID), out SetupInfo setupInfo))
            {
                return setupInfo.SetupTime;
            }

            return 0;
        }

        public List<Job> GetPriorityJobGroup(Job job)
        {
            if (this.SchedulingProblem.PriorityJobGroups.TryGetValue(job.Index, out List<Job> value))
            {
                return value;
            }

            return new List<Job>();
        }

        public Arrange GetArrange(Job job, Equipment eqp)
        {
            if (this.SchedulingProblem.ArrangeMappings.TryGetValue(Tuple.Create(job.PropertyID, eqp.EqpID), out Arrange value))
            {
                return value;
            }

            return null;
        }
    }
}
