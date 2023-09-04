using Nodez.Sdmp;
using Nodez.Sdmp.Constants;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SDMP.General.CRP.Controls
{
    public class UserSolverControl : SolverControl
    {
        private static readonly Lazy<UserSolverControl> lazy = new Lazy<UserSolverControl>(() => new UserSolverControl());

        public static new UserSolverControl Instance { get { return lazy.Value; } }

        public override int GetRunMaxTime()
        {
            return Int32.MaxValue;
        }

        public override ObjectiveFunctionType GetObjectiveFuntionType(IRunConfig runConfig)
        {
            ObjectiveFunctionType objectiveFunctionType = UtilityHelper.StringToEnum(runConfig.OBJECTIVE_FUNCTION_TYPE, ObjectiveFunctionType.Minimize);

            return objectiveFunctionType;
        }

        public override string GetProjectName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;
        }

        public override string GetOutputDirectoryPath(string solverName)
        {
            return null;
        }
    }
}
