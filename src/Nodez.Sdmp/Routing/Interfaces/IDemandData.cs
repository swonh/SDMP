// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.Interfaces
{
    public interface IDemandData
    {
        string ID { get; }

        string NAME { get; }

        string PRODUCT_ID { get; }

        double QUANTITY { get; }
    }
}
