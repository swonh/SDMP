// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.Managers;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.GeneralTemplate.Controls
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
