﻿// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.Interface;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.Scheduling.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.SchedulingTemplate.Controls
{
    public class UserDataControl : DataControl
    {
        private static readonly Lazy<UserDataControl> lazy = new Lazy<UserDataControl>(() => new UserDataControl());

        public static new UserDataControl Instance { get { return lazy.Value; } }

        public override IData GetData()
        {
            // Default logic
            SchedulingDataManager dataManager = SchedulingDataManager.Instance;

            dataManager.InitializeSchedulingData();
            dataManager.InitializeSchedulingProblem();

            return dataManager.SchedulingData;
        }
    }
}
