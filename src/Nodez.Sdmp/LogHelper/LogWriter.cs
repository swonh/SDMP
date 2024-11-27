// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.LogHelper
{
    public static class LogWriter
    {
        private static TextWriter _fileWriter;

        public static void SetFileWriter(TextWriter fileWriter) 
        {
            _fileWriter = fileWriter;
        }

        public static void Write(string value) 
        {
            Console.Write(value);
            _fileWriter.Write(value);
        }

        public static void WriteLine(string value)
        {
            Console.WriteLine(value);
            _fileWriter.WriteLine(value);
        }

        public static void WriteLine(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
            _fileWriter.WriteLine(string.Format(Console.Out.FormatProvider, format, arg));
        }

        public static void WriteLine()
        {
            Console.WriteLine();
            _fileWriter.WriteLine();
        }

        public static void WriteConsoleOnly(string value)
        {
            Console.Write(value);
        }

        public static void WriteLineConsoleOnly(string value)
        {
            Console.WriteLine(value);
        }

        public static void WriteLineConsoleOnly()
        {
            Console.WriteLine();
        }

        public static void WriteLineConsoleOnly(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
    }
}
