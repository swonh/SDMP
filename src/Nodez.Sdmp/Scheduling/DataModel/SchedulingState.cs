// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.Scheduling.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class SchedulingState : State
    {
        public int[] JobProcessStatus { get; set; }

        public double[] NextMinStartTime { get; set; }

        public double[] JobStartTime { get; set; }

        public double[] JobSetupStartTime { get; set; }

        public int[] ParentJobEndCount { get; set; }

        public int[] ParentJobAssignedEqp { get; set; }

        public int[] JobAssignedEqp { get; set; }

        public double[] EqpAvailableTime { get; set; }

        public int[] EqpLastJob { get; set; }

        public int[] RemainTarget { get; set; }

        public int LastJobIndex { get; set; }

        public int LastEqpIndex { get; set; }

        public double Makespan { get; set; }

        public int RemainTargetValue { get; set; }

        public SchedulingState(string key)
        {
            this.Key = key;
        }

        public SchedulingState()
        {
            this.PrevStates = new Dictionary<string, State>();
            this.PrevBestStates = new Dictionary<string, State>();
        }

        public virtual void Initialize()
        {
            this.InitJobProcessStatus();
            this.InitJobStartTime();
            this.InitJobSetupStartTime();
            this.InitParentJobEndCount();
            this.InitJobAssignedEqp();
            this.InitParentJobAssignedEqp();
            this.InitEqpLastJob();
            this.InitNextMinStartTime();
            this.InitEqpAvailableTime();
            this.InitMakeSpan();
            this.InitRemainTarget();
            this.InitRemainTargetValue();
        }

        public void SetMakeSpan()
        {
            this.Makespan = EqpAvailableTime.Max();
        }

        public void SetRemainTargetValue()
        {
            this.RemainTargetValue = this.RemainTarget.Sum();
        }

        public string GetKey()
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("Last Eqp:{0}@", this.LastEqpIndex);
            str.AppendFormat("Last Job:{0}@", this.LastJobIndex);

            str.AppendFormat("|");

            foreach (var item in this.JobProcessStatus)
            {
                str.AppendFormat("{0}@", item);
            }

            str.AppendFormat("|");

            foreach (var item in this.EqpAvailableTime)
            {
                str.AppendFormat("{0}@", item);
            }

            return str.ToString();
        }

        public void InitMakeSpan()
        {
            this.Makespan = 0;
        }

        public void InitRemainTargetValue()
        {
            this.RemainTargetValue = this.RemainTarget.Sum();
        }

        public void InitRemainTarget()
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            SchedulingProblem prob = dataManager.SchedulingProblem;
            this.RemainTarget = new int[prob.TargetInfoList.Count];

            foreach (TargetInfo target in prob.TargetInfoList)
            {
                this.RemainTarget[target.Index] = target.TargetQty;
            }
        }

        public void InitEqpLastJob()
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            SchedulingProblem prob = dataManager.SchedulingProblem;

            this.EqpLastJob = new int[prob.EqpList.Count];

            for (int i = 0; i < this.EqpLastJob.Length; i++)
            {
                this.EqpLastJob[i] = -1;
            }
        }

        public void InitJobAssignedEqp()
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            SchedulingProblem prob = dataManager.SchedulingProblem;

            this.JobAssignedEqp = new int[prob.JobList.Count];

            for (int i = 0; i < this.JobAssignedEqp.Length; i++)
            {
                this.JobAssignedEqp[i] = -1;
            }
        }

        public void InitParentJobAssignedEqp()
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            SchedulingProblem prob = dataManager.SchedulingProblem;

            this.ParentJobAssignedEqp = new int[prob.ParentJobList.Count];

            for (int i = 0; i < this.ParentJobAssignedEqp.Length; i++)
            {
                this.ParentJobAssignedEqp[i] = -1;
            }
        }

        public void InitJobStartTime()
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            SchedulingProblem prob = dataManager.SchedulingProblem;

            this.JobStartTime = new double[prob.JobList.Count];

            for (int i = 0; i < this.JobStartTime.Length; i++)
            {
                this.JobStartTime[i] = -1;
            }
        }

        public void InitJobSetupStartTime()
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            SchedulingProblem prob = dataManager.SchedulingProblem;

            this.JobSetupStartTime = new double[prob.JobList.Count];

            for (int i = 0; i < this.JobSetupStartTime.Length; i++)
            {
                this.JobSetupStartTime[i] = -1;
            }
        }

        public void InitParentJobEndCount()
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            SchedulingProblem prob = dataManager.SchedulingProblem;

            this.ParentJobEndCount = new int[prob.ParentJobList.Count];

            for (int i = 0; i < this.ParentJobEndCount.Length; i++)
            {
                this.ParentJobEndCount[i] = 0;
            }
        }

        public void InitJobProcessStatus()
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            SchedulingProblem prob = dataManager.SchedulingProblem;

            this.JobProcessStatus = new int[prob.JobList.Count];

            for (int i = 0; i < this.JobProcessStatus.Length; i++)
            {
                this.JobProcessStatus[i] = 0;
            }
        }

        public void InitEqpAvailableTime()
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            SchedulingProblem prob = dataManager.SchedulingProblem;

            this.EqpAvailableTime = new double[prob.EqpList.Count];

            for (int i = 0; i < this.EqpAvailableTime.Length; i++)
            {
                this.EqpAvailableTime[i] = 0;
            }
        }

        public void InitNextMinStartTime()
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            SchedulingProblem prob = dataManager.SchedulingProblem;

            this.NextMinStartTime = new double[prob.JobList.Count];

            for (int i = 0; i < this.NextMinStartTime.Length; i++)
            {
                this.NextMinStartTime[i] = 0;
            }
        }

        public new SchedulingState Clone()
        {
            SchedulingState clone = (SchedulingState)this.MemberwiseClone();

            clone.CopyStateInfo(this);

            return clone;
        }

        public virtual void CopyStateInfo(SchedulingState state)
        {
            int[] jobProcStatus = state.JobProcessStatus;
            double[] jobStartTime = state.JobStartTime;
            double[] jobSetupStartTime = state.JobSetupStartTime;
            int[] jobAssignedEqp = state.JobAssignedEqp;
            double[] nextMinStartTime = state.NextMinStartTime;
            double[] eqpAvailableTime = state.EqpAvailableTime;
            int[] eqpLastJob = state.EqpLastJob;
            int[] parentJobEndCount = state.ParentJobEndCount;
            int[] parentJobAssignedEqp = state.ParentJobAssignedEqp;
            int[] remainTarget = state.RemainTarget;

            this.JobProcessStatus = new int[jobProcStatus.Length];
            this.JobStartTime = new double[jobStartTime.Length];
            this.JobSetupStartTime = new double[jobSetupStartTime.Length];
            this.JobAssignedEqp = new int[jobAssignedEqp.Length];
            this.NextMinStartTime = new double[nextMinStartTime.Length];
            this.EqpAvailableTime = new double[eqpAvailableTime.Length];
            this.EqpLastJob = new int[eqpLastJob.Length];
            this.ParentJobEndCount = new int[parentJobEndCount.Length];
            this.ParentJobAssignedEqp = new int[parentJobAssignedEqp.Length];
            this.RemainTarget = new int[remainTarget.Length];

            this.LastEqpIndex = state.LastEqpIndex;
            this.LastJobIndex = state.LastJobIndex;

            Buffer.BlockCopy(jobProcStatus, 0, this.JobProcessStatus, 0, jobProcStatus.Length * sizeof(int));
            Buffer.BlockCopy(jobStartTime, 0, this.JobStartTime, 0, jobStartTime.Length * sizeof(double));
            Buffer.BlockCopy(jobSetupStartTime, 0, this.JobSetupStartTime, 0, jobSetupStartTime.Length * sizeof(double));
            Buffer.BlockCopy(jobAssignedEqp, 0, this.JobAssignedEqp, 0, jobAssignedEqp.Length * sizeof(int));
            Buffer.BlockCopy(nextMinStartTime, 0, this.NextMinStartTime, 0, nextMinStartTime.Length * sizeof(double));
            Buffer.BlockCopy(eqpAvailableTime, 0, this.EqpAvailableTime, 0, eqpAvailableTime.Length * sizeof(double));
            Buffer.BlockCopy(eqpLastJob, 0, this.EqpLastJob, 0, eqpLastJob.Length * sizeof(int));
            Buffer.BlockCopy(parentJobEndCount, 0, this.ParentJobEndCount, 0, parentJobEndCount.Length * sizeof(int));
            Buffer.BlockCopy(parentJobAssignedEqp, 0, this.ParentJobAssignedEqp, 0, parentJobAssignedEqp.Length * sizeof(int));
            Buffer.BlockCopy(remainTarget, 0, this.RemainTarget, 0, remainTarget.Length * sizeof(int));
        }
    }
}
