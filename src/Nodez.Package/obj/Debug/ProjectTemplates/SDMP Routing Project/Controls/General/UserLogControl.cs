// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Constants;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Routing.DataModel;
using Nodez.Sdmp.Routing.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Controls
{
    public class UserLogControl : LogControl
    {
        private static readonly Lazy<UserLogControl> lazy = new Lazy<UserLogControl>(() => new UserLogControl());

        public static new UserLogControl Instance { get { return lazy.Value; } }

        public override void WriteOptimalLog()
        {
            SolutionManager solutionManager = SolutionManager.Instance;
            Solution optSol = solutionManager.OptimalSolution;

            Console.WriteLine(Constants.LINE);
            Console.WriteLine(string.Format("Optimal Objective Value: {0}", optSol.Value));
            this.WriteSolution(optSol);
            Console.WriteLine(Constants.LINE);
        }

        public override void WriteBestSolutionLog()
        {
            SolutionManager solutionManager = SolutionManager.Instance;
            Solution bestSol = solutionManager.BestSolution;

            Console.WriteLine(Constants.LINE);
            Console.WriteLine(string.Format("Objective Value: {0}", bestSol.Value));
            this.WriteSolution(bestSol);
            Console.WriteLine(Constants.LINE);
        }

        public override void WriteSolution(Solution solution)
        {
            // Default Logic
            RoutingDataManager manager = RoutingDataManager.Instance;

            IOrderedEnumerable<KeyValuePair<int, State>> states = solution.States.OrderBy(x => x.Key);

            Dictionary<int, List<int>> vehicleRoutes = new Dictionary<int, List<int>>();
            foreach (KeyValuePair<int, State> item in states)
            {
                RoutingState state = item.Value as RoutingState;

                int vehicleIndex = state.CurrentVehicleIndex;
                int customerIndex = state.CurrentCustomerIndex;

                if (vehicleRoutes.TryGetValue(vehicleIndex, out List<int> route) == false)
                {
                    vehicleRoutes.Add(vehicleIndex, new List<int>() { customerIndex });
                }
                else
                    vehicleRoutes[vehicleIndex].Add(customerIndex);
            }

            double totalLoad = 0;
            double totalDistance = 0;

            foreach (Vehicle vehicle in manager.RoutingProblem.Vehicles)
            {
                Console.WriteLine(string.Format("[Route for {0}]", vehicle.Name));
                Console.WriteLine("Routing Sequence: ");

                StringBuilder routingStr = new StringBuilder();
                Depot depot = RoutingDataManager.Instance.RoutingProblem.Depot;

                double vehicleLoad = 0;
                double vehicleDistance = 0;
                double vehicleAvailableTime = 0;

                if (vehicleRoutes.TryGetValue(vehicle.Index, out List<int> value))
                {
                    routingStr.AppendFormat("{0}->", depot.Name);
                    Customer prevCust = depot;

                    for (int idx = 0; idx < value.Count; idx++)
                    {
                        int c = value.ElementAt(idx);

                        Customer customer = manager.GetCustomer(c);

                        if (depot.Index == c)
                            customer = depot;

                        double qty = 0;
                        string workType = string.Empty;

                        qty = customer.Demand == null ? 0 : customer.Demand.Quantity;
                        workType = customer.IsDelivery ? "Delivery" : "Pickup";

                        double dist = manager.GetDistance(prevCust.Index, customer.Index);
                        double time = manager.GetTime(prevCust.Index, customer.Index);

                        totalLoad += qty;
                        totalDistance += dist;

                        vehicleLoad += qty;
                        vehicleDistance += dist;
                        vehicleAvailableTime += time;

                        if (idx == value.Count - 1)
                        {
                            routingStr.AppendFormat("{0} {1}({2}) | AvailTime:{3}", customer.Name, workType, qty, vehicleAvailableTime);
                            break;
                        }

                        routingStr.AppendFormat("{0} {1}({2})| AvailTime:{3} ->", customer.Name, workType, qty, vehicleAvailableTime);

                        prevCust = customer;
                    }
                }

                Console.WriteLine(routingStr.ToString());
                Console.WriteLine("Distance: {0}", vehicleDistance);
                Console.WriteLine("Load: {0}\n", vehicleLoad);
            }

            Console.WriteLine("Total distance of all routes {0}", totalDistance);
            Console.WriteLine("Total load of all routes {0}", totalLoad);
        }

        public override void WritePruneLog(State state)
        {
            //Console.WriteLine("Prune => StateIndex:{0}, State:{1}, Stage:{2}, DualBound:{3}, BestValue:{4}, BestPrimalBound:{5}", state.Index, state.ToString(), state.Stage.Index, state.DualBound, state.BestValue, BoundManager.Instance.BestPrimalBound);
        }
    }
}
