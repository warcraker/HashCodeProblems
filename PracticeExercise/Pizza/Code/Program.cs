using GlobalUtils;
using HashCode.Common;
using HashCode.Common.Models;
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
            InputFile inputFile = FileHelper.SelectInputFile(files);

            BenchmarkHelper.BeginSection("Generate intermediate file");
            string temporalFilePath = First.GenerateFileWithAllSlices(inputFile.FullPath, Path.Combine(rootPath, "Temp"));
            Console.WriteLine("..."); Console.ReadLine();

            BenchmarkHelper.BeginSection("Generate solution");
            string greedySolutionFilePath = Greedy.GenerateGreedySolution(inputFile, temporalFilePath, Path.Combine(rootPath, "Out"));
            Console.WriteLine("..."); Console.ReadLine();

            BenchmarkHelper.BeginSection("Validate solution");
            IEnumerable<string> errors = Tester.GetOutputErrors(inputFile.FullPath, greedySolutionFilePath);
            Console.WriteLine($"File has errors: {errors.Any()}");
            foreach (string error in errors)
            {
                Console.WriteLine(error);
            }
            Console.WriteLine("..."); Console.ReadLine();

            BenchmarkHelper.BeginSection("Count points");
            int points = Tester.GetPoints(greedySolutionFilePath);
            Console.WriteLine($"Total points: {points}");

            Utils.EndProgram();
        }
    }
}
