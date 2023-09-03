using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.Interfaces
{
    public interface IPlanInfoData
    {
        int PLANNING_HORIZON { get; }

        int PRIORITY_LOOKAHEAD { get; }
    }
}
