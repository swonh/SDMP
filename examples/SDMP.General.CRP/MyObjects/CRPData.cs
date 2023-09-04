using Nodez.Data;
using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDMP.General.CRP.MyObjects
{
    public class CRPData : IData
    {
        public CRPFactory CRPFactory { get; set; }

        public double[,] SetupCostMatrix { get; set; }

    }
}
