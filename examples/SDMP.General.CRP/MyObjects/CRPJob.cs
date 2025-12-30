// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

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
