// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.DataModel;
using Nodez.Data.Managers;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.General.Managers
{
    public class SolverManager
    {
        private static Lazy<SolverManager> lazy = new Lazy<SolverManager>(() => new SolverManager());

        public static SolverManager Instance { get { return lazy.Value; } }

        public void Reset() { lazy = new Lazy<SolverManager>(); }

        public string CurrentSolverName { get; private set; }

        public IRunConfig RunConfig { get { return GetRunConfig(); } }

        public ObjectiveFunctionType ObjectiveFunctionType { get; private set; }

        public Stopwatch StopWatch { get; private set; }

        private Dictionary<string, DateTime> _engineStartTime { get; set; }

        private Dictionary<string, DateTime> _engineEndTime { get; set; }

        private Dictionary<string, DateTime> _bestSolutionDateTime { get; set; }

        private Dictionary<string, DateTime> _rootSolutionDateTime { get; set; }

        private Dictionary<string, string> _outputDirectoryPath { get; set; }

        private List<Log> _logs { get; set; }

        private IRunConfig _runConfig { get; set; }

        public SolverManager() 
        {
            this._engineStartTime = new Dictionary<string, DateTime>();
            this._engineEndTime = new Dictionary<string, DateTime>();
            this._bestSolutionDateTime = new Dictionary<string, DateTime>();
            this._rootSolutionDateTime = new Dictionary<string, DateTime>();
            this._outputDirectoryPath = new Dictionary<string, string>();
            this._logs = new List<Log>();
        }

        public void SetRunConfig(IRunConfig runConfig) 
        {
            this._runConfig = runConfig;
        }

        public IRunConfig GetRunConfig() 
        {
            return _runConfig;
        }

        public void SetObjectiveFunctionType(ObjectiveFunctionType objectiveFunctionType) 
        {
            this.ObjectiveFunctionType = objectiveFunctionType;
        }

        public void SetStopWatch(Stopwatch stopWatch) 
        {
            this.StopWatch = stopWatch;
        }

        public void SetCurrentSolverName(string solverName) 
        {
            this.CurrentSolverName = solverName;
        }

        public void ClearStateLogs() 
        {
            this._logs.Clear();
        }

        public void AddLog(Log log) 
        {
            this._logs.Add(log);
        }

        public List<Log> GetLogs() 
        {
            return this._logs;
        }

        public void ExportLogs() 
        {
            OutputTable table = new OutputTable();
            foreach (Log log in this._logs) 
            {
                table.AddRow(log);
            }

            string engineStartTime = SolverManager.Instance.GetEngineStartTime(CurrentSolverName).ToString("yyyyMMdd_HHmmss");
            string dirPath = SolverManager.Instance.GetOutputDirectoryPath(CurrentSolverName);

            if (Directory.Exists(dirPath) == false)
                Directory.CreateDirectory(dirPath);

            table.WriteToFile(dirPath, false, true, $"{Constants.Constants.LOG}_{CurrentSolverName}_{engineStartTime}");
            OutputManager.Instance.SetOutput(table.Name, table);
        }

        public void SetOutputDirectoryPath(string solverName, string outputDirectoryPath)
        {
            if (this._outputDirectoryPath.ContainsKey(solverName))
                this._outputDirectoryPath[solverName] = outputDirectoryPath;
            else
                this._outputDirectoryPath.Add(solverName, outputDirectoryPath);
        }

        public void SetEngineStartTime(string solverName, DateTime engineStartTime)
        {
            if (solverName == null)
                return;

            if (this._engineStartTime.ContainsKey(solverName))
                this._engineStartTime[solverName] = engineStartTime;
            else
                this._engineStartTime.Add(solverName, engineStartTime);
        }

        public void SetEngineEndTime(string solverName, DateTime engineEndTime)
        {
            if (solverName == null)
                return;

            if (this._engineEndTime.ContainsKey(solverName))
                this._engineEndTime[solverName] = engineEndTime;
            else
                this._engineEndTime.Add(solverName, engineEndTime);
        }

        public void SetBestSolutionDateTime(string solverName, DateTime bestSolutionDateTime) 
        {
            if (solverName == null)
                return;

            if (this._bestSolutionDateTime.ContainsKey(solverName))
                this._bestSolutionDateTime[solverName] = bestSolutionDateTime;
            else
                this._bestSolutionDateTime.Add(solverName, bestSolutionDateTime);
        }

        public void SetRootSolutionDateTime(string solverName, DateTime rootSolutionDateTime)
        {
            if (solverName == null)
                return;

            if (this._rootSolutionDateTime.ContainsKey(solverName))
                this._rootSolutionDateTime[solverName] = rootSolutionDateTime;
            else
                this._rootSolutionDateTime.Add(solverName, rootSolutionDateTime);
        }

        public DateTime GetEngineStartTime(string solverName)
        {
            this._engineStartTime.TryGetValue(solverName, out DateTime engineStartTime);

            return engineStartTime;
        }

        public DateTime GetEngineEndTime(string solverName)
        {
            this._engineEndTime.TryGetValue(solverName, out DateTime engineEndTime);

            return engineEndTime;
        }

        public DateTime GetBestSolutionDateTime(string solverName) 
        {
            this._bestSolutionDateTime.TryGetValue(solverName, out DateTime bestSolutionDateTime);

            return bestSolutionDateTime;
        }

        public DateTime GetRootSolutionDateTime(string solverName)
        {
            this._rootSolutionDateTime.TryGetValue(solverName, out DateTime rootSolutionDateTime);

            return rootSolutionDateTime;
        }

        public TimeSpan GetRunTime(string solverName) 
        {
            return this.GetEngineEndTime(solverName).Subtract(this.GetEngineStartTime(solverName));
        }

        public TimeSpan GetBestSolutionTime(string solverName)
        {
            return this.GetBestSolutionDateTime(solverName).Subtract(this.GetEngineStartTime(solverName));
        }

        public TimeSpan GetRootSolutionTime(string solverName)
        {
            return this.GetRootSolutionDateTime(solverName).Subtract(this.GetEngineStartTime(solverName));
        }

        public string GetOutputDirectoryPath(string solverName)
        {
            this._outputDirectoryPath.TryGetValue(solverName, out string path);

            return path;
        }
    }
}
