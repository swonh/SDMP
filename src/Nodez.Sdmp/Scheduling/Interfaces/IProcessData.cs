// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.Interfaces
{
    public interface IProcessData
    {
        string PROCESS_ID { get; }

        int SEQUENCE { get; }

        string EQP_ID { get; }

        double PROC_TIME { get; }
    }
}
