// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Nodez.Project.SchedulingTemplate.MyObjects;
using Nodez.Sdmp;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.MachineLearningHelper;
using Nodez.Sdmp.Scheduling.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.SchedulingTemplate.Controls
{
    public class UserMachineLearningControl : MachineLearningControl
    {
        private static readonly Lazy<UserMachineLearningControl> lazy = new Lazy<UserMachineLearningControl>(() => new UserMachineLearningControl());

        public static new UserMachineLearningControl Instance { get { return lazy.Value; } }

        public override bool IsOnlineLearning() 
        {
            return false;        
        }

        public override bool IsOfflineLearning() 
        {
            return false;
        }

        public override bool IsLoadModelFile() 
        {
            return false;
        }

        public override string GetModelFilePath() 
        {
            return @"";
        }

        public override bool IsApplyStateClustering()
        {
            return false;
        }

        public override int GetOnlineTrainingPeriod() 
        {
            return 100;
        }

        public override double GetPredictedValue(State state) 
        {
            MachineLearningManager mlManager = MachineLearningManager.Instance;

            if (mlManager.Model == null)
                return 0;

            SchedulingState schedState = state as SchedulingState;

            // Set Input / Output Schema
            SchemaDefinition definedInputSchema = SchemaDefinition.Create(typeof(MLInputData));
            var vectorItemType = ((VectorDataViewType)definedInputSchema[nameof(MLInputData.JobProcessStatus)].ColumnType)
                .ItemType;
            definedInputSchema[nameof(MLInputData.JobProcessStatus)].ColumnType = new VectorDataViewType(vectorItemType, schedState.JobProcessStatus.Length);

            SchemaDefinition definedOutputSchema = SchemaDefinition.Create(typeof(MLOutputData));
            definedOutputSchema[nameof(MLOutputData.PredictedValue)].ColumnName = "Score";

            var predEngine = mlManager.MLContext.Model.CreatePredictionEngine<MLInputData, MLOutputData>(mlManager.Model, true, definedInputSchema, definedOutputSchema);

            MLInputData inputData = new MLInputData();
            inputData.StageIndex = schedState.Stage.Index;
            inputData.JobProcessStatus = Array.ConvertAll<int, float>(schedState.JobProcessStatus, new Converter<int, float>(UtilityHelper.IntToFloat));

            double predictValue = predEngine.Predict(inputData).PredictedValue;

            return predictValue;

        }

        public override IDataView CreateValuePredictionTrainData() 
        {
            MachineLearningManager mlManager = MachineLearningManager.Instance;

            List<MLInputData> rows = new List<MLInputData>();
            int jobProcessStatusLength = 0;
            foreach (Solution solution in SolutionManager.Instance.Solutions)
            {
                IOrderedEnumerable<KeyValuePair<int, State>> states = solution.States.OrderBy(x => x.Key);
                int makespan = (int)solution.Value;

                foreach (KeyValuePair<int, State> item in states)
                {
                    int stageIndex = item.Key;
                    SchedulingState state = item.Value as SchedulingState;

                    if (state.IsInitial)
                        continue;

                    MLInputData row = new MLInputData();
                    row.StageIndex = stageIndex;
                    row.Makespan = makespan;
                    row.JobProcessStatus = Array.ConvertAll<int, float>(state.JobProcessStatus, new Converter<int, float>(UtilityHelper.IntToFloat));

                    jobProcessStatusLength = row.JobProcessStatus.Length;

                    rows.Add(row);
                }
            }

            var max = rows.Max(x => x.Makespan);

            // Set Input / Output Schema
            SchemaDefinition definedSchema = SchemaDefinition.Create(typeof(MLInputData));
            var vectorItemType = ((VectorDataViewType)definedSchema[nameof(MLInputData.JobProcessStatus)].ColumnType)
                .ItemType;
            definedSchema[nameof(MLInputData.JobProcessStatus)].ColumnType = new VectorDataViewType(vectorItemType, jobProcessStatusLength);

            IDataView dataView = mlManager.MLContext.Data.LoadFromEnumerable<MLInputData>(rows, definedSchema);

            return dataView;
        }

        public override EstimatorChain<ColumnConcatenatingTransformer> GetValuePredictionDataProcessPipeline()
        {
            MachineLearningManager mlManager = MachineLearningManager.Instance;

            var pipeline = mlManager.MLContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(MLInputData.Makespan))
                            .Append(mlManager.MLContext.Transforms.Concatenate("Features", nameof(MLInputData.StageIndex), nameof(MLInputData.JobProcessStatus))).AppendCacheCheckpoint(mlManager.MLContext);

            //MLConsoleHelper.PeekDataViewInConsole(mlManager.MLContext, mlManager.DataView, pipeline, 1000);
            //MLConsoleHelper.PeekVectorColumnDataInConsole(mlManager.MLContext, "Features", mlManager.DataView, pipeline, 1000);

            return pipeline;
        }

        public override IEstimator<ITransformer> GetValuePredictionTrainer()
        {
            return MachineLearningManager.Instance.MLContext.Regression.Trainers.Sdca();
        }

        public override bool IsExportOutputs()
        {
            return true;
        }
    }
}
