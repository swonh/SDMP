using Nodez.Data;
using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDMP.General.CRP.MyObjects
{
    public class CRPParameter : ISets
    {
        public static int JOBS_NUM = 24; 

        public static int CONV_NUM = 8;

        public static int JOBS_PER_CONV = JOBS_NUM / CONV_NUM;

        public static int COLOR_NUM = 20;
    }
}
