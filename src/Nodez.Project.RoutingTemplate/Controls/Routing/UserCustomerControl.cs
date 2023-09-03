using Nodez.Sdmp.Routing.Controls;
using Nodez.Sdmp.Routing.DataModel;
using Nodez.Sdmp.Routing.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.RoutingTemplate.Controls
{
    public class UserCustomerControl : CustomerControl
    {
        private static readonly Lazy<UserCustomerControl> lazy = new Lazy<UserCustomerControl>(() => new UserCustomerControl());

        public static new UserCustomerControl Instance { get { return lazy.Value; } }

        public override Dictionary<int, VehicleStateInfo> GetVisitableCustomers(Dictionary<int, VehicleStateInfo> vehicleInfos)
        {
            return base.GetVisitableCustomers(vehicleInfos);         
        }
    }
}
