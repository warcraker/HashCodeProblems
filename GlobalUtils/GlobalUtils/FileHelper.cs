using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalUtils.Models;

namespace HashCode.Common
{
    public static class FileHelper
    {
        public static string GetAppRootFolder()
        {
            string executingPath = System.Reflection.Assembly.GetCallingAssembly().Location;
            DirectoryInfo executingPathInfo = new DirectoryInfo(executingPath);
            DirectoryInfo problemRootPathInfo = executingPathInfo.Parent.Parent.Parent.Parent;

            return problemRootPathInfo.FullName;
        }
        public static IEnumerable<string> GetFileLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
        public static InputFile SelectInputFile(ProblemFiles files)
        {
            InputFile[] inputFiles = files.InputFiles.ToArray();
            int selectedOption = SelectOption(inputFiles.Select(f => f.FileName).ToArray());

            return inputFiles[selectedOption];
        }

        private static int SelectOption(string[] options)
        {
            int value;
            bool numberIsValid = false;

            do
            {
                Console.WriteLine();
                Console.WriteLine("Select an option:");
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"\t[{i}] {options[i]}");
                }
                Console.WriteLine("Write desired option and press [Enter]...");

                string line = Console.ReadLine(); // TODO use TextDisplay Utils version > ReadLine();

                bool inputIsNumeric = int.TryParse(line, out value);
                if (!inputIsNumeric)
                {
                    numberIsValid = false;
                }
                else
                {
                    numberIsValid = (0 <= value && value < options.Length);
                }

                if (!numberIsValid)
                {
                    Console.WriteLine("Invalid option.");
                }
            } while (!numberIsValid);

            return value;
        }
    }
}
