using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.Interfaces
{
    public interface IResourceData
    {
        string ID { get; }

        string NAME { get; }

        string PRODUCT_ID { get; }

        double CAPACITY { get; }
    }
}
