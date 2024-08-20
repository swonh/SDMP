// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Constants;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nodez.Sdmp.General.Managers
{
    public class SolutionManager
    {
        private static Lazy<SolutionManager> lazy = new Lazy<SolutionManager>(() => new SolutionManager());

        public static SolutionManager Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredManagers.TryGetValue(ManagerType.SolutionManager.ToString(), out object control))
                {
                    return (SolutionManager)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public void Reset() { lazy = new Lazy<SolutionManager>(); }

        public ObjectiveFunctionType ObjectiveFunctionType { get; private set; }

        public Solution BestSolution { get { return this._bestSolution; } }

        public Solution OptimalSolution { get; private set; }

        public Solution InitialSolution { get; private set; }

        public HashSet<Solution> Solutions { get { return this._solutions; } }

        private HashSet<Solution> _solutions;

        private Solution _bestSolution;

        public SolutionManager() 
        {
            this._solutions = new HashSet<Solution>();
        }

        public void SetObjectiveFunctionType(ObjectiveFunctionType objectiveFunctionType)
        {
            this.ObjectiveFunctionType = objectiveFunctionType;
        }

        public virtual Solution GetOptimalSolution(State finalState)
        {
            Solution optSol = new Solution(finalState.GetBestStatesBackward());

            return optSol;
        }

        public virtual void AddSolution(Solution solution, bool isOptimal = false, bool isInitial = false)
        {
            if (solution == null)
                return;

            if (this._solutions.Contains(solution) == false)
                this._solutions.Add(solution);

            this.UpdateBestSoltion(solution);

            if (isOptimal)
            {
                this.OptimalSolution = solution;
                solution.SetIsOptimal(true);
            }

            if (isInitial)
            {
                this.InitialSolution = solution;
            }
        }

        private void UpdateBestSoltion(Solution solution)
        {
            if (this.ObjectiveFunctionType == ObjectiveFunctionType.Maximize)
            {
                if (this._bestSolution == null)
                    this._bestSolution = solution;
                else if (this._bestSolution.Value < solution.Value)
                {
                    this._bestSolution = solution;
                }
            }
            else
            {
                if (this._bestSolution == null)
                    this._bestSolution = solution;                
                else if (this._bestSolution.Value > solution.Value)
                {
                    this._bestSolution = solution;
                }
            }
        }

        public virtual bool CheckOptimalityCondition()
        {
            BoundManager boundManager = BoundManager.Instance;
            BoundControl boundControl = BoundControl.Instance;

            double bestPrimalBound = boundManager.BestPrimalBound;
            double bestDualBound = boundManager.BestDualBound;

            double relativeDualityGap = Math.Round(Math.Abs(bestPrimalBound - bestDualBound) / bestDualBound, 6);
            double absDualityGap = Math.Round(Math.Abs(bestPrimalBound - bestDualBound), 6);

            bool isUseAbsOptimalityGap = boundControl.UseAbsoluteOptimalityGap();

            double optimalityGap = 0;
            if (isUseAbsOptimalityGap)
                optimalityGap = boundControl.GetAbsoluteOptimalityGap();
            else
                optimalityGap = boundControl.GetRelativeOptimalityGap();

            if (isUseAbsOptimalityGap)
            {
                if (absDualityGap <= optimalityGap)
                {
                    return true;
                }
            }
            else
            {
                if (relativeDualityGap <= optimalityGap)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
