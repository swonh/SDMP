using Nodez.Data.Interface;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.Routing.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.RoutingTemplate.Controls
{
    public class UserDataControl : DataControl
    {
        private static readonly Lazy<UserDataControl> lazy = new Lazy<UserDataControl>(() => new UserDataControl());

        public static new UserDataControl Instance { get { return lazy.Value; } }

        public override IData GetData(dynamic[] args)
        {
            // Default logic
            RoutingDataManager dataManager = RoutingDataManager.Instance;

            dataManager.InitializeRoutingData();
            dataManager.InitializeRoutingProblem();

            return dataManager.RoutingData;
        }
    }
}
