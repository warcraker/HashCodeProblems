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
            InputFile inputFile = Utils.SelectInputFile(files);

            Utils.BeginSection("Generate intermediate file");
            string temporalFilePath = First.GenerateFileWithAllSlices(inputFile.FullPath, Path.Combine(rootPath, "Temp"));
            Console.WriteLine("..."); Console.ReadLine();

            Utils.BeginSection("Generate solution");
            string greedySolutionFilePath = Greedy.GenerateGreedySolution(inputFile, temporalFilePath, Path.Combine(rootPath, "Out"));
            Console.WriteLine("..."); Console.ReadLine();

            Utils.BeginSection("Validate solution");
            IList<string> errors;
            bool isValid = Tester.ValidateOutput(inputFile.FullPath, greedySolutionFilePath, out errors);
            Console.WriteLine($"File is valid: {isValid}");
            foreach (string error in errors)
            {
                Console.WriteLine(error);
            }
            Console.WriteLine("..."); Console.ReadLine();

            Utils.BeginSection("Count points");
            int points = Tester.GetPoints(greedySolutionFilePath);
            Console.WriteLine($"Total points: {points}");

            Utils.EndProgram();
        }
    }
}
