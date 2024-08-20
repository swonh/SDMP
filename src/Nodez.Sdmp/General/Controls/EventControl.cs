// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Constants;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.General.Controls
{
    public class EventControl
    {
        private static readonly Lazy<EventControl> lazy = new Lazy<EventControl>(() => new EventControl());

        public static EventControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.EventControl.ToString(), out object control))
                {
                    return (EventControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual void OnBeginSolve() 
        {
        
        }

        public virtual void OnDataLoad() 
        {
        
        }

        public virtual void OnVisitState(State state) 
        {
        
        }

        public virtual void OnVisitToState(State fromState, State toState)
        {

        }

        public virtual void OnStageChanged(Stage stage) 
        {
            
        }

        public virtual void OnDoneSolve()
        {

        }
    }
}
