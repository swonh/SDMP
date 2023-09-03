using Nodez.Data.Controls;
using Nodez.Data.DataModel;
using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace Nodez.Data.Managers
{
    public class OutputManager
    {
        private static readonly Lazy<OutputManager> lazy = new Lazy<OutputManager>(() => new OutputManager());

        public static OutputManager Instance { get { return lazy.Value; } }

        public static Dictionary<string, OutputTable> Outputs { get { return _outputs; } }

        private static Dictionary<string, string> _pathOutputsMappings;

        private static Dictionary<string, OutputTable> _outputs;

        public void DeleteOutputFiles()
        {
            string projectName = Assembly.GetCallingAssembly().GetName().Name;
            string outputPath = string.Format(@"..\..\..\{0}\Output", projectName);

            if (Directory.Exists(outputPath) == false)
                return;

            DirectoryInfo dinfo = new DirectoryInfo(outputPath);
            FileInfo[] dfiles = dinfo.GetFiles();

            foreach (FileInfo dfile in dfiles)
            {
                dfile.Delete();
            }
        }

        public void SetPathOutputsMappings(string key, string path)
        {
            if (_pathOutputsMappings.ContainsKey(key) == false)
                _pathOutputsMappings.Add(key, path);
            else
                _pathOutputsMappings[key] = path;
        }

        public void SetPathOutputsMappings(Dictionary<string, string> pathMappings)
        {
            _pathOutputsMappings = pathMappings;
        }

        public void ClearOutputs()
        {
            _outputs.Clear();

            if (_pathOutputsMappings != null)
                _pathOutputsMappings.Clear();
        }


        public OutputTable GetOutput(string key)
        {
            OutputTable output;

            if (_outputs.TryGetValue(key, out output))
                return output;

            return null;
        }

        public void SetOutput(string key, OutputTable output)
        {
            if (_outputs == null)
                _outputs = new Dictionary<string, OutputTable>();

            if (_outputs.ContainsKey(key) == false)
                _outputs.Add(key, output);
            else
                _outputs[key] = output;

        }

    }
}
