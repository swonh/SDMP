// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Text;

namespace Nodez.Sdmp
{
    public static class UtilityHelper
    {
        public static string IdentityNull = "-";

        public static T StringToEnum<T>(this string src, T defValue)
        {
            if (string.IsNullOrEmpty(src))
                return defValue;

            foreach (string en in System.Enum.GetNames(typeof(T)))
            {
                if (en.Equals(src, StringComparison.CurrentCultureIgnoreCase))
                {
                    defValue = (T)System.Enum.Parse(typeof(T), src, true);
                    return defValue;
                }
            }

            return defValue;
        }

        public static string IsYN(bool isTrue)
        {
            if (isTrue)
                return Nodez.Sdmp.Constants.Constants.Y;

            return Nodez.Sdmp.Constants.Constants.N;
        }

        public static bool StringToBoolean(string str)
        {
            Boolean.TryParse(str, out bool result);

            return result;
        }

        public static float IntToFloat(int value)
        {
            return (float)value;
        }

        public static float DoubleToFloat(double value)
        {
            return (float)value;
        }

        public static double CalculateEuclideanDistance(double x1, double y1, double x2, double y2)
        {
            double dx = x1 - x2;
            double dy = y1 - y2;

            double dist = Math.Sqrt(dx * dx + dy * dy);

            return dist;
        }

        public static double CalculateManhattanDistance(double x1, double y1, double x2, double y2)
        {
            double dx = Math.Abs(x1 - x2);
            double dy = Math.Abs(y1 - y2);

            double dist = dx + dy;

            return dist;
        }

        public static string CreateKey(string[] keys)
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
    }
}
