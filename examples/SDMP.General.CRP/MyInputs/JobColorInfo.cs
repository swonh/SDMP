// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data;
using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SDMP.General.CRP.MyInputs
{
    public class JobColorInfo : IInputRow
    {
        public int JOB_NUM { get; private set; }

        public int COLOR { get; private set; }


        public JobColorInfo()
        {
            HashSet<string> key = new HashSet<string>();
            key.Add("JOB_NUM");

            this.KeyMappings.Add(1, key);
        }
    }
}
