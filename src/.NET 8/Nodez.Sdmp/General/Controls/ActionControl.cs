// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using System;
using System.Collections.Generic;

namespace Nodez.Sdmp.General.Controls
{
    public class ActionControl
    {
        private static readonly Lazy<ActionControl> lazy = new Lazy<ActionControl>(() => new ActionControl());

        public static ActionControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.ActionControl.ToString(), out object control))
                {
                    return (ActionControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual List<DataModel.StateActionMap> GetStateActionMaps(State state)
        {
            return new List<DataModel.StateActionMap>();
        }
    }
}
