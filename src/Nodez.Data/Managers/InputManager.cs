// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.Controls;
using Nodez.Data.DataModel;
using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace Nodez.Data.Managers
{
    public class InputManager
    {
        private static readonly Lazy<InputManager> lazy = new Lazy<InputManager>(() => new InputManager());

        public static InputManager Instance { get { return lazy.Value; } }

        public static Dictionary<string, InputTable> Inputs { get { return _inputs; } }

        private static Dictionary<string, string> _pathInputsMappings;

        private static Dictionary<string, InputTable> _inputs;

        private static Dictionary<Tuple<string, string>, string> _columnNameMappings;

        private static InputControl _inputControl;

        public void DeleteInputFiles(List<string> filterList = null)
        {
            string projectName = Assembly.GetCallingAssembly().GetName().Name;
            string inputPath = string.Format(@"..\..\..\{0}\Data", projectName);

            if (Directory.Exists(inputPath) == false)
                return;

            DirectoryInfo dinfo = new DirectoryInfo(inputPath);
            FileInfo[] dfiles = dinfo.GetFiles();

            foreach (FileInfo dfile in dfiles)
            {
                if (filterList != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(dfile.FullName);
                    if (filterList.Contains(fileName) == false)
                        dfile.Delete();
                }
                else
                    dfile.Delete();
            }
        }

        public void SetInputControl(InputControl inputControl)
        {
            _inputControl = inputControl;
        }

        public void SetPathInputsMappings(string key, string path)
        {
            if (_pathInputsMappings.ContainsKey(key) == false)
                _pathInputsMappings.Add(key, path);
            else
                _pathInputsMappings[key] = path;
        }

        public void SetPathInputsMappings(Dictionary<string, string> pathMappings)
        {
            _pathInputsMappings = pathMappings;
        }

        public void ClearInputs()
        {
            _inputs.Clear();
            _pathInputsMappings.Clear();
            _columnNameMappings.Clear();
        }

        public void LoadInputs(List<string> tableNames, Assembly assembly =null, string inputPath = null, string typePath = null, bool skipExistingData = false, int maxLoadLineCount = 0)
        {
            if (assembly == null)
                assembly = Assembly.GetEntryAssembly();

            Console.WriteLine(string.Format("Project name: {0}", assembly.GetName().Name));
            Console.WriteLine("Start load data...");

            if (_inputControl == null)
                _inputControl = InputControl.Instance;

            if (_columnNameMappings == null)
                _columnNameMappings = new Dictionary<Tuple<string, string>, string>();

            Dictionary<string, string> pathMappings = _inputControl.GetInputsPathMappings(inputPath, tableNames);
            SetPathInputsMappings(pathMappings);

            Stopwatch stopwatch = new Stopwatch();
            foreach (KeyValuePair<string, string> item in _pathInputsMappings)
            {
                string key = item.Key;
                string path = item.Value;

                if (skipExistingData) 
                {
                    if (_inputs.ContainsKey(key))
                        continue;
                }

                stopwatch.Restart();
                Console.WriteLine(string.Format("Start loading file: [{0}]", key));

                Type rowType = null;
                if (typePath != null)
                    rowType = assembly.GetType(string.Format("{0}.{1}", typePath, key));
                else
                    rowType = assembly.GetType(string.Format("{0}.MyInputs.{1}", assembly.GetName().Name, key));

                if (rowType == null)
                    continue;

                InputTable table = new InputTable();
                table.Name = key;

                StreamReader reader = new StreamReader(path);

                string[] columnNames = null;
                List<string[]> lines = new List<string[]>();
                int lineCount = 0;
                while (!reader.EndOfStream)
                {
                    if (maxLoadLineCount != 0)
                    {
                        if (maxLoadLineCount < lineCount)
                            break;
                    }

                    string line = reader.ReadLine();
                    string[] orgValues = line.Split(',');
                    List<string> valueList = new List<string>();

                    foreach (string val in orgValues)
                    {
                        valueList.Add(val);
                    }

                    string[] values = valueList.ToArray();
                    if (lineCount == 0)
                    {
                        columnNames = values;
                        table.ColumnNames = valueList;
                        lineCount++;

                        Dictionary<string, string> mappings = _inputControl.GetColumnNameMappings(table);

                        foreach (KeyValuePair<string, string> map in mappings)
                        {
                            Tuple<string, string> keyAttr = Tuple.Create(table.Name, map.Key);
                            if (_columnNameMappings.ContainsKey(keyAttr) == false)
                                _columnNameMappings.Add(keyAttr, map.Value);
                        }

                        continue;
                    }

                    lines.Add(values);

                    IInputRow row = (IInputRow)Activator.CreateInstance(rowType);
                    row.SetInputTable(table);

                    for (int i = 0; i < columnNames.Length; i++)
                    {
                        if (string.IsNullOrEmpty(columnNames[i]))
                            continue;

                        values[i] = _inputControl.PersistInputDataLoad(table.Name, columnNames[i], lineCount, values[i]);

                        row.SetColumnValue(table.Name, columnNames[i], values[i], _columnNameMappings);
                    }

                    _inputControl.PersistInputData(row);

                    table.AddRow(row);

                    lineCount++;
                }

                this.SetInput(key, table);

                stopwatch.Stop();
                Console.WriteLine(string.Format("End loading file: [{0}] (Rows:{1},Time:{2})", key, lineCount - 1, stopwatch.Elapsed));

            }

            Console.WriteLine("End load data.");

        }

        public InputTable GetInput(string key)
        {
            InputTable input;

            if (_inputs.TryGetValue(key, out input))
                return input;

            return null;
        }

        public void SetInput(string key, InputTable input)
        {
            if (_inputs == null)
                _inputs = new Dictionary<string, InputTable>();

            if (_inputs.ContainsKey(key) == false)
                _inputs.Add(key, input);
            else
                _inputs[key] = input;
        }
    }
}
