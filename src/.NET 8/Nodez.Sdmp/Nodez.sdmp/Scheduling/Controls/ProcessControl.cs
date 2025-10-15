// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Scheduling.DataModel;
using System;

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
