using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.Interfaces
{
    public interface IJobData
    {
        string JOB_ID { get; }

        string NAME { get; }

        string PRODUCT_ID { get; }

        string PROCESS_ID { get; }

        string STEP_SEQ { get; }

        double RELEASE_TIME { get; }

        int SPLIT_COUNT { get; }

        int QTY { get; }

        string PARENT_JOB_ID { get; }

        int PRIORITY { get; }

        string PROPERTY_ID { get; }

        bool IS_ACT { get; }

    }
}
