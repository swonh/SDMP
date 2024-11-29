// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Constants;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nodez.Sdmp.General.Managers
{
    public class StateManager
    {
        private static Lazy<StateManager> lazy = new Lazy<StateManager>(() => new StateManager());

        public void Reset() { lazy = new Lazy<StateManager>(); }

        public static StateManager Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredManagers.TryGetValue(ManagerType.StateManager.ToString(), out object control))
                {
                    return (StateManager)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public StateManager() 
        {
            this._filteredStateCount = new Dictionary<int, int>();
            this._valueFunctionEstimatedStateCount = new Dictionary<int, int>();
            this._valueFunctionCalculatedStateCount = new Dictionary<int, int>();
        }

        public State InitialState { get; private set; }

        public State FinalState { get; private set; }

        public int ExploredStateCount { get { return this._exploredStateCount; } }

        public int PrunedStateCount { get { return this._prunedStateCount; } }

        public int FilteredStateCount { get { return this._filteredStateCount.Values.Sum(); } }

        public int DualBoundCalculatedStateCount { get { return this._dualBoundCalculatedStateCount; } }

        public int PrimalBoundCalculatedStateCount { get { return this._primalBoundCalculatedStateCount; } }

        public int ValueFunctionEstimatedStateCount { get { return this._valueFunctionEstimatedStateCount.Values.Sum(); } }

        public int ValueFunctionCalculatedStateCount { get { return this._valueFunctionCalculatedStateCount.Values.Sum(); } }

        private int _exploredStateCount { get; set; }

        private int _prunedStateCount { get; set; }

        private int _primalBoundCalculatedStateCount { get; set; }

        private int _dualBoundCalculatedStateCount { get; set; }

        private Dictionary<int, int> _filteredStateCount { get; set; }

        private Dictionary<int, int> _valueFunctionEstimatedStateCount { get; set; }

        private Dictionary<int, int> _valueFunctionCalculatedStateCount { get; set; }

        public void SetInitialState(State state) 
        {
            this.InitialState = state;
        }

        public void SetFinalState(State state)
        {
            this.FinalState = state;
        }

        public void AddState(State state, Stage stage)
        {
            state.Stage = stage;
            this._exploredStateCount++;
        }

        public void PruneState(State state) 
        {
            this._prunedStateCount++;
        }

        public void SetFilteredStateCount(int stageIndex, int filteredCount) 
        {
            if (this._filteredStateCount.ContainsKey(stageIndex) == false)
            {
                this._filteredStateCount[stageIndex] = filteredCount;
            }
        }

        public void AddValueFunctionEstimatedState(State state)
        {
            if (this._valueFunctionEstimatedStateCount.ContainsKey(state.Stage.Index))
            {
                this._valueFunctionEstimatedStateCount[state.Stage.Index]++;
            }
            else
            {
                this._valueFunctionEstimatedStateCount[state.Stage.Index] = 1;
            }
        }

        public void AddValueFunctionCalculatedState(State state)
        {
            if (this._valueFunctionCalculatedStateCount.ContainsKey(state.Stage.Index))
            {
                this._valueFunctionCalculatedStateCount[state.Stage.Index]++;
            }
            else
            {
                this._valueFunctionCalculatedStateCount[state.Stage.Index] = 1;
            }
        }

        public void AddDualBoundCalculatedState(State state)
        {
            this._dualBoundCalculatedStateCount++;
        }

        public void AddPrimalBoundCalculatedState(State state)
        {
            this._primalBoundCalculatedStateCount++;
        }

        public void SetLinks(DataModel.StateTransition transition) 
        {
            State from = transition.FromState;
            State to = transition.ToState;

            to.AddPrevState(from);
        }

    }
}
