using Nodez.Sdmp.Constants;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
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

        public State InitialState { get; private set; }

        public State FinalState { get; private set; }

        public int StateCount { get; private set; }

        private Dictionary<int, List<State>> _stageStateMappings { get; set; }

        public List<State> GetStates(int stageNum) 
        {
            List<State> states = new List<State>();

            if (this._stageStateMappings.TryGetValue(stageNum, out List<State> val)) 
            {
                states = val;
            }

            return states;
        }

        public void SetInitialState(State state) 
        {
            this.InitialState = state;
        }

        public void SetFinalState(State state)
        {
            SolutionManager solutionManager = SolutionManager.Instance;
            Solution sol = solutionManager.GetOptimalSolution(state);
            solutionManager.AddSolution(sol);

            if (solutionManager.BestSolution == sol)
                this.FinalState = state;
        }

        public void ClearStageStateMappings() 
        {
            this._stageStateMappings.Clear();
        }

        public int GetStateCount() 
        {
            int count = 0;
            //foreach (List<State> item in this._stageStateMappings.Values)
            //{
            //    count += item.Count;
            //}

            return count;
        }

        public void AddState(State state, Stage stage)
        {
            state.Stage = stage;

            //stage.AddState(state);

            //if (this._stageStateMappings == null)
            //    this._stageStateMappings = new Dictionary<int, List<State>>();

            //if (this._stageStateMappings.TryGetValue(stage.Index, out List<State> states))
            //{
            //    states.Add(state);
            //    this.StateCount++;
            //}
            //else
            //{
            //    this._stageStateMappings.Add(stage.Index, new List<State>() { state });
            //    this.StateCount++;
            //}
        }

        public void SetLinks(DataModel.StateTransition transition) 
        {
            State from = transition.FromState;
            State to = transition.ToState;

            to.AddPrevState(from);
        }

    }
}
