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

        private List<StatusLog> _statusLogs { get; set; }

        private List<StateInfoLog> _stateInfoLogs { get; set; }

        private IRunConfig _runConfig { get; set; }

        public SolverManager() 
        {
            this._engineStartTime = new Dictionary<string, DateTime>();
            this._engineEndTime = new Dictionary<string, DateTime>();
            this._bestSolutionDateTime = new Dictionary<string, DateTime>();
            this._rootSolutionDateTime = new Dictionary<string, DateTime>();
            this._outputDirectoryPath = new Dictionary<string, string>();
            this._statusLogs = new List<StatusLog>();
            this._stateInfoLogs = new List<StateInfoLog>();
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

        public void ClearStatusLogs() 
        {
            this._statusLogs.Clear();
        }

        public void ClearStateInfoLogs()
        {
            this._stateInfoLogs.Clear();
        }

        public void AddStatusLog(StatusLog log) 
        {
            this._statusLogs.Add(log);
        }

        public void AddStateInfoLog(StateInfoLog log, bool autoFlush = true)
        {
            this._stateInfoLogs.Add(log);

            if (autoFlush)
            {
                if (this._stateInfoLogs.Count > 5000)
                {
                    OutputTable stateInfoLogTable = new OutputTable();

                    foreach (StateInfoLog item in _stateInfoLogs)
                    {
                        stateInfoLogTable.AddRow(item);
                    }

                    string engineStartTime = SolverManager.Instance.GetEngineStartTime(CurrentSolverName).ToString("yyyyMMdd_HHmmss");
                    string dirPath = SolverManager.Instance.GetOutputDirectoryPath(CurrentSolverName);

                    if (Directory.Exists(dirPath) == false)
                        Directory.CreateDirectory(dirPath);

                    if (OutputManager.Instance.GetOutput(Constants.Constants.STATE_INFO_LOG) == null)
                    {
                        stateInfoLogTable.WriteToFile(dirPath, false, true, $"{Constants.Constants.STATE_INFO_LOG}_{CurrentSolverName}_{engineStartTime}");
                        OutputManager.Instance.SetOutput(stateInfoLogTable.Name, stateInfoLogTable);
                    }
                    else
                    {
                        stateInfoLogTable.WriteToFile(dirPath, true, false, $"{Constants.Constants.STATE_INFO_LOG}_{CurrentSolverName}_{engineStartTime}");
                    }

                    this.ClearStateInfoLogs();
                    stateInfoLogTable.Clear();
                }
            }
        }

        public void AddStateInfoLogs(List<StateInfoLog> logs, bool autoFlush = true) 
        {
            this._stateInfoLogs.AddRange(logs);

            if (autoFlush)
            {
                if (this._stateInfoLogs.Count >= 5000)
                {
                    OutputTable stateInfoLogTable = new OutputTable();
                                                     
                    foreach (StateInfoLog log in _stateInfoLogs) 
                    {
                        stateInfoLogTable.AddRow(log);
                    }

                    string engineStartTime = SolverManager.Instance.GetEngineStartTime(CurrentSolverName).ToString("yyyyMMdd_HHmmss");
                    string dirPath = SolverManager.Instance.GetOutputDirectoryPath(CurrentSolverName);

                    if (Directory.Exists(dirPath) == false)
                        Directory.CreateDirectory(dirPath);

                    if (OutputManager.Instance.GetOutput(Constants.Constants.STATE_INFO_LOG) == null)
                    {
                        stateInfoLogTable.WriteToFile(dirPath, false, true, $"{Constants.Constants.STATE_INFO_LOG}_{CurrentSolverName}_{engineStartTime}");
                        OutputManager.Instance.SetOutput(stateInfoLogTable.Name, stateInfoLogTable);
                    }
                    else 
                    {
                        stateInfoLogTable.WriteToFile(dirPath, true, false, $"{Constants.Constants.STATE_INFO_LOG}_{CurrentSolverName}_{engineStartTime}");
                    }

                    this.ClearStateInfoLogs();
                    stateInfoLogTable.Clear();
                }
            }
        }

        public List<StatusLog> GetStatusLogs() 
        {
            return this._statusLogs;
        }

        public void WriteStatusLogs() 
        {
            OutputTable table = new OutputTable();
            foreach (StatusLog log in this._statusLogs) 
            {
                table.AddRow(log);
            }

            string engineStartTime = SolverManager.Instance.GetEngineStartTime(CurrentSolverName).ToString("yyyyMMdd_HHmmss");
            string dirPath = SolverManager.Instance.GetOutputDirectoryPath(CurrentSolverName);

            if (Directory.Exists(dirPath) == false)
                Directory.CreateDirectory(dirPath);

            table.WriteToFile(dirPath, false, true, $"{Constants.Constants.STATUS_LOG}_{CurrentSolverName}_{engineStartTime}");
            OutputManager.Instance.SetOutput(table.Name, table);
        }

        public void WriteStateInfoLogs()
        {
            OutputTable table = new OutputTable();
            foreach (StateInfoLog log in this._stateInfoLogs)
            {
                table.AddRow(log);
            }

            string engineStartTime = SolverManager.Instance.GetEngineStartTime(CurrentSolverName).ToString("yyyyMMdd_HHmmss");
            string dirPath = SolverManager.Instance.GetOutputDirectoryPath(CurrentSolverName);

            if (Directory.Exists(dirPath) == false)
                Directory.CreateDirectory(dirPath);

            if (OutputManager.Instance.GetOutput(Constants.Constants.STATE_INFO_LOG) == null)
            {
                table.WriteToFile(dirPath, false, true, $"{Constants.Constants.STATE_INFO_LOG}_{CurrentSolverName}_{engineStartTime}");
                OutputManager.Instance.SetOutput(table.Name, table);
            }
            else
            {
                table.WriteToFile(dirPath, true, false, $"{Constants.Constants.STATE_INFO_LOG}_{CurrentSolverName}_{engineStartTime}");
            }
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
