using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDMP.General.CRP.MyObjects
{
    public class CRPColor
    {
        public int ColorNumber { get; private set; }

        public void SetColorNumber(int colorNumber) 
        {
            this.ColorNumber = colorNumber;
        }

    }
}
