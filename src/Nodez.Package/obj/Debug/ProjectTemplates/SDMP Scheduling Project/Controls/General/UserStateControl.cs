using Nodez.Data;
using Nodez.Data.Managers;
using Nodez.Sdmp.Constants;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Scheduling.DataModel;
using Nodez.Sdmp.Scheduling.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $safeprojectname$.Controls
{
    public class UserStateControl : StateControl
    {
        private static readonly Lazy<UserStateControl> lazy = new Lazy<UserStateControl>(() => new UserStateControl());

        public static new UserStateControl Instance { get { return lazy.Value; } }

        public override State GetInitialState()
        {
            // Default Logic
            SchedulingState state = new SchedulingState();
            state.Initialize();

            return state;
        }

        public override string GetKey(State state)
        {
            // Default Logic
            SchedulingState schedulingState = state as SchedulingState;
            string key = schedulingState.GetKey();

            return key;
        }

        public override Solution GetFeasibleSolution(State state)
        {
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            SchedulingState schedulingState = state as SchedulingState;
            SchedulingState copiedState = schedulingState.Clone();
            copiedState.IsInitial = false;

            List<SchedulingState> states = new List<SchedulingState>();

            states.Add(schedulingState);
            states.AddRange(copiedState.GetBestStatesBackward().Cast<SchedulingState>().ToList());

            HashSet<string> filteredEqpIDs = new HashSet<string>();
            while (copiedState.Stage.Index < dataManager.SchedulingProblem.JobList.Count())
            {
                Equipment selectedEqp = null;
                double selectedAvailableTime = Int32.MaxValue;
                for (int i = 0; i < copiedState.EqpAvailableTime.Length; i++)
                {
                    Equipment eqp = dataManager.GetEqp(i);
                    double availTime = copiedState.EqpAvailableTime[i];

                    if (selectedAvailableTime > availTime)
                    {
                        selectedAvailableTime = availTime;
                        selectedEqp = eqp;
                    }
                }

                Dictionary<int, double> remainJobs = new Dictionary<int, double>();
                for (int i = 0; i < copiedState.JobProcessStatus.Length; i++)
                {
                    int status = copiedState.JobProcessStatus[i];

                    if (status > 0)
                        continue;

                    Job job = dataManager.GetJob(i);

                    if (job.ReleaseTime > selectedAvailableTime)
                        continue;

                    double processTime = dataManager.GetProcTime(job, selectedEqp);

                    if (job.SplitCount > 1)
                        processTime = (int)Math.Ceiling((decimal)processTime / job.SplitCount);

                    remainJobs.Add(job.Index, processTime);
                }

                if (remainJobs.Count <= 0)
                    break;

                var orderedJobs = remainJobs.OrderByDescending(x => x.Value);
                KeyValuePair<int, double> selected = orderedJobs.FirstOrDefault();

                Job selectedJob = dataManager.GetJob(selected.Key);
                double procTime = selected.Value;

                Job lastJob = dataManager.GetJob(copiedState.EqpLastJob[selectedEqp.Index]);

                double setupTime = dataManager.GetSetupTime(selectedEqp, lastJob, selectedJob);

                double jobStartTime = selectedAvailableTime + setupTime;

                Stage stage = new Stage(copiedState.Stage.Index + 1);
                copiedState.Stage = stage;

                copiedState.JobProcessStatus[selectedJob.Index] += 1;
                copiedState.EqpAvailableTime[selectedEqp.Index] = jobStartTime + procTime;
                copiedState.NextMinStartTime[selectedJob.Index] = jobStartTime + procTime;
                copiedState.JobStartTime[selectedJob.Index] = jobStartTime;
                copiedState.JobAssignedEqp[selectedJob.Index] = selectedEqp.Index;
                copiedState.LastEqpIndex = selectedEqp.Index;
                copiedState.LastJobIndex = selectedJob.Index;
                copiedState.EqpLastJob[selectedEqp.Index] = selectedJob.Index;
                copiedState.ParentJobEndCount[selectedJob.ParentJob.Index] += 1;
                copiedState.ParentJobAssignedEqp[selectedJob.ParentJob.Index] = selectedEqp.Index;

                copiedState.SetMakeSpan();

                copiedState.BestValue = copiedState.Makespan;

                copiedState.SetKey(copiedState.GetKey());

                states.Add(copiedState);
                copiedState = copiedState.Clone();
            }

            Solution feasibleSol = new Solution(states);

            return feasibleSol;
        }

        public override bool CanPruneByOptimality(State state, ObjectiveFunctionType objFuncType, double pruneTolerance)
        {
            BoundManager boundManager = BoundManager.Instance;

            double bestPrimalBound = boundManager.BestPrimalBound;
            double dualBound = state.DualBound;
            double bestValue = state.BestValue;

            double rootDualBound = boundManager.RootDualBound;

            if (objFuncType == ObjectiveFunctionType.Minimize)
            {
                if (dualBound < rootDualBound - bestValue)
                    dualBound = rootDualBound - bestValue;

                if (bestPrimalBound + pruneTolerance <= bestValue + dualBound)
                    return true;
                else
                    return false;
            }
            else
            {
                if (dualBound > rootDualBound - bestValue)
                    dualBound = rootDualBound - bestValue;

                if (bestPrimalBound >= bestValue + dualBound + pruneTolerance)
                    return true;
                else
                    return false;
            }
        }
    }

}
