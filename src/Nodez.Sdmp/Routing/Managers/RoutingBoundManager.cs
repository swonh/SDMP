using Nodez.Sdmp.Routing.DataModel;
using Nodez.Sdmp.Routing.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.Managers
{
    public class RoutingBoundManager
    {
        private static readonly Lazy<RoutingBoundManager> lazy = new Lazy<RoutingBoundManager>(() => new RoutingBoundManager());

        public static RoutingBoundManager Instance { get { return lazy.Value; } }

        public double GetDualBound(RoutingState state) 
        {
            PrimAlgorithm prim = new PrimAlgorithm(state);
            prim.Run();

            double mstValue = prim.GetMSTValue();

            double bound = mstValue - state.BestValue;

            return bound;
        }
    }
}
