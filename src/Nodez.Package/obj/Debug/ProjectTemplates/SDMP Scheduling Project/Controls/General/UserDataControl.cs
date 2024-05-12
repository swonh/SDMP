using Nodez.Data.Interface;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.Scheduling.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Controls
{
    public class UserDataControl : DataControl
    {
        private static readonly Lazy<UserDataControl> lazy = new Lazy<UserDataControl>(() => new UserDataControl());

        public static new UserDataControl Instance { get { return lazy.Value; } }

        public override IData GetData(dynamic[] args)
        {
            // Default logic
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            dataManager.InitializeSchedulingData();
            dataManager.InitializeSchedulingProblem();

            return dataManager.SchedulingData;
        }
    }
}
