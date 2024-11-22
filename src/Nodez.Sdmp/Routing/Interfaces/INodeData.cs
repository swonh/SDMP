// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.Interfaces
{
    public interface INodeData
    {
        string ID { get; }

        string NAME { get; }

        string ORDER_ID { get; }

        double START_TIME_WINDOW { get; }

        double END_TIME_WINDOW { get; }

        double X_COORDINATE { get; }

        double Y_COORDINATE { get; }

        string IS_DELIVERY { get; }

        string IS_DEPOT { get; }
    }
}
