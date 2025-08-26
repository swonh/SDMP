// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Nodez.Sdmp.Scheduling.Interfaces
{
    public interface IArrangeData
    {
        string PRODUCT_ID { get; }

        string RECIPE_ID { get; }

        string EQP_ID { get; }

        double PROC_TIME { get; }
    }
}
