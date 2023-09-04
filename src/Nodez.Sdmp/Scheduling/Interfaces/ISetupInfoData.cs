// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

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
