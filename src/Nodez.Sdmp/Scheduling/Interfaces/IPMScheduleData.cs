// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Nodez.Sdmp.Scheduling.Interfaces
{
    public interface IPMScheduleData
    {
        string PM_ID { get; }

        string EQP_ID { get; }

        string MODULE_ID { get; }

        double PM_START_TIME { get; }

        double PM_END_TIME { get; }
    }
}
