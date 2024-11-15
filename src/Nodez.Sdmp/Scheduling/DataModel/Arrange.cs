// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class Arrange
    {
        public int Index { get; set; }

        public string ProductID { get; set; }

        public string RecipeID { get; set; }

        public string EqpID { get; set; }

        public double ProcTime { get; set; }

        public IArrangeData ArrangeData { get; set; }
    }
}
