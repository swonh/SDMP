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
    public interface ISetupInfoData
    {
        string EQP_ID { get; }

        string FROM_PROPERTY_ID { get; }

        string TO_PROPERTY_ID { get; }

        double SETUP_TIME { get; }
    }
}
