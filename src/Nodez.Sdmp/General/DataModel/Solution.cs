using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nodez.Sdmp.General.DataModel
{
    public class Solution
    {
        public Dictionary<int, State> States { get; private set; }

        public double Value { get; private set; }

        public bool IsOptimal { get; private set; }

        public double DualityGap { get; private set; }

        public bool IsHeuristic { get; private set; }

        public Solution(IEnumerable<State> states, bool isHeuristic = false) 
        {
            this.States = new Dictionary<int, State>();

            foreach (State state in states) 
            {
                this.States.Add(state.Stage.Index, state);
            }

            this.SetValue();
            this.IsHeuristic = isHeuristic;
        }

        public void SetValue() 
        {
            IOrderedEnumerable<KeyValuePair<int, State>> ordered = this.States.OrderBy(x => x.Key);
            this.Value = ordered.Last().Value.BestValue;
        }

        public void SetIsOptimal(bool isOptimal) 
        {
            this.IsOptimal = isOptimal;
        }

    }
}
