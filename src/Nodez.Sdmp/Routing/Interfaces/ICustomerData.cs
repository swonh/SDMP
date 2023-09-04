// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.Interfaces
{
    public interface ICustomerData
    {
        string ID { get; }

        string NAME { get; }

        string DEMAND_ID { get; }

        double START_TIME_WINDOW { get; }

        double END_TIME_WINDOW { get; }

        double X_COORDINATE { get; }

        double Y_COORDINATE { get; }

        string IS_DELIVERY { get; }

        string IS_DEPOT { get; }
    }
}
