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
