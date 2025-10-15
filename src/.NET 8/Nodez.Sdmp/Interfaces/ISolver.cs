// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.Interface;
using Nodez.Data.Managers;
using Nodez.Sdmp.Constants;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.LogHelper;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Nodez.Sdmp.Interfaces
{
    public abstract class ISolver
    {
        public string ProjectName { get; protected set; }

        public string OutputDirectoryPath { get; protected set; }

        public DateTime EngineStartTime { get; protected set; }

        public DateTime EngineEndTime { get; protected set; }

        public string Name { get; protected set; }

        public Stopwatch StopWatch { get; protected set; }

        public TimeSpan TimeLimit { get; protected set; }

        public ObjectiveFunctionType ObjectiveFunctionType { get; protected set; }

        public int PrimalSolutionUpdatePeriod { get; protected set; }

        public int DualBoundUpdatePeriod { get; protected set; }

        public int ValueFuctionEstimateUpdatePeriod { get; protected set; }

        public int OnlineTrainingPeriod { get; protected set; }

        public int LogPeriod { get; protected set; }

        public int CurrentStateIndex { get; protected set; }

        public int CurrentStageIndex { get; protected set; }

        public Dictionary<string, State> VisitedStates { get; protected set; }

        public FastPriorityQueue<State> TransitionQueue { get; protected set; }

        public int TransitionIndex { get; protected set; }

        public bool IsUsePrimalBound { get; protected set; }

        public bool IsUseDualBound { get; protected set; }

        public bool IsUseValueFunctionEstimate { get; protected set; }

        public double PruneTolerance { get; protected set; }

        public bool IsApplyStateFiltering { get; protected set; }

        public bool IsApplyApproximation { get; protected set; }

        public bool IsApplyMachineLearning { get; protected set; }

        public bool IsOnlineLearning { get; protected set; }

        public bool IsOfflineLearning { get; protected set; }

        public bool IsApplyStateClustering { get; protected set; }

        public IRunConfig RunConfig { get; protected set; }

        public bool IsInitialized { get; protected set; }

        public string SolverEndMessage { get; protected set; }

        public void Initialize(List<object> controls, List<object> managers)
        {
            if (CheckRunConfig() == false)
            {
                LogWriter.WriteLineConsoleOnly(Messages.INVALID_RUN_CONFIG);
                return;
            }

            this.RegisterControls(controls);
            this.RegisterManagers(managers);

            SolverManager.Instance.SetRunConfig(this.RunConfig);
            SolverManager.Instance.SetCurrentSolverName(this.RunConfig.SOLVER_NAME);
            this.Name = SolverManager.Instance.CurrentSolverName;

            this.IsInitialized = true;
        }

        public void Initialize(List<object> controls)
        {
            if (CheckRunConfig() == false)
            {
                LogWriter.WriteLineConsoleOnly(Messages.INVALID_RUN_CONFIG);
                return;
            }

            this.RegisterControls(controls);

            SolverManager.Instance.SetRunConfig(this.RunConfig);
            SolverManager.Instance.SetCurrentSolverName(this.RunConfig.SOLVER_NAME);
            this.Name = SolverManager.Instance.CurrentSolverName;

            this.IsInitialized = true;
        }

        protected void SetOutputDirectory()
        {
            this.OutputDirectoryPath = SolverControl.Instance.GetOutputDirectoryPath(this.Name);
            SolverManager.Instance.SetOutputDirectoryPath(this.Name, this.OutputDirectoryPath);

            if (Directory.Exists(this.OutputDirectoryPath) == false)
                Directory.CreateDirectory(this.OutputDirectoryPath);
        }

        protected void SetConsole()
        {
            string engineStartTime = SolverManager.Instance.GetEngineStartTime(this.Name).ToString("yyyyMMdd_HHmmss");

            StreamWriter fileWriter = new StreamWriter($"{this.OutputDirectoryPath}{Path.DirectorySeparatorChar}{Constants.Constants.CONSOLE_OUTPUT_LOG}_{RunConfig.RUN_SEQ}_{this.Name}_{engineStartTime}.txt");
            fileWriter.AutoFlush = true;

            LogWriter.SetFileWriter(fileWriter);
        }

        protected void RegisterControls(List<object> controls)
        {
            ControlManager manager = ControlManager.Instance;

            foreach (object control in controls)
            {
                string name = control.GetType().BaseType.Name;
                manager.RegisterControl(name, control);
            }
        }

        protected void RegisterManagers(List<object> managers)
        {
            ControlManager manager = ControlManager.Instance;

            foreach (object mgr in managers)
            {
                string name = mgr.GetType().BaseType.Name;
                manager.RegisterManager(name, mgr);
            }
        }

        public State GetNextState()
        {
            return this.TransitionQueue.Dequeue();
        }

        public virtual void AddNextStates(List<State> states)
        {
            foreach (State state in states)
            {
                float priority = state.Index;

                this.TransitionQueue.Enqueue(state, priority);
            }
        }

        public virtual void AddNextState(State state)
        {
            float priority = state.Index;

            this.TransitionQueue.Enqueue(state, priority);
        }

        protected void UpdatePriorityQueue(State state)
        {

        }

        protected virtual List<General.DataModel.StateActionMap> GetStateActionMaps(State state)
        {
            ActionControl actionControl = ActionControl.Instance;

            List<General.DataModel.StateActionMap> maps = actionControl.GetStateActionMaps(state);

            return maps;
        }

        protected virtual List<General.DataModel.StateTransition> GetStateTransitions(List<General.DataModel.StateActionMap> stateActionMaps)
        {
            StateTransitionControl stateTransitionControl = StateTransitionControl.Instance;

            List<StateTransition> trans = new List<StateTransition>();
            foreach (StateActionMap map in stateActionMaps)
            {
                trans.AddRange(stateTransitionControl.GetStateTransitions(map));
            }

            return trans;
        }

        protected virtual void DoInitialStateTransitions(State initialState)
        {
            ActionControl transitionControl = ActionControl.Instance;
            StateManager stateManager = StateManager.Instance;
            StateControl stateControl = StateControl.Instance;
            ApproximationControl approxControl = ApproximationControl.Instance;

            List<State> newStates = new List<State>();

            List<General.DataModel.StateActionMap> initStateActionMaps = this.GetStateActionMaps(initialState);
            foreach (StateActionMap map in initStateActionMaps)
            {
                map.PostActionState.SetKey(stateControl.GetKey(map.PostActionState));
                map.PostActionState.IsPostActionState = true;
                map.PostActionState.PreActionState = initialState;
            }

            List<General.DataModel.StateTransition> initStateTransitions = this.GetStateTransitions(initStateActionMaps);

            Stage nextStage = new Stage(initialState.Stage.Index + 1);
            foreach (General.DataModel.StateTransition tran in initStateTransitions)
            {
                State toState = tran.ToState;

                tran.ToState.Stage = nextStage;

                toState.Index = ++this.CurrentStateIndex;
                toState.SetKey(stateControl.GetKey(toState));
                tran.SetIndex(++this.TransitionIndex);

                double cost = tran.Cost;
                double nextValue = initialState.CurrentBestValue + cost;

                toState.CurrentBestValue = nextValue;

                toState.SetPrevBestState(initialState);

                stateManager.SetLinks(tran);
                stateManager.AddState(toState, nextStage);

                if (this.VisitedStates.ContainsKey(toState.Key) == false)
                    this.VisitedStates.Add(toState.Key, toState);

                newStates.Add(toState);
            }

            if (this.CheckLocalFilteringCondition(initialState.Stage.Index))
            {
                int maximumTransitionCount = approxControl.GetLocalTransitionCount();
                int approximationStartStageIndex = approxControl.GetApproximationStartStageIndex();

                newStates = approxControl.FilterLocalStates(initialState, newStates, maximumTransitionCount);
            }

            this.AddNextStates(newStates);
        }

        private bool CheckRunConfig()
        {
            bool isValid = true;

            if (this.RunConfig == null)
                isValid = false;

            if (string.IsNullOrEmpty(this.RunConfig.SOLVER_NAME))
                isValid = false;

            if (string.IsNullOrEmpty(this.RunConfig.OBJECTIVE_FUNCTION_TYPE))
                isValid = false;

            return isValid;
        }

        private void SetSolverEnd(string endMessage)
        {
            this.SolverEndMessage = endMessage;
        }

        public void Run()
        {
            if (this.RunConfig.IS_RUN == false)
                return;

            StateControl stateControl = StateControl.Instance;
            ActionControl transitionControl = ActionControl.Instance;
            SolverControl solverControl = SolverControl.Instance;
            DataControl dataControl = DataControl.Instance;
            BoundControl boundControl = BoundControl.Instance;
            EventControl eventControl = EventControl.Instance;
            LogControl logControl = LogControl.Instance;
            ApproximationControl approxControl = ApproximationControl.Instance;
            MachineLearningControl mlControl = MachineLearningControl.Instance;

            BoundManager boundManager = BoundManager.Instance;
            ApproximationManager approxManager = ApproximationManager.Instance;
            SolutionManager solutionManager = SolutionManager.Instance;
            SolverManager solverManager = SolverManager.Instance;
            StateManager stateManager = StateManager.Instance;
            DataManager dataManager = DataManager.Instance;
            StateTransitionManager transitionManager = StateTransitionManager.Instance;
            MachineLearningManager mlManager = MachineLearningManager.Instance;

            if (this.IsInitialized == false)
            {
                LogWriter.WriteConsoleOnly(Messages.SOLVER_IS_NOT_INITIALIZED);
                eventControl.OnDoneSolve();
                return;
            }

            IData data = dataControl.GetData();
            dataManager.SetData(data);
            eventControl.OnDoneDataLoad();

            this.StopWatch.Start();
            this.EngineStartTime = DateTime.Now;

            solverManager.SetEngineStartTime(this.Name, this.EngineStartTime);
            solverManager.ClearStatusLogs();

            if (solverControl.IsWriteOutput())
            {
                this.SetOutputDirectory();
                this.SetConsole();
            }

            if (data == null)
            {
                LogWriter.WriteLine(Messages.DATA_IS_NULL);
                eventControl.OnDoneSolve();
                return;
            }

            try
            {
                eventControl.OnBeginSolve();

                ObjectiveFunctionType objectiveFunctionType = solverControl.GetObjectiveFuntionType(this.RunConfig);
                SetObjectiveFunctionType(objectiveFunctionType);

                this.TimeLimit = TimeSpan.FromSeconds(solverControl.GetRunMaxTime());
                solverManager.SetTimeLimit(this.TimeLimit);
                solverManager.SetObjectiveFunctionType(objectiveFunctionType);
                solverManager.SetStopWatch(this.StopWatch);

                logControl.WriteStartLog(this);

                solutionManager.SetObjectiveFunctionType(objectiveFunctionType);
                boundManager.SetPrimalBound(boundControl.GetInitialPrimalBound(objectiveFunctionType));
                boundManager.SetDualBound(boundControl.GetInitialDualBound(objectiveFunctionType));

                this.ProjectName = solverControl.GetProjectName();
                this.PrimalSolutionUpdatePeriod = boundControl.GetPrimalSolutionUpdatePeriod();
                this.DualBoundUpdatePeriod = boundControl.GetDualBoundUpdatePeriod();
                this.ValueFuctionEstimateUpdatePeriod = approxControl.GetValueFunctionEstimateUpdatePeriod();
                this.LogPeriod = logControl.GetLogPeriod();
                this.IsUsePrimalBound = boundControl.IsUsePrimalBound();
                this.IsUseDualBound = boundControl.IsUseDualBound();
                this.IsUseValueFunctionEstimate = approxControl.IsUseValueFunctionEstimate();
                this.OnlineTrainingPeriod = mlControl.GetOnlineTrainingPeriod();
                this.PruneTolerance = boundControl.GetPruneTolerance();
                this.IsApplyStateFiltering = approxControl.IsApplyStateFiltering();
                this.IsApplyApproximation = approxControl.IsApplyApproximation();
                this.IsOnlineLearning = mlControl.IsOnlineLearning();
                this.IsOfflineLearning = mlControl.IsOfflineLearning();
                this.IsApplyStateClustering = mlControl.IsApplyStateClustering();

                if (this.IsOnlineLearning || this.IsOfflineLearning || this.IsApplyStateClustering)
                    this.IsApplyMachineLearning = true;

                if (this.IsApplyMachineLearning)
                {
                    mlManager.InitializeMLContext();
                }

                if (mlControl.IsLoadModelFile())
                {
                    string modelPath = mlControl.GetModelFilePath();

                    if (modelPath != null && File.Exists(modelPath))
                    {
                        var trainedModel = mlManager.MLContext.Model.Load(modelPath, out var modelInputSchema);
                        mlManager.SetModel(trainedModel);
                    }
                }

                State initialState = stateControl.GetInitialState();
                initialState.Index = 0;
                initialState.IsInitial = true;

                initialState.SetKey(stateControl.GetKey(initialState));

                stateManager.SetInitialState(initialState);

                Stage initStage = new Stage(0, initialState);

                stateManager.AddState(initialState, initStage);

                if (initialState == null)
                {
                    this.SetSolverEnd(Messages.INITIAL_STATE_IS_NULL);
                    return;
                }

                this.VisitedStates.Add(initialState.Key, initialState);

                Solution initialFeasibleSol = stateControl.GetFeasibleSolution(initialState);

                if (initialFeasibleSol != null)
                {
                    if (this.IsUsePrimalBound)
                    {
                        double firstPrimalBound = boundControl.GetPrimalBound(initialFeasibleSol);

                        initialState.SetPrimalBound(firstPrimalBound);
                        boundManager.SetRootPrimalBound(firstPrimalBound);
                        boundManager.UpdateBestPrimalBound(initialState, firstPrimalBound, this.ObjectiveFunctionType, StopWatch.Elapsed);
                        solutionManager.AddSolution(initialFeasibleSol, false, true);
                    }
                }

                if (this.IsOfflineLearning)
                {
                    logControl.WriteRandomSolutionGenerationStartLog();

                    List<Solution> randomSolutions = mlControl.GetRandomSolutions();
                    if (randomSolutions != null)
                    {
                        foreach (Solution sol in randomSolutions)
                        {
                            solutionManager.AddSolution(sol);
                            double primalBound = boundControl.GetPrimalBound(sol);

                            initialState.SetPrimalBound(primalBound);
                            boundManager.UpdateBestPrimalBound(initialState, primalBound, this.ObjectiveFunctionType, StopWatch.Elapsed);
                        }
                    }

                    logControl.WriteRandomSolutionGenerationEndLog();
                }

                if (this.IsUseDualBound)
                {
                    double firstDualBound = boundControl.GetDualBound(initialState);

                    initialState.SetDualBound(firstDualBound);
                    boundManager.UpdateBestDualBound(initialState, firstDualBound, this.ObjectiveFunctionType, StopWatch.Elapsed);
                    boundManager.SetRootDualBound(firstDualBound);
                }

                if (this.IsUseValueFunctionEstimate)
                {
                    double firstEstimationValue = approxControl.GetValueFunctionEstimate(initialState);

                    initialState.SetValueFunctionEstimate(firstEstimationValue);
                }

                if (solutionManager.CheckOptimalityCondition())
                {
                    solutionManager.AddSolution(initialFeasibleSol, true);
                    logControl.WriteStatusLog(initialState, StopWatch.Elapsed);

                    StatusLog log = LogControl.Instance.GetStatusLog(initialState, StopWatch.Elapsed);
                    SolverManager.Instance.AddStatusLog(log);

                    this.SetSolverEnd(Messages.FOUND_OPTIMAL_SOLUTION);
                    return;
                }

                this.DoInitialStateTransitions(initialState);

                this.DoSolve();

                if (string.IsNullOrEmpty(this.SolverEndMessage))
                {
                    this.SetSolverEnd(Messages.SEARCH_FINISHED);
                }
            }
            finally
            {
                this.EngineEndTime = DateTime.Now;
                solverManager.SetEngineEndTime(this.Name, this.EngineEndTime);
                this.StopWatch.Reset();

                if (solutionManager.OptimalSolution != null)
                {
                    LogWriter.WriteLine($"{Messages.FOUND_OPTIMAL_SOLUTION} (Obj. Val.: {solutionManager.BestSolution.Value})");
                }
                else if (stateManager.FinalState != null)
                {
                    Solution sol = solutionManager.GetSolution(stateManager.FinalState);

                    if (solutionManager.BestSolution != null)
                    {
                        if (ObjectiveFunctionType == ObjectiveFunctionType.Maximize)
                        {
                            if (sol.Value < solutionManager.BestSolution.Value)
                            {
                                sol = solutionManager.BestSolution;
                            }
                        }
                        else
                        {
                            if (sol.Value > solutionManager.BestSolution.Value)
                            {
                                sol = solutionManager.BestSolution;
                            }
                        }
                    }

                    solutionManager.AddSolution(sol, true);
                    LogWriter.WriteLine($"{Messages.FOUND_SOLUTION} (Obj. Val.: {solutionManager.BestSolution.Value})");
                }
                else if (solutionManager.BestSolution != null && solutionManager.OptimalSolution == null)
                {
                    Solution bestSol = solutionManager.BestSolution;

                    solutionManager.AddSolution(bestSol);
                    LogWriter.WriteLine($"{Messages.FOUND_SOLUTION} (Obj. Val.: {solutionManager.BestSolution.Value})");
                }
                else if (solutionManager.BestSolution == null && solutionManager.OptimalSolution == null)
                {
                    LogWriter.WriteLine(Messages.NO_SOLUTION_EXIST);
                }

                LogControl.Instance.WriteEndLog(this.SolverEndMessage);

                if (this.IsOfflineLearning)
                {
                    mlManager.FitValuePredictionModel(true);
                }

                if (logControl.IsWriteStatusLog())
                {
                    solverManager.WriteStatusLogs();
                }

                if (logControl.IsWriteStateInfoLog())
                {
                    solverManager.WriteStateInfoLogs();
                }

                eventControl.OnDoneSolve();

                this.Reset();
            }
        }

        private void Reset()
        {
            ApproximationManager.Instance.Reset();
            BoundManager.Instance.Reset();
            ControlManager.Instance.Reset();
            MachineLearningManager.Instance.Reset();
            SolutionManager.Instance.Reset();
            SolverManager.Instance.Reset();
            StateManager.Instance.Reset();
            StateTransitionManager.Instance.Reset();

            GC.Collect();
        }

        protected void RemoveVisitedStates(Dictionary<string, double> fixedKeys, int stageIndex, int memoryMaxCount)
        {
            if (this.CurrentStageIndex < stageIndex)
            {
                fixedKeys.Clear();
                this.VisitedStates.Clear();
                this.CurrentStageIndex++;
            }
        }

        private void UpdateGlobalDualBound(Dictionary<string, double> fixedStates, int stageIndex, ObjectiveFunctionType objectiveFunctionType, TimeSpan elapsedTime)
        {
            if (fixedStates.Count <= 0)
                return;

            if (this.CurrentStageIndex < stageIndex)
            {
                double globalDualBound = 0;

                if (objectiveFunctionType == ObjectiveFunctionType.Minimize)
                {
                    globalDualBound = fixedStates.Min(x => x.Value);
                }
                else
                {
                    globalDualBound = fixedStates.Max(x => x.Value);
                }

                BoundManager.Instance.UpdateBestDualBound(null, globalDualBound, objectiveFunctionType, elapsedTime);
            }
        }

        private void FilterStatesByApproximation(State currentState, FastPriorityQueue<State> transitionQueue, ObjectiveFunctionType objectiveFunctionType, double pruneTolerance)
        {
            BoundControl boundControl = BoundControl.Instance;
            ApproximationControl approxControl = ApproximationControl.Instance;
            SolverManager solverManager = SolverManager.Instance;
            StateManager stateManager = StateManager.Instance;

            List<State> stateList = transitionQueue.ToList();

            int loopCount = 0;
            double minEstimationValue = double.MaxValue;

            for (int i = 0; i < stateList.Count; i++)
            {
                State state = stateList.ElementAt(i);

                if (state.IsFinal)
                    continue;

                double estimatedValue = approxControl.GetValueFunctionEstimate(state);
                state.ValueFunctionEstimate = estimatedValue;

                if (minEstimationValue > state.ValueFunctionEstimate)
                    minEstimationValue = state.ValueFunctionEstimate;

                loopCount++;
            }

            if (minEstimationValue <= 0)
                return;

            if (objectiveFunctionType == ObjectiveFunctionType.Minimize)
                stateList = stateList.OrderBy(x => x.ValueFunctionEstimate).ToList();
            else
                stateList = stateList.OrderByDescending(x => x.ValueFunctionEstimate).ToList();

            double minTransitionCost = approxControl.GetMinimumTransitionCost();
            double multiplier = approxControl.GetMultiplier();
            int transitionCount = approxControl.GetApproximationTransitionCount();

            List<State> selectedStateList = new List<State>();
            int count = 0;
            foreach (State state in stateList)
            {
                if (approxControl.CanPruneByApproximation(state, objectiveFunctionType, minEstimationValue, minTransitionCost, multiplier, pruneTolerance))
                    continue;

                if (transitionCount <= count)
                    break;

                selectedStateList.Add(state);
                count++;
            }

            foreach (State state in stateList)
            {
                if (state.IsInitial == false && state.IsFinal == false)
                {
                    solverManager.AddStateInfoLog(LogControl.Instance.GetStateInfoLog(state, false));
                }
            }

            foreach (State state in selectedStateList)
            {
                if (state.IsInitial == false && state.IsFinal == false)
                {
                    solverManager.AddStateInfoLog(LogControl.Instance.GetStateInfoLog(state, true));
                }
            }

            int totalStateCount = stateList != null ? stateList.Count : 0;
            int selectedStateCount = selectedStateList != null ? selectedStateList.Count : 0;
            int filteredStateCount = totalStateCount - selectedStateCount;

            stateManager.SetFilteredStateCount(currentState.Stage.Index, filteredStateCount);

            this.TransitionQueue.Clear();
            this.AddNextStates(selectedStateList);
        }

        private bool CheckApproximationCondition(int stageIndex)
        {
            if (this.IsApplyApproximation == false)
                return false;

            ApproximationControl approxControl = ApproximationControl.Instance;

            if (this.CurrentStageIndex >= stageIndex)
                return false;

            int startIndex = approxControl.GetApproximationStartStageIndex();
            if (stageIndex < startIndex)
                return false;

            return true;
        }

        private bool CheckStateClusteringCondition(int stageIndex)
        {
            if (this.IsApplyStateClustering == false)
                return false;

            MachineLearningControl mlControl = MachineLearningControl.Instance;

            if (this.CurrentStageIndex >= stageIndex)
                return false;

            int startIndex = mlControl.GetStateClusteringStartStageIndex();
            if (stageIndex < startIndex)
                return false;

            int endIndex = mlControl.GetStateClusteringStopStageIndex();
            if (stageIndex > endIndex)
                return false;

            return true;
        }

        private bool CheckGlobalFilteringCondition(int stageIndex)
        {
            if (this.IsApplyStateFiltering == false)
                return false;

            ApproximationControl approxControl = ApproximationControl.Instance;
            StateFilteringType filteringType = approxControl.GetStateFilteringType();

            if (filteringType == StateFilteringType.Local)
                return false;

            if (this.CurrentStageIndex >= stageIndex)
                return false;

            int startIndex = approxControl.GetGlobalFilteringStartStageIndex();
            if (stageIndex < startIndex)
                return false;

            return true;
        }

        protected bool CheckLocalFilteringCondition(int stageIndex)
        {
            if (this.IsApplyStateFiltering == false)
                return false;

            ApproximationControl approxControl = ApproximationControl.Instance;
            StateFilteringType filteringType = approxControl.GetStateFilteringType();

            if (filteringType == StateFilteringType.Global)
                return false;

            int startIndex = approxControl.GetLocalFilteringStartStageIndex();
            if (stageIndex < startIndex)
                return false;

            return true;
        }

        private void ClusterGlobalStates(FastPriorityQueue<State> transitionQueue)
        {
            LogControl.Instance.WriteStateClusteringStartLog(transitionQueue.Count);

            MachineLearningControl mlControl = MachineLearningControl.Instance;
            MachineLearningManager mlManager = MachineLearningManager.Instance;

            if (mlManager.FitClusterPredictionModel(transitionQueue.ToList(), false) == false)
                return;

            if (mlManager.Model == null)
                return;

            foreach (State state in transitionQueue)
            {
                if (state.IsFinal)
                    continue;

                Tuple<int, double> value = mlControl.GetPredictedClusterValue(state);
                state.ClusterID = value.Item1;
                state.ClusterDistance = value.Item2;
            }

            LogControl.Instance.WriteStateClusteringEndLog();
        }

        private void FilterGlobalStates(State currentState, FastPriorityQueue<State> transitionQueue, ObjectiveFunctionType objectiveFunctionType, double pruneTolerance, bool isApplyStateClustering)
        {
            LogControl.Instance.WriteGlobalFilteringStartLog(currentState.Stage.Index, transitionQueue.Count);

            ApproximationControl approxControl = ApproximationControl.Instance;
            SolverManager solverManager = SolverManager.Instance;
            StateManager stateManager = StateManager.Instance;

            StateFilteringType filteringType = approxControl.GetStateFilteringType();

            if (filteringType == StateFilteringType.Local)
                return;

            if (this.CurrentStageIndex >= currentState.Stage.Index)
                return;

            int startIndex = approxControl.GetApproximationStartStageIndex();
            if (currentState.Stage.Index < startIndex)
                return;

            int globalMaximumTransitCount = approxControl.GetGlobalTransitionCount();

            List<State> stateList = transitionQueue.ToList();

            List<State> selectedStateList = approxControl.FilterGlobalStates(stateList, globalMaximumTransitCount, objectiveFunctionType, pruneTolerance, isApplyStateClustering);

            foreach (State state in stateList)
            {
                if (state.IsInitial == false && state.IsFinal == false)
                {
                    solverManager.AddStateInfoLog(LogControl.Instance.GetStateInfoLog(state, false));
                }
            }

            foreach (State state in selectedStateList)
            {
                if (state.IsInitial == false && state.IsFinal == false)
                {
                    solverManager.AddStateInfoLog(LogControl.Instance.GetStateInfoLog(state, true));
                }
            }

            int totalStateCount = stateList != null ? stateList.Count : 0;
            int selectedStateCount = selectedStateList != null ? selectedStateList.Count : 0;
            int filteredStateCount = totalStateCount - selectedStateCount;

            stateManager.SetFilteredStateCount(currentState.Stage.Index, filteredStateCount);

            this.TransitionQueue.Clear();

            this.AddNextStates(selectedStateList);

            LogControl.Instance.WriteGlobalFilteringEndLog();
        }

        private void DoSolve()
        {
            StateControl stateControl = StateControl.Instance;
            StateTransitionControl transitionControl = StateTransitionControl.Instance;
            SolverControl solverControl = SolverControl.Instance;
            DataControl dataControl = DataControl.Instance;
            BoundControl boundControl = BoundControl.Instance;
            EventControl eventControl = EventControl.Instance;
            ApproximationControl approxControl = ApproximationControl.Instance;
            MachineLearningControl mlControl = MachineLearningControl.Instance;

            BoundManager boundManager = BoundManager.Instance;
            SolutionManager solutionManager = SolutionManager.Instance;
            StateManager stateManager = StateManager.Instance;
            DataManager dataManager = DataManager.Instance;
            StateTransitionManager transitionManager = StateTransitionManager.Instance;
            LogControl logControl = LogControl.Instance;
            ApproximationManager approxManager = ApproximationManager.Instance;
            MachineLearningManager mlManager = MachineLearningManager.Instance;

            int stateLogPeriod = this.LogPeriod;
            int primalSolutionUpdatePeriod = this.PrimalSolutionUpdatePeriod;
            int dualBoundUpdatePeriod = this.DualBoundUpdatePeriod;
            int valueFunctionEstimateUpdatePeriod = this.ValueFuctionEstimateUpdatePeriod;
            int omlineTrainingPeriod = this.OnlineTrainingPeriod;
            bool isUsePrimalBound = this.IsUsePrimalBound;
            bool isUseDualBound = this.IsUseDualBound;
            bool isUseValueFuctionEstimate = this.IsUseValueFunctionEstimate;
            double pruneTolerance = this.PruneTolerance;

            ObjectiveFunctionType objectiveFunctionType = this.ObjectiveFunctionType;
            FastPriorityQueue<State> transitionQueue = this.TransitionQueue;

            int memoryClearPeriod = 10000;

            int loopCount = -1;
            Dictionary<string, double> fixedStateBounds = new Dictionary<string, double>();

            while (transitionQueue.Count > 0)
            {
                if (this.StopWatch.Elapsed.TotalSeconds >= this.TimeLimit.TotalSeconds)
                {
                    this.SetSolverEnd($"{Messages.TIME_LIMIT} Time Limit={this.TimeLimit.TotalSeconds} sec.");
                    return;
                }

                loopCount++;

                State peek = transitionQueue.First();

                int stageIndex = peek.Stage.Index;

                if (this.CheckStateClusteringCondition(stageIndex))
                {
                    eventControl.OnBeforeFilterStates(transitionQueue.AsEnumerable());
                    this.ClusterGlobalStates(transitionQueue);
                    eventControl.OnAfterFilterStates(transitionQueue.AsEnumerable());
                }

                if (this.CheckGlobalFilteringCondition(stageIndex))
                {
                    eventControl.OnBeforeFilterStates(transitionQueue.AsEnumerable());
                    this.FilterGlobalStates(peek, transitionQueue, objectiveFunctionType, pruneTolerance, IsApplyStateClustering);
                    eventControl.OnAfterFilterStates(transitionQueue.AsEnumerable());
                }

                if (this.CheckApproximationCondition(stageIndex))
                {
                    eventControl.OnBeforeFilterStates(transitionQueue.AsEnumerable());
                    this.FilterStatesByApproximation(peek, transitionQueue, objectiveFunctionType, pruneTolerance);
                    eventControl.OnAfterFilterStates(transitionQueue.AsEnumerable());
                }

                if (transitionQueue.Count == 0)
                    break;

                State state = this.GetNextState();

                if (state.IsFinal)
                {
                    stateManager.SetFinalState(state);
                    break;
                }

                state.IsFixed = true;

                this.UpdateGlobalDualBound(fixedStateBounds, stageIndex, objectiveFunctionType, StopWatch.Elapsed);

                if (this.CurrentStageIndex < stageIndex)
                {
                    eventControl.OnStageChanged(state.Stage);
                }

                if (loopCount % stateLogPeriod == 0 || this.CurrentStageIndex < stageIndex)
                {
                    logControl.WriteStatusLog(state, StopWatch.Elapsed);

                    StatusLog log = LogControl.Instance.GetStatusLog(state, StopWatch.Elapsed);
                    SolverManager.Instance.AddStatusLog(log);
                }

                this.RemoveVisitedStates(fixedStateBounds, stageIndex, memoryClearPeriod);

                eventControl.OnVisitState(state);

                bool isCalcDualBound = boundManager.IsCalculateDualBound(isUseDualBound, dualBoundUpdatePeriod, stageIndex, loopCount);
                if (isCalcDualBound && state.IsSetDualBound == false)
                {
                    double dualBound = boundControl.GetDualBound(state);
                    state.SetDualBound(dualBound);
                }

                bool isCalcValueFunctionEstimate = approxManager.IsCalculateValueFunctionEstimate(isUseValueFuctionEstimate, valueFunctionEstimateUpdatePeriod, stageIndex, loopCount);
                if (isCalcValueFunctionEstimate && state.IsSetValueFunctionEstimate == false)
                {
                    double valueFunctionEstimate = approxControl.GetValueFunctionEstimate(state);
                    state.SetValueFunctionEstimate(valueFunctionEstimate);
                }

                bool isCalcPrimalBound = boundManager.IsCalculatePrimalBound(isUsePrimalBound, primalSolutionUpdatePeriod, stageIndex, loopCount);
                if (isCalcPrimalBound && state.IsSetPrimalBound == false)
                {
                    Solution feasibleSol = stateControl.GetFeasibleSolution(state);

                    if (feasibleSol != null)
                    {
                        solutionManager.AddSolution(feasibleSol);

                        double primalBound = boundControl.GetPrimalBound(feasibleSol);
                        state.SetPrimalBound(primalBound);
                        boundManager.UpdateBestPrimalBound(state, primalBound, objectiveFunctionType, StopWatch.Elapsed);
                    }
                }

                if (this.IsOnlineLearning && mlManager.IsFitModel(omlineTrainingPeriod, stageIndex))
                {
                    mlManager.FitValuePredictionModel();
                }

                if (solutionManager.CheckOptimalityCondition())
                {
                    solutionManager.AddSolution(solutionManager.BestSolution, true);
                    logControl.WriteStatusLog(state, StopWatch.Elapsed);

                    StatusLog log = LogControl.Instance.GetStatusLog(state, StopWatch.Elapsed);
                    SolverManager.Instance.AddStatusLog(log);

                    this.SetSolverEnd(Messages.FOUND_OPTIMAL_SOLUTION);
                    return;
                }

                if (stateControl.CanPruneByOptimality(state, objectiveFunctionType, pruneTolerance))
                {
                    logControl.WritePruneLog(state);
                    stateManager.PruneState(state);
                    continue;
                }

                if (this.IsApplyStateFiltering == false && this.IsApplyApproximation == false)
                {
                    if (fixedStateBounds.ContainsKey(state.Key) == false)
                        fixedStateBounds.Add(state.Key, state.CurrentBestValue + state.DualBound);
                }

                List<General.DataModel.StateTransition> trans = new List<General.DataModel.StateTransition>();
                if (state.IsLastStage)
                {
                    General.DataModel.StateTransition finalTran = transitionControl.GetFinalStateTransition(state);
                    trans.Add(finalTran);
                }
                else
                {
                    List<General.DataModel.StateActionMap> stateActionMaps = this.GetStateActionMaps(state);
                    foreach (StateActionMap map in stateActionMaps)
                    {
                        map.PostActionState.SetKey(stateControl.GetKey(map.PostActionState));
                        map.PostActionState.IsPostActionState = true;
                        map.PostActionState.PreActionState = map.PostActionState;
                    }

                    trans = this.GetStateTransitions(stateActionMaps);
                }

                this.DoTransitions(state, trans);
            }
        }

        protected virtual void DoTransitions(State state, List<General.DataModel.StateTransition> nextTrans)
        {
            StateManager stateManager = StateManager.Instance;
            StateControl stateControl = StateControl.Instance;
            EventControl eventControl = EventControl.Instance;
            ApproximationControl approxControl = ApproximationControl.Instance;

            List<State> newStates = new List<State>();
            Stage nextStage = new Stage(state.Stage.Index + 1);

            foreach (General.DataModel.StateTransition nextTran in nextTrans)
            {
                nextTran.SetIndex(++this.TransitionIndex);

                State toState = nextTran.ToState;
                toState.Stage = nextStage;

                if (toState.IsLastStage)
                {
                    toState.Stage.SetIsLastStage(true);
                }

                if (toState.IsFinal)
                {
                    toState.SetKey(Constants.Constants.FINAL);
                    toState.Stage.SetIsFinalStage(true);
                }
                else
                {
                    toState.SetKey(stateControl.GetKey(toState));
                }

                double cost = nextTran.Cost;
                double nextValue = state.CurrentBestValue + cost;

                State visited;
                if (this.VisitedStates.TryGetValue(toState.Key, out visited))
                {
                    stateManager.SetLinks(nextTran);

                    if (this.ObjectiveFunctionType == ObjectiveFunctionType.Minimize)
                    {
                        if (nextValue < visited.CurrentBestValue)
                        {
                            visited.PrevBestStates.Clear();
                            visited.SetPrevBestState(state);

                            visited.CurrentBestValue = nextValue;

                            this.UpdatePriorityQueue(visited);
                        }
                        else if (nextValue == visited.CurrentBestValue)
                        {
                            visited.SetPrevBestState(state);
                        }
                    }
                    else if (this.ObjectiveFunctionType == ObjectiveFunctionType.Maximize)
                    {
                        if (nextValue > visited.CurrentBestValue)
                        {
                            visited.PrevBestStates.Clear();
                            visited.SetPrevBestState(state);

                            visited.CurrentBestValue = nextValue;

                            this.UpdatePriorityQueue(visited);
                        }
                        else if (nextValue == visited.CurrentBestValue)
                        {
                            visited.SetPrevBestState(state);
                        }
                    }
                }
                else
                {
                    toState.Index = ++this.CurrentStateIndex;

                    stateManager.SetLinks(nextTran);
                    stateManager.AddState(toState, nextStage);

                    eventControl.OnVisitToState(state, toState);

                    toState.SetPrevBestState(state);

                    toState.CurrentBestValue = nextValue;

                    this.VisitedStates.Add(toState.Key, toState);
                    toState.IsVisited = true;

                    newStates.Add(toState);
                }
            }

            if (this.CheckLocalFilteringCondition(state.Stage.Index))
            {
                int maximumTransitionCount = approxControl.GetLocalTransitionCount();
                int approximationStartStageIndex = approxControl.GetApproximationStartStageIndex();

                newStates = approxControl.FilterLocalStates(state, newStates, maximumTransitionCount);
            }

            this.AddNextStates(newStates);
        }

        public TimeSpan GetRunTime()
        {
            return this.StopWatch.Elapsed;
        }

        protected void SetObjectiveFunctionType(ObjectiveFunctionType objectiveFunctionType)
        {
            this.ObjectiveFunctionType = objectiveFunctionType;
        }

        public SolutionManager GetSolutionManager()
        {
            return SolutionManager.Instance;
        }

        public BoundManager GetBoundManager()
        {
            return BoundManager.Instance;
        }

        public ControlManager GetControlManager()
        {
            return ControlManager.Instance;
        }

        public StateTransitionManager GetStateTransitionManager()
        {
            return StateTransitionManager.Instance;
        }

        public StateManager GetStateManager()
        {
            return StateManager.Instance;
        }

        public DataManager GetDataManager()
        {
            return DataManager.Instance;
        }
    }
}