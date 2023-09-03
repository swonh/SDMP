using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Interfaces
{
    public interface IRunConfig
    {
        int RUN_SEQ { get; }

        string SOLVER_NAME { get; }

        string OBJECTIVE_FUNCTION_TYPE { get; }

        string OPT_SOLVER_NAME { get; }

        bool IS_RUN { get; }
    }
}
