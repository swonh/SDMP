using Nodez.Data;
using Nodez.Data.Interface;
using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nodez.Project.SchedulingTemplate.MyInputs
{
    public class PlanInfo : IInputRow, IPlanInfoData
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)

        public int PLANNING_HORIZON { get; private set; }

        public int PRIORITY_LOOKAHEAD { get; private set; }

        public PlanInfo()
        {

        }
    }
}
