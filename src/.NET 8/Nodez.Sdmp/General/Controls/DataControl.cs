// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.Interface;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Managers;
using System;

namespace Nodez.Sdmp.General.Controls
{
    public class DataControl
    {
        private static readonly Lazy<DataControl> lazy = new Lazy<DataControl>(() => new DataControl());

        public static DataControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.DataControl.ToString(), out object control))
                {
                    return (DataControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual IData GetData()
        {
            return null;
        }
    }
}
