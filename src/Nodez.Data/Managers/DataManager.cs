// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Nodez.Data;
using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.Text;

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
