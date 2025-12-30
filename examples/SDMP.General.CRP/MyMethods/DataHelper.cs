// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.DataModel;
using Nodez.Data.Interface;
using Nodez.Data.Managers;
using SDMP.General.CRP.MyInputs;
using SDMP.General.CRP.MyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDMP.General.CRP.MyMethods
{
    public static class DataHelper
    {
        public static IData CreateData() 
        {
            CRPData data = new CRPData();

            data.SetupCostMatrix = GetSetupCostMatrix();

            List<CRPJob> jobs = CreateJobs();
            Dictionary<int, CRPConveyor> conveyors = CreateConveyors(jobs);
            CRPFactory factory = CreateFactory(conveyors);
            
            data.CRPFactory = factory;
            return data;
        }

        public static string CreateKey(int[] keys)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < keys.Length; i++)
            {
                if (i < keys.Length - 1)
                    stringBuilder.AppendFormat("{0}@", keys[i]);
                else
                    stringBuilder.AppendFormat("{0}", keys[i]);
            }

            return stringBuilder.ToString();
        }

        public static double GetSetupCost(CRPJob fromJob, CRPJob toJob) 
        {
            double cost = 0;

            if (fromJob == null || toJob == null)
                return cost;

            CRPData data = DataManager.Instance.Data as CRPData;
            cost = data.SetupCostMatrix[fromJob.Number, toJob.Number];

            return cost;
        }

        private static List<CRPJob> CreateJobs() 
        {
            InputTable table = InputManager.Instance.GetInput("JobColorInfo");

            List<CRPJob> jobs = new List<CRPJob>();

            for (int i = 1; i <= CRPParameter.JOBS_NUM; i++) 
            {
                CRPJob job = new CRPJob();
                job.SetNumber(i);

                JobColorInfo find = (JobColorInfo)table.FindRows(1, i).FirstOrDefault();

                if (find == null)
                    continue;

                CRPColor color = new CRPColor();
                color.SetColorNumber(find.COLOR);
                job.SetColor(color);

                jobs.Add(job);
            }

            return jobs;
        }

        private static Dictionary<int, CRPConveyor> CreateConveyors(List<CRPJob> jobs) 
        {
            Dictionary<int, CRPConveyor> conveyors = new Dictionary<int, CRPConveyor>();

            int k = 0;
            for (int i = 1; i <= CRPParameter.CONV_NUM; i++) 
            {
                CRPConveyor conveyor = new CRPConveyor();

                conveyor.ConveyorNum = i;

                for (int j = 1; j <= CRPParameter.JOBS_PER_CONV; j++)
                {
                    conveyor.Jobs.Enqueue(jobs.ElementAt(k));
                    k++;
                }
                
                conveyors.Add(i, conveyor);               
            }

            return conveyors;
        }

        private static CRPFactory CreateFactory(Dictionary<int, CRPConveyor> conveyors) 
        {
            CRPFactory factory = new CRPFactory();

            factory.Conveyors = conveyors;

            List<CRPJob> jobs = new List<CRPJob>();
            foreach (KeyValuePair<int, CRPConveyor> item in conveyors) 
            {
                item.Value.Factory = factory;
                jobs.AddRange(item.Value.Jobs);
            }

            factory.Jobs = jobs;

            return factory;
        }

        private static double[,] GetSetupCostMatrix() 
        {
            InputManager inputsManager = InputManager.Instance;
            InputTable table = inputsManager.GetInput("SetupCost");

            double[,] matrix = new double[CRPParameter.JOBS_NUM + 1, CRPParameter.JOBS_NUM + 1];

            for (int i = 1; i <= CRPParameter.JOBS_NUM; i++) 
            {
                for (int j = 1; j <= CRPParameter.JOBS_NUM; j++)
                { 
                    SetupCost cost = (SetupCost)table.FindRows(1, i, j).FirstOrDefault();

                    if (cost == null)
                        continue;

                    matrix[i, j] = cost.SETUP_COST;
                }
            }

            return matrix;
        }
    }
}
