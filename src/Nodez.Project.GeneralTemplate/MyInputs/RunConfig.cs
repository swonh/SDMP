﻿// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data;
using Nodez.Data.Interface;
using Nodez.Sdmp.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nodez.Project.GeneralTemplate.MyInputs
{
    public class RunConfig : IInputRow, IRunConfig
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)

        /// <summary>
        /// Sequence of running
        /// </summary>
        public int RUN_SEQ { get; set; }

        /// <summary>
        /// Solver Name
        /// </summary>
        public string SOLVER_NAME { get; set; }

        /// <summary>
        /// Maximize, Minimize
        /// </summary>
        public string OBJECTIVE_FUNCTION_TYPE { get; set; }

        public string OPT_SOLVER_NAME { get; set; }

        public bool IS_RUN { get; set; }

        public RunConfig()
        {
            // Define keys here (You can search data with the key defined here. Allow multiple keys)
        }
    }
}
