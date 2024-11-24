// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.General.Managers;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nodez.Sdmp.General.DataModel
{
    public class State : FastPriorityQueueNode
    {
        public string Key { get; set; }

        public object Data { get; set; }

        public int Index { get; set; }

        public double CurrentBestValue { get; set; }

        public double DualBound { get; set; }

        public double ValueFunctionEstimate { get; set; }

        public double PrimalBound { get; set; }

        public bool IsInitial { get; set; }

        public bool IsLeaf { get; set; }

        public bool IsFinal { get; set; }

        public bool IsLastStage { get; set; }

        public bool IsVisited { get; set; } 

        public bool IsFixed { get; set; }

        public bool IsPostActionState { get; set; }

        public bool IsSetDualBound { get; set; }

        public bool IsSetPrimalBound { get; set; }

        public bool IsSetValueFunctionEstimate { get; set; }

        public State PreActionState { get; set; }

        public Dictionary<string, State> PrevBestStates { get; set; }

        public State PrevBestState { get; set; }

        public Dictionary<string, State> PrevStates { get; set; }

        public Stage Stage { get; set; }

        public int ClusterID { get; set; }

        public double ClusterDistance { get; set; }


        public State(string key)
        {
            this.Key = key;
            this.DualBound = BoundManager.Instance.RootDualBound;
        }

        public State()
        {
            this.PrevStates = new Dictionary<string, State>();
            this.PrevBestStates = new Dictionary<string, State>();
            this.DualBound = BoundManager.Instance.RootDualBound;
        }

        public virtual void SetDualBound(double value)
        {
            this.DualBound = value;
            this.IsSetDualBound = true;
        }

        public virtual void SetPrimalBound(double value)
        {
            this.PrimalBound = value;
            this.IsSetPrimalBound = true;
        }

        public virtual void SetValueFunctionEstimate(double value) 
        {
            this.ValueFunctionEstimate = value;
            this.IsSetValueFunctionEstimate = true;
        }

        public virtual void SetKey(string key)
        {
            this.Key = key;
        }

        public virtual void AddPrevState(State state)
        {
            if (this.PrevStates.ContainsKey(state.Key) == false)
            {
                this.PrevStates.Add(state.Key, state);
            }
        }

        public virtual void SetPrevBestState(State state)
        {
            this.PrevBestState = state;

            if (this.PrevBestStates.ContainsKey(state.Key) == false)
            {
                this.PrevBestStates.Add(state.Key, state);
            }
        }

        public virtual List<State> GetBestStatesBackward() 
        {
            List<State> states = new List<State>();

            if (this.PrevBestState == null)
                return states;

            State currState = this.PrevBestState;

            states.Add(currState);

            while (currState.IsInitial == false) 
            {
                currState = currState.PrevBestState;
                states.Add(currState);
            }

            return states;
        }

        public State Clone()
        {
            State clone = (State)this.MemberwiseClone();

            return clone;
        }

        public override string ToString()
        {
            return this.Key;
        }

    }
}
