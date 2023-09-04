// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Nodez.Data.Interface;
using Nodez.Sdmp.General.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.GeneralTemplate.Controls
{
    public class UserDataControl : DataControl
    {
        private static readonly Lazy<UserDataControl> lazy = new Lazy<UserDataControl>(() => new UserDataControl());

        public static new UserDataControl Instance { get { return lazy.Value; } }

        public override IData GetData(dynamic[] args)
        {
            return null;
        }
    }
}
