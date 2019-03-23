using GlobalUtils;
using GlobalUtils.Models;
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
            string rootPath = FileHelper.GetAppRootFolder();
            ProblemFiles files = new ProblemFiles(rootPath);
            InputFile selectedFile = FileHelper.SelectInputFile(files); 

            First.GenerateFileWithAllSlices(selectedFile.FullPath, Path.Combine(rootPath, "Temp"));

            Utils.EndProgram();
        }
    }
}
