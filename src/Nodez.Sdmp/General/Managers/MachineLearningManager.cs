// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.Interfaces;
using Nodez.Sdmp.MachineLearningHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.General.Managers
{
    public class MachineLearningManager
    {
        private static Lazy<MachineLearningManager> lazy = new Lazy<MachineLearningManager>(() => new MachineLearningManager());

        public static MachineLearningManager Instance { get { return lazy.Value; } }

        public void Reset() { lazy = new Lazy<MachineLearningManager>(); }

        public MLContext MLContext { get; private set; }

        public IDataView DataView { get; private set; }

        public ITransformer Model { get; private set; }

        public IEstimator<ITransformer> Trainer { get; private set; }

        public EstimatorChain<ColumnConcatenatingTransformer> DataProcessPipeline { get; private set; }

        public int CurrentSolutionCount { get; private set; }

        internal void InitializeMLContext()
        {
            this.MLContext = new MLContext();
        }

        internal void SetDataView(IDataView dataView)
        {
            this.DataView = dataView;
        }

        internal void SetDataProcessPipeline(EstimatorChain<ColumnConcatenatingTransformer> pipeline)
        {
            this.DataProcessPipeline = pipeline;
        }

        internal bool IsFitModel(int trainingPeriod, int stageIndex)
        {
            int solutionCount = SolutionManager.Instance.Solutions.Count;

            if (solutionCount == this.CurrentSolutionCount)
                return false;

            if (solutionCount % trainingPeriod != 0)
                return false;

            this.CurrentSolutionCount = solutionCount;

            return true;
        }

        public void SetTrainer(IEstimator<ITransformer> trainer)
        {
            this.Trainer = trainer;
        }

        public void SetModel(ITransformer model) 
        {
            this.Model = model;
        }

        internal bool FitClusterPredictionModel(List<State> states, bool isEvaluate = false)
        {
            MachineLearningControl mlControl = MachineLearningControl.Instance;

            int clusterCount = mlControl.GetClusterCount();

            IDataView dataView = mlControl.CreateClusterPredictionTrainData(states);

            if (dataView == null || dataView.GetRowCount() <= clusterCount)
                return false;

            this.SetDataView(dataView);

            this.SetDataProcessPipeline(mlControl.GetClusterPredictionDataProcessPipeline());

            this.SetTrainer(mlControl.GetClusterPredictionTrainer(clusterCount));

            var trainPipeline = this.DataProcessPipeline.Append(this.Trainer);

            try
            {
                var model = trainPipeline.Fit(this.DataView);
                this.SetModel(model);
            }
            catch 
            {
                return false;
            }

            return true;

            //string solverName = SolverManager.Instance.CurrentSolverName;
            //string dateString = SolverManager.Instance.GetEngineStartTime(solverName).ToString("yyyyMMdd_HHmmss");
            //string exportDirPath = string.Format(@"{0}\ML", SolverManager.Instance.GetOutputDirectoryPath(solverName));

            //bool isExportOutputs = mlControl.IsExportOutputs();
            //if (isExportOutputs)
            //{
            //    if (Directory.Exists(exportDirPath) == false)
            //        Directory.CreateDirectory(exportDirPath);

            //    string modelName = string.Format("Model_Clustering_{0}", dateString);

            //    string filePath = string.Format(@"{0}\{1}.zip", exportDirPath, modelName);
            //    this.MLContext.Model.Save(this.Model, this.DataView.Schema, filePath);
            //}

            //if (isEvaluate)
            //{
            //    this.EvaluateClusterPredictionModel(isExportOutputs, exportDirPath, dateString);
            //}
        }

        internal void FitValuePredictionModel(bool isEvaluate = false)
        {
            MachineLearningControl mlControl = MachineLearningControl.Instance;

            IDataView dataView = mlControl.CreateValuePredictionTrainData();

            if (dataView == null || dataView.GetRowCount() == 0)
                return;

            this.SetDataView(dataView);

            this.SetDataProcessPipeline(mlControl.GetValuePredictionDataProcessPipeline());

            this.SetTrainer(mlControl.GetValuePredictionTrainer());

            var trainPipeline = this.DataProcessPipeline.Append(this.Trainer);

            var model = trainPipeline.Fit(this.DataView);

            this.SetModel(model);

            string solverName = SolverManager.Instance.CurrentSolverName;
            string dateString = SolverManager.Instance.GetEngineStartTime(solverName).ToString("yyyyMMdd_HHmmss");
            string exportDirPath = string.Format(@"{0}\ML", SolverManager.Instance.GetOutputDirectoryPath(solverName));

            bool isExportOutputs = mlControl.IsExportOutputs();
            if (isExportOutputs)
            {
                if (Directory.Exists(exportDirPath) == false)
                    Directory.CreateDirectory(exportDirPath);

                string modelName = string.Format("Model_{0}", dateString);

                string filePath = string.Format(@"{0}\{1}.zip", exportDirPath, modelName);
                this.MLContext.Model.Save(this.Model, this.DataView.Schema, filePath);
            }

            if (isEvaluate)
            {
                this.EvaluateValuePredictionModel(isExportOutputs, exportDirPath, dateString);
            }
        }

        internal void EvaluateClusterPredictionModel(bool isExportOutputs, string exportDirPath, string dateString)
        {
            MachineLearningControl mlControl = MachineLearningControl.Instance;
            var dataProcessPipeline = this.DataProcessPipeline;

            var data = this.MLContext.Data.TrainTestSplit(this.DataView, 0.2);
            var trainingDataView = data.TrainSet;
            var testDataView = data.TestSet;

            var trainingPipeline = dataProcessPipeline.Append(this.Trainer);

            Console.WriteLine("=============== Training the model ===============");
            var trainedModel = trainingPipeline.Fit(trainingDataView);

            Console.WriteLine("===== Evaluating Model's accuracy with Test data =====");

            IDataView predictions = trainedModel.Transform(testDataView);

            var metrics = this.MLContext.Clustering.Evaluate(data: predictions);

            MLConsoleHelper.PrintClusteringMetrics(trainedModel.ToString(), metrics);

            if (isExportOutputs)
            {
                string trainFilePath = string.Format(@"{0}\{1}_{2}.csv", exportDirPath, "Train", dateString);
                string testFilePath = string.Format(@"{0}\{1}_{2}.csv", exportDirPath, "Test", dateString);

                // Save train split
                using (var fileStream = File.Create(trainFilePath))
                {
                    MLContext.Data.SaveAsText(trainingDataView, fileStream, separatorChar: ',', headerRow: true, schema: true, false, true);
                }

                // Save test split 
                using (var fileStream = File.Create(testFilePath))
                {
                    MLContext.Data.SaveAsText(testDataView, fileStream, separatorChar: ',', headerRow: true, schema: true, false, true);
                }
            }
        }

        internal void EvaluateValuePredictionModel(bool isExportOutputs, string exportDirPath, string dateString)
        {
            MachineLearningControl mlControl = MachineLearningControl.Instance;
            var dataProcessPipeline = this.DataProcessPipeline;

            var data = this.MLContext.Data.TrainTestSplit(this.DataView, 0.2);
            var trainingDataView = data.TrainSet;
            var testDataView = data.TestSet;

            var trainingPipeline = dataProcessPipeline.Append(this.Trainer);

            Console.WriteLine("=============== Training the model ===============");
            var trainedModel = trainingPipeline.Fit(trainingDataView);

            Console.WriteLine("===== Evaluating Model's accuracy with Test data =====");

            IDataView predictions = trainedModel.Transform(testDataView);

            var metrics = this.MLContext.Regression.Evaluate(data: predictions);

            MLConsoleHelper.PrintRegressionMetrics(trainedModel.ToString(), metrics);

            if (isExportOutputs)
            {
                string trainFilePath = string.Format(@"{0}\{1}_{2}.csv", exportDirPath, "Train", dateString);
                string testFilePath = string.Format(@"{0}\{1}_{2}.csv", exportDirPath, "Test", dateString);

                // Save train split
                using (var fileStream = File.Create(trainFilePath))
                {
                    MLContext.Data.SaveAsText(trainingDataView, fileStream, separatorChar: ',', headerRow: true, schema: true, false, true);
                }

                // Save test split 
                using (var fileStream = File.Create(testFilePath))
                {
                    MLContext.Data.SaveAsText(testDataView, fileStream, separatorChar: ',', headerRow: true, schema: true, false, true);
                }
            }
        }
    }
}
