using Nodez.Data.Managers;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.RoutingTemplate.Controls
{
    public class UserEventControl : EventControl
    {
        private static readonly Lazy<UserEventControl> lazy = new Lazy<UserEventControl>(() => new UserEventControl());

        public static new UserEventControl Instance { get { return lazy.Value; } }


        public override void OnBeginSolve()
        {

        }

        public override void OnDataLoad()
        {

        }

        public override void OnVisitState(State state)
        {

        }

        public override void OnVisitToState(State fromState, State toState)
        {

        }

        public override void OnStageChanged(Stage stage)
        {

        }

        public override void OnDoneSolve()
        {

        }
    }
}
