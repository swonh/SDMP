// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Microsoft.ML;
using Microsoft.ML.Data;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using SDMP.General.CRP.MyObjects;
using SDMP.General.CRP.MyObjects.MachineLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDMP.General.CRP.Controls
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
            return true;
        }

        public override string GetModelFilePath()
        {
            return null;
        }

        public override bool IsApplyStateClustering()
        {
            return true;
        }

        public override int GetClusterCount()
        {
            return 3;
        }

        public override int GetStateClusteringStartStageIndex()
        {
            return 0;
        }

        public override int GetStateClusteringStopStageIndex()
        {
            return Int32.MaxValue;
        }

        public override int GetOnlineTrainingPeriod()
        {
            return 100;
        }

        public override int GetRandomSolutionGenerationCount()
        {
            return 0;
        }

        public override List<Solution> GetRandomSolutions()
        {
            return null;
        }

        public override double GetPredictedValue(State state)
        {
            MachineLearningManager mlManager = MachineLearningManager.Instance;

            if (mlManager.Model == null)
                return 0;

            double predictValue = 0;

            return predictValue;
        }

        public override IDataView CreateValuePredictionTrainData()
        {
            IDataView dataView = null;

            return dataView;
        }

        public override EstimatorChain<ColumnConcatenatingTransformer> GetValuePredictionDataProcessPipeline()
        {
            EstimatorChain<ColumnConcatenatingTransformer> pipeline = null;

            return pipeline;
        }

        public override IEstimator<ITransformer> GetValuePredictionTrainer()
        {
            return MachineLearningManager.Instance.MLContext.Regression.Trainers.Sdca();
        }

        public override Tuple<int, double> GetPredictedClusterValue(State state)
        {
            MachineLearningManager mlManager = MachineLearningManager.Instance;
            MachineLearningControl mlControl = MachineLearningControl.Instance;

            if (mlManager.Model == null)
                return null;

            int clusterCount = mlControl.GetClusterCount();

            CRPState crpState = state as CRPState;

            // Set Input / Output Schema
            SchemaDefinition definedInputSchema = SchemaDefinition.Create(typeof(MLClusteringInputData));
            var vectorItemType = ((VectorDataViewType)definedInputSchema[nameof(MLClusteringInputData.JobCount)].ColumnType)
                .ItemType;
            definedInputSchema[nameof(MLClusteringInputData.JobCount)].ColumnType = new VectorDataViewType(vectorItemType, crpState.StateInfo.Count);
            vectorItemType = ((VectorDataViewType)definedInputSchema[nameof(MLClusteringInputData.ColorCount)].ColumnType)
                .ItemType;
            definedInputSchema[nameof(MLClusteringInputData.ColorCount)].ColumnType = new VectorDataViewType(vectorItemType, crpState.StateInfo.Count);

            SchemaDefinition definedOutputSchema = SchemaDefinition.Create(typeof(MLClusteringOutputData));
            definedOutputSchema[nameof(MLClusteringOutputData.PredictedClusterID)].ColumnName = "PredictedLabel";

            vectorItemType = ((VectorDataViewType)definedOutputSchema[nameof(MLClusteringOutputData.Distances)].ColumnType)
    .ItemType;
            definedOutputSchema[nameof(MLClusteringOutputData.Distances)].ColumnType = new VectorDataViewType(vectorItemType, clusterCount);
            definedOutputSchema[nameof(MLClusteringOutputData.Distances)].ColumnName = "Score";

            var predEngine = mlManager.MLContext.Model.CreatePredictionEngine<MLClusteringInputData, MLClusteringOutputData>(mlManager.Model, true, definedInputSchema, definedOutputSchema);

            int[] jobCountValue = new int[crpState.StateInfo.Count];
            int[] colorCountValue = new int[crpState.StateInfo.Count];

            foreach (KeyValuePair<int, CRPConveyor> item in crpState.StateInfo) 
            {
                jobCountValue[item.Key - 1] = item.Value.JobCount;
                colorCountValue[item.Key - 1] = crpState.GetColorCount(item.Key);
            }

            MLClusteringInputData inputData = new MLClusteringInputData();
            inputData.BestValue = (float)crpState.BestValue;
            inputData.JobCount = Array.ConvertAll<int, float>(jobCountValue, new Converter<int, float>(Nodez.Sdmp.UtilityHelper.IntToFloat));
            inputData.ColorCount = Array.ConvertAll<int, float>(colorCountValue, new Converter<int, float>(Nodez.Sdmp.UtilityHelper.IntToFloat));

            int clusterID = (int)predEngine.Predict(inputData).PredictedClusterID;
            double distance = predEngine.Predict(inputData).Distances[clusterID - 1];

            Tuple<int, double> value = Tuple.Create(clusterID, distance);

            return value;
        }

        public override IDataView CreateClusterPredictionTrainData(List<State> states)
        {
            MachineLearningManager mlManager = MachineLearningManager.Instance;

            List<MLClusteringInputData> rows = new List<MLClusteringInputData>();
            int jobCount = 0;
            int colorCount = 0;

            foreach (State state in states)
            {
                if (state.IsFinal)
                    break;

                CRPState crpState = state as CRPState;

                if (crpState.IsInitial)
                    continue;

                int[] jobCountValue = new int[crpState.StateInfo.Count];
                int[] colorCountValue = new int[crpState.StateInfo.Count];

                foreach (KeyValuePair<int, CRPConveyor> item in crpState.StateInfo)
                {
                    jobCountValue[item.Key - 1] = item.Value.JobCount;
                    colorCountValue[item.Key - 1] = crpState.GetColorCount(item.Key);
                }

                MLClusteringInputData row = new MLClusteringInputData();
                row.BestValue = (float)crpState.BestValue;
                row.JobCount = Array.ConvertAll<int, float>(jobCountValue, new Converter<int, float>(Nodez.Sdmp.UtilityHelper.IntToFloat));
                row.ColorCount = Array.ConvertAll<int, float>(colorCountValue, new Converter<int, float>(Nodez.Sdmp.UtilityHelper.IntToFloat));

                jobCount = row.JobCount.Length;
                colorCount = row.ColorCount.Length;

                rows.Add(row);
            }

            // Set Input / Output Schema
            SchemaDefinition definedSchema = SchemaDefinition.Create(typeof(MLClusteringInputData));
            var vectorItemType = ((VectorDataViewType)definedSchema[nameof(MLClusteringInputData.JobCount)].ColumnType)
                .ItemType;
            definedSchema[nameof(MLClusteringInputData.JobCount)].ColumnType = new VectorDataViewType(vectorItemType, jobCount);

            vectorItemType = ((VectorDataViewType)definedSchema[nameof(MLClusteringInputData.ColorCount)].ColumnType)
                .ItemType;
            definedSchema[nameof(MLClusteringInputData.ColorCount)].ColumnType = new VectorDataViewType(vectorItemType, colorCount);

            IDataView dataView = mlManager.MLContext.Data.LoadFromEnumerable<MLClusteringInputData>(rows, definedSchema);

            return dataView;
        }

        public override EstimatorChain<ColumnConcatenatingTransformer> GetClusterPredictionDataProcessPipeline()
        {
            MachineLearningManager mlManager = MachineLearningManager.Instance;

            var pipeline = mlManager.MLContext.Transforms
                            .Concatenate("Features", nameof(MLClusteringInputData.BestValue), nameof(MLClusteringInputData.JobCount), nameof(MLClusteringInputData.ColorCount)).AppendCacheCheckpoint(mlManager.MLContext);

            return pipeline;
        }

        public override IEstimator<ITransformer> GetClusterPredictionTrainer(int numberOfClusters)
        {
            return MachineLearningManager.Instance.MLContext.Clustering.Trainers.KMeans("Features", numberOfClusters: numberOfClusters);
        }

        public override bool IsExportOutputs()
        {
            return true;
        }
    }
}
