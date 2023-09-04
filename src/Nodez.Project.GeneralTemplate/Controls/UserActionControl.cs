// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Nodez.Data.Managers;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nodez.Project.GeneralTemplate.Controls
{
    public class UserActionControl : ActionControl
    {
        private static readonly Lazy<UserActionControl> lazy = new Lazy<UserActionControl>(() => new UserActionControl());

        public static new UserActionControl Instance { get { return lazy.Value; } }

        public override List<StateActionMap> GetStateActionMaps(State state)
        {
            List<StateActionMap> maps = new List<StateActionMap>();

            return maps;
        }
    }
}
