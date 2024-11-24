// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.General.DataModel
{
    public class StatusLog : IOutputRow
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)

        public string STATE_INDEX { get; set; }

        public string STAGE_INDEX { get; set; }

        public string BEST_SOLUTION { get; set; }

        public string BEST_DUAL_BOUND { get; set; }

        public string SOLUTION_COUNT { get; set; }

        public string RELATIVE_DUALITY_GAP { get; set; }

        public string TIME { get; set; }

    }
}
