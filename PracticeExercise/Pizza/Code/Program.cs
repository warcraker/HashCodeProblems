using GlobalUtils;
using HashCode.Common;
using System;
using System.IO;
using System.Linq;

namespace HashPizza
{
    class Program
    {
        static void Main(string[] args)
        {
            Utils.Init();

            string rootPath = Utils.GetAppRootFolder();
            ProblemFiles files = new ProblemFiles(rootPath);
            InputFile[] inputFiles = files.InputFiles.ToArray();
            int selectedFileIndex = Utils.SelectOption(inputFiles.Select(f => f.FileName).ToArray());
            string selectedFilePath = inputFiles[selectedFileIndex].FullPath;

            First.GenerateFileWithAllSlices(selectedFilePath, Path.Combine(rootPath, "Temp"));

            Utils.EndProgram();
        }
    }
}
