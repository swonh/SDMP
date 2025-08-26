// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Routing.DataModel;
using Nodez.Sdmp.Routing.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nodez.Sdmp.Routing.Logic
{
    public class PrimAlgorithm
    {
        public HashSet<int> NodeSet { get; set; }

        public HashSet<int> PrimNodeSet { get; set; }

        public HashSet<int> NonPrimNodeSet { get; set; }

        public Dictionary<ValueTuple<int, int>, double> PrimEdgeSet { get; set; }

        public PrimAlgorithm(HashSet<int> nodeSet)
        {
            this.NodeSet = nodeSet;
            this.PrimNodeSet = new HashSet<int>();
            this.NonPrimNodeSet = nodeSet;
            this.PrimEdgeSet = new Dictionary<ValueTuple<int, int>, double>();
        }

        public PrimAlgorithm(HashSet<int> nodeSet, HashSet<int> primNodeSet, Dictionary<ValueTuple<int, int>, double> primEdgeSet)
        {
            this.NodeSet = nodeSet;
            this.PrimNodeSet = primNodeSet;
            this.NonPrimNodeSet = this.NodeSet.Except(this.PrimNodeSet).ToHashSet();
            this.PrimEdgeSet = primEdgeSet;
        }

        public PrimAlgorithm(RoutingState state)
        {
            RoutingDataManager manager = RoutingDataManager.Instance;

            List<RoutingState> states = new List<RoutingState>();
            states.Add(state);
            states.AddRange(state.GetBestStatesBackward().Cast<RoutingState>().ToList());

            this.NodeSet = new HashSet<int>();
            this.PrimNodeSet = new HashSet<int>();
            this.PrimEdgeSet = new Dictionary<ValueTuple<int, int>, double>();
            this.NonPrimNodeSet = new HashSet<int>();

            HashSet<int> visitedNodeSet = new HashSet<int>();

            foreach (RoutingState st in states)
            {
                foreach (var info in st.VehicleStateInfos)
                {
                    visitedNodeSet.Add(info.Value.CurrentNodeIndex);
                }
            }

            Dictionary<ValueTuple<int, int>, double> edges = new Dictionary<ValueTuple<int, int>, double>();
            for (int i = 0; i < visitedNodeSet.Count; i++)
            {
                if (i < visitedNodeSet.Count - 1)
                {
                    int from = visitedNodeSet.ElementAt(i);
                    int to = visitedNodeSet.ElementAt(i + 1);

                    double dist = manager.GetDistance(from, to);
                    edges.Add((from, to), dist);
                }
            }

            this.NodeSet.Add(manager.RoutingProblem.Depot.Index);

            foreach (Node c in manager.RoutingProblem.Nodes)
            {
                this.NodeSet.Add(c.Index);
            }

            this.PrimNodeSet = visitedNodeSet;
            this.PrimEdgeSet = edges;
            this.NonPrimNodeSet = this.NodeSet.Except(this.PrimNodeSet).ToHashSet();
        }

        public double GetMSTValue()
        {
            return this.PrimEdgeSet.Values.Sum();
        }

        public void Run()
        {
            if (this.NodeSet == null || this.NodeSet.Count == 0)
                return;

            DoSearch();
        }

        private void DoSearch()
        {
            RoutingDataManager manager = RoutingDataManager.Instance;

            while (this.PrimNodeSet.SetEquals(this.NodeSet) == false)
            {
                this.NonPrimNodeSet = this.NodeSet.Except(this.PrimNodeSet).ToHashSet();
                HashSet<int> candidateNodes = this.NonPrimNodeSet;

                double minDist = Double.MaxValue;
                int minFrom = 0;
                int minTo = 0;

                foreach (int fromNode in this.PrimNodeSet)
                {
                    foreach (int toNode in candidateNodes)
                    {
                        double dist = manager.GetDistance(fromNode, toNode);

                        if (minDist > dist)
                        {
                            minDist = dist;
                            minFrom = fromNode;
                            minTo = toNode;
                        }
                    }
                }

                this.PrimNodeSet.Add(minTo);
                this.PrimEdgeSet.Add((minFrom, minTo), minDist);
            }
        }
    }
}
