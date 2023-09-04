// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Microsoft.ML;
using Microsoft.ML.Data;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.General.Controls
{
    public class MachineLearningControl
    {
        private static readonly Lazy<MachineLearningControl> lazy = new Lazy<MachineLearningControl>(() => new MachineLearningControl());

        public static MachineLearningControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.MachineLearningControl.ToString(), out object control))
                {
                    return (MachineLearningControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual bool IsOnlineLearning()
        {
            return false;
        }

        public virtual bool IsOfflineLearning()
        {
            return false;
        }

        public virtual bool IsLoadModelFile()
        {
            return false;
        }

        public virtual string GetModelFilePath()
        {
            return null;
        }

        public virtual bool IsApplyStateClustering() 
        {
            return false;
        }

        public virtual int GetClusterCount() 
        {
            return 3;
        }

        public virtual int GetStateClusteringStartStageIndex() 
        {
            return 0;
        }

        public virtual int GetStateClusteringStopStageIndex()
        {
            return 10;
        }

        public virtual int GetOnlineTrainingPeriod()
        {
            return 100;
        }

        public virtual int GetRandomSolutionGenerationCount() 
        {
            return 1000;
        }

        public virtual List<Solution> GetRandomSolutions()
        {
            return null;
        }

        public virtual double GetPredictedValue(State state)
        {
            return 0;
        }

        public virtual IDataView CreateValuePredictionTrainData()
        {
            return null;
        }

        public virtual EstimatorChain<ColumnConcatenatingTransformer> GetValuePredictionDataProcessPipeline()
        {
            return null;
        }

        public virtual IEstimator<ITransformer> GetValuePredictionTrainer() 
        {
            return MachineLearningManager.Instance.MLContext.Regression.Trainers.Sdca();
        }

        public virtual Tuple<int, double> GetPredictedClusterValue(State state)
        {
            return null;
        }

        public virtual IDataView CreateClusterPredictionTrainData(List<State> states)
        {
            return null;
        }

        public virtual EstimatorChain<ColumnConcatenatingTransformer> GetClusterPredictionDataProcessPipeline()
        {
            return null;
        }

        public virtual IEstimator<ITransformer> GetClusterPredictionTrainer(int numberOfClusters)
        {
            return MachineLearningManager.Instance.MLContext.Clustering.Trainers.KMeans("Features", numberOfClusters: numberOfClusters);
        }

        public virtual bool IsExportOutputs() 
        {
            return false;
        }
    }
}
