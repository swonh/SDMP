// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nodez.Sdmp.General.Managers
{
    public class StateTransitionManager
    {
        private static Lazy<StateTransitionManager> lazy = new Lazy<StateTransitionManager>(() => new StateTransitionManager());

        public static StateTransitionManager Instance { get { return lazy.Value; } }

        public void Reset() { lazy = new Lazy<StateTransitionManager>(); }

        public List<StateTransition> GetStateTransitions(StateActionMap stateActionMap) 
        {
            List<StateTransition> trans = new List<StateTransition>();

            StateTransition tran = new StateTransition();

            tran.FromState = stateActionMap.PostActionState;
            tran.ToState = stateActionMap.PostActionState.Clone();
            tran.Cost = stateActionMap.Cost;

            trans.Add(tran);

            return trans;
        }
    }
}
