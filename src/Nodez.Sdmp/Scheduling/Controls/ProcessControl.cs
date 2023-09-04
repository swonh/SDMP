// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Scheduling.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.Controls
{
    public class ProcessControl
    {
        private static readonly Lazy<ProcessControl> lazy = new Lazy<ProcessControl>(() => new ProcessControl());

        public static ProcessControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.ProcessControl.ToString(), out object control))
                {
                    return (ProcessControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual bool IsLoadable(Job job, Equipment eqp) 
        {
            return true;
        }
    }
}
