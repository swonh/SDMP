// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nodez.Data.DataModel
{
    public class OutputTable
    {
        public Dictionary<int, Dictionary<IComparable, List<IOutputRow>>> Views;

        public List<string> ColumnNames;

        private List<IOutputRow> _rows;

        public string Name;

        public List<IOutputRow> Rows()
        {
            return this._rows;
        }

        private void Init(IOutputRow row)
        {
            Type type = row.GetType();
            IEnumerable<FieldInfo> fieldInfos = row.GetType().GetRuntimeFields();

            List<string> colNames = new List<string>();
            foreach (FieldInfo info in fieldInfos)
            {
                string value = info.Name;
                string[] tail = value.Split('<');

                string name = string.Empty;
                if (tail.Length > 1)
                    name = tail[1].Split('>')[0];

                if (name == string.Empty)
                    continue;

                colNames.Add(name);
            }

            this.ColumnNames = colNames;
            this.Name = type.Name;
        }

        public void WriteToFile(string path = null, bool isAppend = false, bool isHeaderInclude = true, string name = null)
        { 
            string projectName = Assembly.GetCallingAssembly().GetName().Name;

            string exportPath = null;
            if (path == null)
                exportPath = string.Format(@"..{0}..{0}..{0}{1}\Output", Path.DirectorySeparatorChar, projectName);
            else
                exportPath = path;

            if (Directory.Exists(exportPath) == false)
                Directory.CreateDirectory(exportPath);

            string filePath = string.Format(@"{0}{1}{2}.csv", exportPath, Path.DirectorySeparatorChar, this.Name);
            if (name != null)
                filePath = string.Format(@"{0}{1}{2}.csv", exportPath, Path.DirectorySeparatorChar, name);

            StreamWriter sw = new StreamWriter(filePath, isAppend, System.Text.Encoding.Default);

            if (isHeaderInclude)
            {
                int i = 0;
                foreach (string colName in this.ColumnNames)
                {
                    sw.Write(colName.Trim());
                    if (i < this.ColumnNames.Count - 1)
                        sw.Write(",");

                    i++;
                }

                sw.Write(sw.NewLine);
            }

            foreach (IOutputRow row in this._rows)
            {
                int j = 0;
                foreach (string colName in this.ColumnNames)
                {
                    dynamic value = row.GetValue(colName);
                    string strVal = Convert.ToString(value);

                    if (strVal != null)
                        sw.Write(strVal.Trim());

                    if (j < this.ColumnNames.Count - 1)
                        sw.Write(",");

                    j++;
                }

                sw.Write(sw.NewLine);
            }

            sw.Close();
        }


        public void AddToView(int viewNumber, IComparable key, IOutputRow row)
        {
            if (this.Views == null)
                this.Views = new Dictionary<int, Dictionary<IComparable, List<IOutputRow>>>();

            Dictionary<IComparable, List<IOutputRow>> values;
            if (this.Views.TryGetValue(viewNumber, out values) == false)
            {
                values = new Dictionary<IComparable, List<IOutputRow>>();
                this.Views.Add(viewNumber, values);
            }

            List<IOutputRow> rows;
            if (values.TryGetValue(key, out rows) == false)
                values.Add(key, new List<IOutputRow>() { row });
            else
                rows.Add(row);
        }

        public void AddRow(IOutputRow row)
        {
            foreach (KeyValuePair<int, HashSet<string>> value in row.KeyMappings)
            {
                IComparable rowKey = row.GetKey(value.Key);

                this.AddToView(value.Key, rowKey, row);
            }

            if (this._rows == null)
            {
                this._rows = new List<IOutputRow>();
                this._rows.Add(row);

                this.Init(row);

                return;
            }

            this._rows.Add(row);
        }

        public List<IOutputRow> FindRows(int viewNum, params string[] keys)
        {
            if (this._rows == null || this._rows.Count == 0)
                return new List<IOutputRow>();

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < keys.Length; i++)
            {
                if (i < keys.Length - 1)
                    stringBuilder.Append(string.Format("{0}@", keys[i]));
                else
                    stringBuilder.Append(string.Format("{0}", keys[i]));
            }

            string key = stringBuilder.ToString();

            Dictionary<IComparable, List<IOutputRow>> values;
            if (this.Views.TryGetValue(viewNum, out values) == false)
                return new List<IOutputRow>();

            List<IOutputRow> list = new List<IOutputRow>();
            if (values.TryGetValue(key, out list))
            {
                return list;
            }

            return new List<IOutputRow>();
        }

    }

}
