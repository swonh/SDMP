// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
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
        public int CurrentCustomerIndex { get; set; }

        public int[] VisitedCustomerFlag { get; set; }

        public int[] NextVistableCustomerFlag { get; set; }

        public int VisitedCustomerCount { get; set; }

        public int VistableCustomerCount { get; set; }

        public double[] RemainCapacity { get; set; }

        public bool IsActive { get; set; }

        public double AvailableTime { get; set; }

        public bool IsDoneVisitCustomers() 
        {
            bool isDoneVisit = false;

            //if (this.VisitedCustomerCount == VisitedCustomerFlag.Length - 1 || this.VistableCustomerCount == 0)
            //    isDoneVisit = true;

            if (this.VistableCustomerCount == 0)
                isDoneVisit = true;

            return isDoneVisit;
        }

        public VehicleStateInfo Clone() 
        {
            VehicleStateInfo clone = new VehicleStateInfo();

            clone.CurrentCustomerIndex = this.CurrentCustomerIndex;
            clone.VisitedCustomerCount = this.VisitedCustomerCount;
            clone.VistableCustomerCount = this.VistableCustomerCount;
            clone.IsActive = this.IsActive;
            clone.AvailableTime = this.AvailableTime;

            clone.VisitedCustomerFlag = new int[this.VisitedCustomerFlag.Length];
            clone.NextVistableCustomerFlag = new int[this.NextVistableCustomerFlag.Length];
            clone.RemainCapacity = new double[this.RemainCapacity.Length];

            Buffer.BlockCopy(this.VisitedCustomerFlag, 0, clone.VisitedCustomerFlag, 0, this.VisitedCustomerFlag.Length * sizeof(int));
            Buffer.BlockCopy(this.NextVistableCustomerFlag, 0, clone.NextVistableCustomerFlag, 0, this.NextVistableCustomerFlag.Length * sizeof(int));
            Buffer.BlockCopy(this.RemainCapacity, 0, clone.RemainCapacity, 0, this.RemainCapacity.Length * sizeof(double));

            return clone;
        }

        public override string ToString() 
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("C:{0}", this.CurrentCustomerIndex);

            str.Append("@V:");
            foreach (int flag in this.VisitedCustomerFlag) 
            {
                str.Append(flag);
            }

            str.AppendFormat("@AvailPeriod:{0}", this.AvailableTime);

            return str.ToString();
        }
    }
}
