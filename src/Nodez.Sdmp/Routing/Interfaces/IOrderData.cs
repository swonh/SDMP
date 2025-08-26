// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Nodez.Sdmp.Routing.Interfaces
{
    public interface IOrderData
    {
        string ID { get; }

        string NAME { get; }

        double ORDER_TIME { get; }

        string PRODUCT_ID { get; }

        string PICKUP_NODE_ID { get; }

        string DELIVERY_NODE_ID { get; }

        double PROCESS_TIME { get; }

        int ORDER_QTY { get; }

        double DEADLINE { get; }
    }
}
