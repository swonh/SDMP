// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

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
