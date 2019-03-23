using HashCode.Common;
using HashCode.Common.Models;
using System.IO;

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
