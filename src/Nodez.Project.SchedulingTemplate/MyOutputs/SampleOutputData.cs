// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.SchedulingTemplate.MyOutputs
{
    public class SampleOutputData : IOutputRow
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)

        public string COL_1 { get; set; }

        public string COL_2 { get; set; }

    }
}
