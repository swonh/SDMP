using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nodez.Data.DataModel
{
    public class InputTable
    {
        public Dictionary<int, Dictionary<IComparable, List<IInputRow>>> Views;

        public List<string> ColumnNames;

        public List<IInputRow> _rows;

        public string Name;

        public List<IInputRow> Rows()
        {
            return this._rows;
        }

        public void Clear()
        {
            if (this._rows != null)
                this._rows.Clear();
        }

        private void Init(IInputRow row)
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

        public void WriteToFile(string path = null)
        {
            string projectName = Assembly.GetCallingAssembly().GetName().Name;

            string exportPath = null;
            if (path == null)
                exportPath = string.Format(@"..\..\..\{0}\Output", projectName);
            else
                exportPath = path;

            if (Directory.Exists(exportPath) == false)
                Directory.CreateDirectory(exportPath);

            string filePath = string.Format(@"{0}\{1}.csv", exportPath, this.Name);

            StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.Default);

            int i = 0;
            foreach (string colName in this.ColumnNames)
            {
                sw.Write(colName.Trim());
                if (i < this.ColumnNames.Count - 1)
                    sw.Write(",");

                i++;
            }

            sw.Write(sw.NewLine);

            foreach (IInputRow row in this._rows)
            {
                int j = 0;
                foreach (string colName in this.ColumnNames)
                {
                    dynamic value = row.GetValue(colName);
                    string strVal = Convert.ToString(value);

                    sw.Write(strVal.Trim());

                    if (j < this.ColumnNames.Count - 1)
                        sw.Write(",");

                    j++;
                }

                sw.Write(sw.NewLine);
            }

            sw.Close();
        }

        public void AddToView(int viewNumber, IComparable key, IInputRow row)
        {
            if (this.Views == null)
                this.Views = new Dictionary<int, Dictionary<IComparable, List<IInputRow>>>();

            Dictionary<IComparable, List<IInputRow>> values;
            if (this.Views.TryGetValue(viewNumber, out values) == false)
            {
                values = new Dictionary<IComparable, List<IInputRow>>();
                this.Views.Add(viewNumber, values);
            }

            List<IInputRow> rows;
            if (values.TryGetValue(key, out rows) == false)
                values.Add(key, new List<IInputRow>() { row });
            else
                rows.Add(row);
        }

        public void AddRow(IInputRow row)
        {
            foreach (KeyValuePair<int, HashSet<string>> value in row.KeyMappings)
            {
                IComparable rowKey = row.GetKey(value.Key);

                this.AddToView(value.Key, rowKey, row);
            }

            if (this._rows == null)
            {
                this._rows = new List<IInputRow>();
                this._rows.Add(row);

                this.Init(row);

                return;
            }

            this._rows.Add(row);
        }

        public List<IInputRow> FindRows(int viewNum, params object[] keys)
        {
            if (this._rows == null || this._rows.Count == 0)
                return new List<IInputRow>();

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < keys.Length; i++)
            {
                if (i < keys.Length - 1)
                    stringBuilder.Append(string.Format("{0}@", keys[i]));
                else
                    stringBuilder.Append(string.Format("{0}", keys[i]));
            }

            string key = stringBuilder.ToString();

            Dictionary<IComparable, List<IInputRow>> values;
            if (this.Views.TryGetValue(viewNum, out values) == false)
                return new List<IInputRow>();

            List<IInputRow> list = new List<IInputRow>();
            if (values.TryGetValue(key, out list))
            {
                return list;
            }

            return new List<IInputRow>();
        }

    }

}
