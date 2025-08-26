// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.Interface;
using System;

namespace Nodez.Data.Managers
{
    public class DataManager
    {
        private static readonly Lazy<DataManager> lazy = new Lazy<DataManager>(() => new DataManager());

        public static DataManager Instance { get { return lazy.Value; } }

        public IData Data { get; private set; }

        public void SetData(IData data)
        {
            this.Data = data;
        }

        public IData GetData()
        {
            return this.Data;
        }
    }
}
