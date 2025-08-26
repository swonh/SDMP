// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Nodez.Sdmp.Scheduling.Interfaces
{
    public interface ITargetInfoData
    {
        string TARGET_ID { get; }

        string PROCESS_ID { get; }

        string STEP_SEQ { get; }

        int TARGET_QTY { get; }
    }
}
