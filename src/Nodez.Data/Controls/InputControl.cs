﻿using Nodez.Data.DataModel;
using Nodez.Data.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nodez.Data.Controls
{
    public class InputControl
    {
        private static readonly Lazy<InputControl> lazy = new Lazy<InputControl>(() => new InputControl());

        public static InputControl Instance { get { return lazy.Value; } }

        public void CopyDataFromSource(string sourcePath, string destinationPath, string extension, bool deleteExistingFiles = true, string fileIdentifier = null)
        {
            if (Directory.Exists(sourcePath) == false)
                return;

            if (Directory.Exists(destinationPath) == false)
                Directory.CreateDirectory(destinationPath);

            if (deleteExistingFiles)
            {
                DirectoryInfo dinfo = new DirectoryInfo(destinationPath);
                FileInfo[] dfiles = dinfo.GetFiles();

                foreach (FileInfo dfile in dfiles)
                {
                    dfile.Delete();
                }
            }

            DirectoryInfo info = new DirectoryInfo(sourcePath);

            FileInfo[] files = info.GetFiles();

            foreach (FileInfo file in files)
            {
                if (file.Extension.Contains(extension) == false)
                    continue;

                string name = string.Format(@"{0}\{1}", destinationPath, file.Name);
                if (fileIdentifier != null)
                    name = string.Format(@"{0}\{1}", destinationPath, string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(file.Name), fileIdentifier, file.Extension));

                file.CopyTo(name, true);
            }
        }

        public Dictionary<string, string> GetInputsPathMappings(string inputPath, List<string> tableNames) 
        {
            if(inputPath == null)
                inputPath = @"..\..\InputData";

            Dictionary<string, string> mappings = new Dictionary<string, string>();

            foreach (string tableName in tableNames)
            {
                mappings.Add(tableName, string.Format(@"{0}\{1}.csv", inputPath, tableName));              
            }

            return mappings;
        }

        public List<string> GetInputFileNames(string inputPath)
        {
            List<string> fileNames = new List<string>();
            if (Directory.Exists(inputPath))
            {
                DirectoryInfo info = new DirectoryInfo(inputPath);

                FileInfo[] files = info.GetFiles();

                foreach (FileInfo file in files)
                    fileNames.Add(Path.GetFileNameWithoutExtension(file.Name));
            }

            return fileNames;
        }

        public List<string> GetInputFileNames()
        {
            string inputPath = @"..\..\InputData";
            List<string> fileNames = new List<string>();
            if (Directory.Exists(inputPath))
            {
                DirectoryInfo info = new DirectoryInfo(inputPath);

                FileInfo[] files = info.GetFiles();

                foreach (FileInfo file in files)
                    fileNames.Add(Path.GetFileNameWithoutExtension(file.Name));
            }

            return fileNames;
        }

        public virtual Dictionary<string, string> GetColumnNameMappings(InputTable table)
        {
            Dictionary<string, string> mappings = new Dictionary<string, string>();

            return mappings;
        }

        public virtual IInputRow PersistInputData(IInputRow inputRow) 
        {
            return inputRow;
        }

        public virtual string PersistInputDataLoad(string tableName, string columnName, int rowNum, string data) 
        {
            return data;
        }
    }
}