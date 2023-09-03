using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.Interfaces
{
    public interface IArrangeData
    {
        string PRODUCT_ID { get; }

        string PROPERTY_ID { get; }

        string EQP_ID { get; }

        double PROC_TIME { get; }
    }
}
