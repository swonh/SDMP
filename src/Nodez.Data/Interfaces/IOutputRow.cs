// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nodez.Data.Interface
{
    public abstract class IOutputRow
    {
        OutputTable OutputTable;

        public Dictionary<int, HashSet<string>> KeyMappings;

        public IOutputRow()
        {
            this.KeyMappings = new Dictionary<int, HashSet<string>>();
        }


        public void SetColumnValue(string columnName, string value)
        {
            PropertyInfo info = this.GetType().GetProperty(columnName);

            if (info == null)
                return;

            object changedValue = Convert.ChangeType(value, info.PropertyType);

            info.SetValue(this, changedValue);

        }

        public void SetOutputTable(OutputTable outputTable)
        {
            this.OutputTable = outputTable;
        }

        public dynamic GetValue(string columnName)
        {
            PropertyInfo info = this.GetType().GetProperty(columnName);

            if (info == null)
                return null;

            return info.GetValue(this);
        }

        public string GetKeys()
        {
            if (this.KeyMappings == null)
                return null;

            StringBuilder stringBuilder = new StringBuilder();
            int keyNum = 0;
            foreach (KeyValuePair<int, HashSet<string>> item in this.KeyMappings)
            {
                stringBuilder.Append($"Key:{item.Key}=>");

                int valCount = 0;
                foreach (string columnName in item.Value)
                {
                    PropertyInfo info = this.GetType().GetProperty(columnName);

                    if (info == null)
                        continue;

                    string val = info.GetValue(this).ToString();

                    if (valCount == item.Value.Count - 1)
                        stringBuilder.Append(val);
                    else
                        stringBuilder.Append(val).Append("@");

                    valCount++;
                }

                if (keyNum != this.KeyMappings.Count - 1)
                    stringBuilder.Append(",");

                keyNum++;
            }

            return stringBuilder.ToString();

        }

        public string GetKey(int keyNumber)
        {
            if (this.KeyMappings == null)
                return null;

            HashSet<string> keyValues;
            if (this.KeyMappings.TryGetValue(keyNumber, out keyValues) == false)
                return null;

            List<string> values = new List<string>();
            foreach (string columnName in keyValues)
            {
                PropertyInfo info = this.GetType().GetProperty(columnName);

                if (info == null)
                    continue;

                string val = info.GetValue(this).ToString();

                values.Add(val);
            }

            StringBuilder stringBuilder = new StringBuilder();
            int count = 0;
            foreach (string val in values)
            {
                if (count == keyValues.Count - 1)
                    stringBuilder.Append(val);
                else
                    stringBuilder.Append(val).Append("@");

                count++;
            }

            return stringBuilder.ToString();

        }

    }

}
