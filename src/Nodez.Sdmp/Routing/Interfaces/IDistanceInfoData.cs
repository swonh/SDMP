using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.Interfaces
{
    public interface IDistanceInfoData
    {
        string FROM_CUSTOMER_ID { get; }

        string TO_CUSTOMER_ID { get; }

        double DISTANCE { get; }

        double TIME { get; }
    }
}
