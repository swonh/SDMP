using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDMP.General.CRP.MyObjects
{
    public class CRPConveyor
    {
        public CRPFactory Factory { get; set; }

        public int ConveyorNum { get; set; }

        public ConcurrentQueue<CRPJob> Jobs { get; set; }

        public int JobCount { get { return Jobs.Count; } }

        public CRPConveyor()
        {
            this.Jobs = new ConcurrentQueue<CRPJob>();
        }

        public CRPJob Retrieve()
        {
            if (this.Jobs.TryDequeue(out CRPJob job))
            {
                return job;
            }

            return null;
        }

        public CRPJob Peek()
        {
            if (this.Jobs.TryPeek(out CRPJob job))
            {
                return job;
            }

            return null;
        }

        public ConcurrentQueue<CRPJob> CopyJobs(CRPConveyor conveyor)
        {
            ConcurrentQueue<CRPJob> jobs = new ConcurrentQueue<CRPJob>();

            foreach (CRPJob job in conveyor.Jobs)
            {
                jobs.Enqueue(job);
            }

            return jobs;
        }

        public void ReplaceJobs(ConcurrentQueue<CRPJob> jobs)
        {
            this.Jobs = jobs;
        }

        public CRPConveyor Clone()
        {
            CRPConveyor clone = (CRPConveyor)this.MemberwiseClone();

            var jobs = this.CopyJobs(clone);
            clone.ReplaceJobs(jobs);

            return clone;
        }

        public override string ToString()
        {
            return string.Format("conv=>{0}, jobcount=>{1}", this.ConveyorNum, this.Jobs.Count);
        }
    }
}
