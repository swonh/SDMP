// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace Nodez.Sdmp.General.Managers
{
    public class ControlManager
    {
        private static Lazy<ControlManager> lazy = new Lazy<ControlManager>(() => new ControlManager());

        public static ControlManager Instance { get { return lazy.Value; } }

        public Dictionary<string, object> RegisteredControls = new Dictionary<string, object>();

        public Dictionary<string, object> RegisteredManagers = new Dictionary<string, object>();

        public void Reset() { lazy = new Lazy<ControlManager>(); }

        public void RegisterControl(string name, object control)
        {
            if (this.RegisteredControls.ContainsKey(name) == false)
                this.RegisteredControls.Add(name, control);
        }

        public void RegisterManager(string name, object manager)
        {
            if (this.RegisteredManagers.ContainsKey(name) == false)
                this.RegisteredManagers.Add(name, manager);
        }

    }
}
