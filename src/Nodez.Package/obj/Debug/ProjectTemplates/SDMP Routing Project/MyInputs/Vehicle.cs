﻿using Nodez.Data;
using Nodez.Data.Interface;
using Nodez.Sdmp.Routing.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace $safeprojectname$.MyInputs
{
    public class Vehicle : IInputRow, IVehicleData
    {
        // Define columns here (NOTICE: The column name defined here and the column name defined in the data file must match.)

        public string ID { get; private set; }

        public string NAME { get; private set; }

        public double SPEED { get; private set; }


        public Vehicle()
        {
            // Define keys here (You can search data with the key defined here. Allow multiple keys)

            HashSet<string> key = new HashSet<string>();
            key.Add("ID");

            this.KeyMappings.Add(1, key);
        }
    }
}