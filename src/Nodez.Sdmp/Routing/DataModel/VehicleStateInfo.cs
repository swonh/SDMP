// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.DataModel
{
    public class VehicleStateInfo
    {
        public int CurrentNodeIndex { get; set; }

        public int[] VisitedNodeFlag { get; set; }

        public int[] NextVistableNodeFlag { get; set; }

        public int VisitedNodeCount { get; set; }

        public int VistableNodeCount { get; set; }

        public double[] RemainCapacity { get; set; }

        public bool IsActive { get; set; }

        public double AvailableTime { get; set; }

        public int PickupCount { get; set; }

        public int DeliveryCount { get; set; }


        public bool IsFinished { get { return VisitedNodeCount > 0 && PickupCount == DeliveryCount; } }

        public bool IsDoneVisitNodes() 
        {
            bool isDoneVisit = false;

            //if (this.VisitedNodeCount == VisitedNodeFlag.Length - 1 || this.VistableNodeCount == 0)
            //    isDoneVisit = true;

            if (this.VistableNodeCount == 0)
                isDoneVisit = true;

            return isDoneVisit;
        }

        public VehicleStateInfo Clone() 
        {
            VehicleStateInfo clone = new VehicleStateInfo();

            clone.CurrentNodeIndex = this.CurrentNodeIndex;
            clone.VisitedNodeCount = this.VisitedNodeCount;
            clone.VistableNodeCount = this.VistableNodeCount;
            clone.IsActive = this.IsActive;
            clone.AvailableTime = this.AvailableTime;

            clone.VisitedNodeFlag = new int[this.VisitedNodeFlag.Length];
            clone.NextVistableNodeFlag = new int[this.NextVistableNodeFlag.Length];
            clone.RemainCapacity = new double[this.RemainCapacity.Length];

            Buffer.BlockCopy(this.VisitedNodeFlag, 0, clone.VisitedNodeFlag, 0, this.VisitedNodeFlag.Length * sizeof(int));
            Buffer.BlockCopy(this.NextVistableNodeFlag, 0, clone.NextVistableNodeFlag, 0, this.NextVistableNodeFlag.Length * sizeof(int));
            Buffer.BlockCopy(this.RemainCapacity, 0, clone.RemainCapacity, 0, this.RemainCapacity.Length * sizeof(double));

            return clone;
        }

        public override string ToString() 
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("C:{0}", this.CurrentNodeIndex);

            str.Append("@V:");
            foreach (int flag in this.VisitedNodeFlag) 
            {
                str.Append(flag);
            }

            str.AppendFormat("@AvailPeriod:{0}", this.AvailableTime);

            return str.ToString();
        }
    }
}
