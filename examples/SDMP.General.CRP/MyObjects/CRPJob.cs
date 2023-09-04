using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDMP.General.CRP.MyObjects
{
    public class CRPJob
    {
        public int Number { get; private set; }

        public CRPColor Color { get; private set; }

        public void SetNumber(int number) 
        {
            this.Number = number;
        }

        public void SetColor(CRPColor color)
        {
            this.Color = color;
        }
    }
}
