// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
