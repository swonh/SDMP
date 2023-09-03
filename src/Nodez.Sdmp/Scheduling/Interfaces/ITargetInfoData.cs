using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
