// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.General.DataModel
{
    public class Stage
    {
        public int Index { get; private set; }

        public List<State> States { get; private set; }

        public bool IsLastStage { get; private set; }

        public bool IsFinalStage { get; private set; }

        public Stage(int index, State state) 
        {
            this.Index = index;
            this.States = new List<State>() { state };
        }

        public Stage(int index)
        {
            this.Index = index;
            this.States = new List<State>();
        }

        public void AddState(State state) 
        {
            this.States.Add(state);
        }

        internal void SetIsLastStage(bool isLastStage) 
        {
            this.IsLastStage = isLastStage;
        }

        internal void SetIsFinalStage(bool isFinalStage)
        {
            this.IsFinalStage = isFinalStage;
        }
    }
}
