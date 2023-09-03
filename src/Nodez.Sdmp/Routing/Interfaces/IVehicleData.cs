using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.Interfaces
{
    public interface IVehicleData
    {
        string ID { get; }

        string NAME { get; }

        double SPEED { get; }
    }
}
