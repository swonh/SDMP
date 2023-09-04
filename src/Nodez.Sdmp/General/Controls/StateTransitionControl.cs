// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

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
    public class StateTransitionControl
    {
        private static readonly Lazy<StateTransitionControl> lazy = new Lazy<StateTransitionControl>(() => new StateTransitionControl());

        public static StateTransitionControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.StateTransitionControl.ToString(), out object control))
                {
                    return (StateTransitionControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual List<DataModel.StateTransition> GetStateTransitions(StateActionMap stateActionMap)
        {
            StateTransitionManager transitionManager = StateTransitionManager.Instance;

            return transitionManager.GetStateTransitions(stateActionMap);
        }

        public virtual DataModel.StateTransition GetFinalStateTransition(State state)
        {
            DataModel.StateTransition tran = new DataModel.StateTransition();

            State finalState = new State();
            finalState.IsFinal = true;

            tran.FromState = state;
            tran.ToState = finalState;

            return tran;
        }
    }
}
