// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.Interfaces
{
    public interface IJobData
    {
        string JOB_ID { get; }

        string JOB_NAME { get; }

        string PRODUCT_ID { get; }

        string PROCESS_ID { get; }

        string STEP_SEQ { get; }

        double RELEASE_TIME { get; }

        int SPLIT_COUNT { get; }

        int QTY { get; }

        string PARENT_JOB_ID { get; }

        int PRIORITY { get; }

        string RECIPE_ID { get; }

        bool IS_ACT { get; }

    }
}
