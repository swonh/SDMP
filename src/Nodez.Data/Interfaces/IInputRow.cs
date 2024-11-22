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
    public abstract class IInputRow
    {
        InputTable InputTable;

        public Dictionary<int, HashSet<string>> KeyMappings;

        public IInputRow()
        {
            this.KeyMappings = new Dictionary<int, HashSet<string>>();
        }


        public void SetColumnValue(string tableName, string columnName, string value, Dictionary<ValueTuple<string, string>, string> columnNameMappings)
        {
            string propertyName;
            if (columnNameMappings.TryGetValue((tableName, columnName), out propertyName) == false)
                propertyName = columnName;

            PropertyInfo info = this.GetType().GetProperty(propertyName);

            if (info == null)
            {
                throw new Exception($"Columnname does not match: Table:{this.InputTable.Name}, Column:{columnName}");
            }

            object changedValue;
            if (value == string.Empty)
                return;
            else
                changedValue = Convert.ChangeType(value, info.PropertyType);

            info.SetValue(this, changedValue);
        }

        public void SetInputTable(InputTable inputTable)
        {
            this.InputTable = inputTable;
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

        public dynamic GetValue(string columnName) 
        {
            PropertyInfo info = this.GetType().GetProperty(columnName);

            if (info == null)
                return null;

            return info.GetValue(this);
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

                if (info.GetValue(this) == null)
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
