// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Nodez.Sdmp.Constants;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using System;
using System.Collections.Generic;
using System.Text;

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
