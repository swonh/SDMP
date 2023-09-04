// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
