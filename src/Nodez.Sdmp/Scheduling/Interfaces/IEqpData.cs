using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.Interfaces
{
    public interface IEqpData
    {
        string EQP_ID { get; }

        string EQP_NAME { get; }

        string GROUP_ID { get; }
    }
}
