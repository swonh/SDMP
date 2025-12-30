// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDMP.General.CRP.MyObjects
{
    public class CRPState : State
    {
        public Dictionary<int, CRPConveyor> StateInfo { get; set; }

        public CRPConveyor CurrentConveyor { get; set; }

        public CRPJob LastRetrievedJob { get; set; }

        public int JobCount { get { return StateInfo.Values.Sum(x => x.JobCount); } }

        public CRPConveyor GetSameColorConveyor(CRPJob job) 
        {
            if (job == null)
                return null;

            foreach (CRPConveyor conv in this.StateInfo.Values) 
            {
                CRPJob firstJob = conv.Peek();

                if (firstJob == null)
                    continue;

                if (job.Color.ColorNumber == firstJob.Color.ColorNumber)
                    return conv;
            }

            return null;
        }

        public int GetTotalColorCount() 
        {
            int colorCount = 0;

            HashSet<int> hash = new HashSet<int>();
            foreach (KeyValuePair<int, CRPConveyor> info in this.StateInfo)
            {
                foreach (CRPJob job in info.Value.Jobs) 
                {
                    int cnum = job.Color.ColorNumber;
                    if (hash.Contains(cnum))
                        continue;

                    hash.Add(cnum);
                }
            }

            colorCount = hash.Count;

            return colorCount;
        }

        public int GetColorCount(int conveyor)
        {
            int colorCount = 0;

            HashSet<int> hash = new HashSet<int>();

            if (this.StateInfo.TryGetValue(conveyor, out CRPConveyor conv)) 
            {
                foreach (CRPJob job in conv.Jobs)
                {
                    int cnum = job.Color.ColorNumber;
                    if (hash.Contains(cnum))
                        continue;

                    hash.Add(cnum);
                }
            }

            colorCount = hash.Count;

            return colorCount;
        }

        public void SetStateInfo(Dictionary<int, CRPConveyor> stateInfo)
        {
            this.StateInfo = stateInfo.ToDictionary(entry => entry.Key, entry => (CRPConveyor)entry.Value.Clone());
        }

        public CRPJob RetrieveJob()
        {
            foreach (CRPConveyor conv in this.StateInfo.Values)
            {
                CRPJob retrievedJob = conv.Retrieve();

                if (retrievedJob == null)
                    continue;

                CRPJob copiedJob = retrievedJob;

                this.LastRetrievedJob = copiedJob;
                this.CurrentConveyor = conv;

                return retrievedJob;
            }

            return null;
        }

        public CRPJob RetrieveJob(int convNum) 
        {
            if (this.StateInfo.TryGetValue(convNum, out CRPConveyor conv)) 
            {
                CRPJob retrievedJob = conv.Retrieve();
                CRPJob copiedJob = retrievedJob;

                this.LastRetrievedJob = copiedJob;
                this.CurrentConveyor = conv;

                return retrievedJob;
            }

            return null;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            foreach (KeyValuePair<int, CRPConveyor> item in this.StateInfo)
            {
                str.AppendFormat("{0}:({1})/", item.Key, item.Value.JobCount);
            }

            str.AppendFormat("C:{0}", this.CurrentConveyor == null ? "-" : this.CurrentConveyor.ConveyorNum.ToString());
            str.AppendFormat("/J:{0}", this.LastRetrievedJob == null ? "-" : this.LastRetrievedJob.Number.ToString());

            return str.ToString();
        }

        public Dictionary<int, CRPConveyor> CopyStateInfo(CRPState clone) 
        {
            Dictionary<int, CRPConveyor> copied = new Dictionary<int, CRPConveyor>();

            foreach (KeyValuePair<int, CRPConveyor> item in clone.StateInfo)
            {
                copied.Add(item.Key, item.Value.Clone());
            }

            return copied;
        }

        public CRPConveyor CopyCurrentConveyor(CRPState clone)
        {
            if (this.CurrentConveyor == null)
                return null;

            return clone.CurrentConveyor.Clone();
        }

        public CRPJob CopyLastRetrievedJob(CRPState clone)
        {
            if (this.LastRetrievedJob == null)
                return null;

            return clone.LastRetrievedJob;
        }

        public void ReplaceStateInfo(Dictionary<int, CRPConveyor> stateInfo) 
        {
            this.StateInfo = stateInfo;
        }

        public void ReplaceCurrentConveyor(CRPConveyor conveyor) 
        {
            this.CurrentConveyor = conveyor;
        }

        public void ReplaceLastRetrievedJob(CRPJob job)
        {
            this.LastRetrievedJob = job;
        }

        public CRPState Clone()
        {
            CRPState clone = (CRPState)this.MemberwiseClone();

            Dictionary<int, CRPConveyor> stateInfo = this.CopyStateInfo(clone);
            CRPConveyor currConv = this.CopyCurrentConveyor(clone);
            CRPJob lastJob = this.CopyLastRetrievedJob(clone);

            clone.ReplaceStateInfo(stateInfo);
            clone.ReplaceCurrentConveyor(currConv);
            clone.ReplaceLastRetrievedJob(lastJob);

            return clone;
        }
    }
}
