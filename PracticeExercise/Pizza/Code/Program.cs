using GlobalUtils;
using HashCode.Common;
using System;
using System.Collections.Generic;
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
            InputFile selectedFile = Utils.SelectInputFile(files); 

            First.GenerateFileWithAllSlices(selectedFile.FullPath, Path.Combine(rootPath, "Temp"));

            Utils.EndProgram();
        }
    }
}
