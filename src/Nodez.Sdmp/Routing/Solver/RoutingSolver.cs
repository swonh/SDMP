using Priority_Queue;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Interfaces;
using Nodez.Sdmp.Routing.Controls;
using Nodez.Sdmp.Routing.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.Solver
{
    public class RoutingSolver : ISolver
    {
        public RoutingSolver(IRunConfig runConfig) 
        {
            this.RunConfig = runConfig;
            this.StopWatch = new Stopwatch();
            this.VisitedStates = new Dictionary<string, State>();
            this.TransitionQueue = new FastPriorityQueue<State>(Convert.ToInt32(9 * Math.Pow(10, 7)));
            //FastPriorityQueue - Convert.ToInt32(9 * Math.Pow(10, 7)
        }

        protected override void DoInitialStateTransitions(State initialState)
        {
            RoutingState initState = initialState as RoutingState;

            ActionControl transitionControl = ActionControl.Instance;
            StateManager stateManager = StateManager.Instance;
            StateControl stateControl = StateControl.Instance;
            CustomerControl customerControl = CustomerControl.Instance;
            ApproximationControl approxControl = ApproximationControl.Instance;

            List<State> newStates = new List<State>();

            List<General.DataModel.StateActionMap> initStateActionMaps = this.GetStateActionMaps(initialState);
            foreach (StateActionMap map in initStateActionMaps)
            {
                map.PostActionState.SetKey(stateControl.GetKey(map.PostActionState));
                map.PostActionState.IsPostActionState = true;
                map.PostActionState.PreActionState = map.PostActionState;
            }

            List<General.DataModel.StateTransition> initStateTransitions = this.GetStateTransitions(initStateActionMaps);

            Stage nextStage = new Stage(initialState.Stage.Index + 1);
            foreach (General.DataModel.StateTransition tran in initStateTransitions)
            {
                State toState = tran.ToState;

                tran.ToState.Stage = nextStage;

                toState.Index = ++this.CurrentStateIndex;
                toState.SetKey(stateControl.GetKey(toState));
                tran.SetIndex(++this.TransitionIndex);

                double cost = tran.Cost;
                double nextValue = initialState.BestValue + cost;

                toState.BestValue = nextValue;

                toState.SetPrevBestState(initialState);

                stateManager.SetLinks(tran);
                stateManager.AddState(toState, nextStage);

                if (this.VisitedStates.ContainsKey(toState.Key) == false)
                    this.VisitedStates.Add(toState.Key, toState);

                newStates.Add(toState);
            }

            if (this.CheckLocalFilteringCondition(initialState.Stage.Index))
            {
                int maximumTransitionCount = approxControl.GetLocalTransitionCount();
                int approximationStartStageIndex = approxControl.GetApproximationStartStageIndex();

                newStates = approxControl.FilterLocalStates(newStates, maximumTransitionCount);
            }

            this.AddNextStates(newStates);
        }

        protected override List<General.DataModel.StateActionMap> GetStateActionMaps(State state)
        {
            RoutingState routingState = state as RoutingState;
            ActionControl actionControl = ActionControl.Instance;
            CustomerControl customerControl = CustomerControl.Instance;

            routingState.VehicleStateInfos = customerControl.GetVisitableCustomers(routingState.VehicleStateInfos);
            List<General.DataModel.StateActionMap> maps = actionControl.GetStateActionMaps(state);

            return maps;
        }
    }
}
