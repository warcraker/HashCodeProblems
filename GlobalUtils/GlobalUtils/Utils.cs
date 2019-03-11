using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HashCode.Common
{
    public class Utils
    {
        private static Queue<string> linesToRead = new Queue<string>();
        public static void SetLinesToRead(IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                linesToRead.Enqueue(line);
            }
        }

        public static string GetAppRootFolder()
        {
            string executingPath = System.Reflection.Assembly.GetCallingAssembly().Location;
            DirectoryInfo executingPathInfo = new DirectoryInfo(executingPath);
            DirectoryInfo problemRootPathInfo = executingPathInfo.Parent.Parent.Parent.Parent;

            return problemRootPathInfo.FullName;
        }
        public static string[] GetFileLines(string filePath)
        {
            return File.ReadAllLines(filePath).ToArray();
        }
        public static T[][] InitializeDefault2DVector<T>(int rows, int columns)
        {
            T[][] vector = new T[rows][];
            for (int row = 0; row < rows; row++)
            {
                vector[row] = new T[columns];
            }

            return vector;
        }
        public static int SelectOption(string[] options)
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
                Console.WriteLine("[C] Close application");
                Console.WriteLine("Write desired option and press [Enter]...");

                string line = ReadLine();
                if (line.Equals("c", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("Closing application.");
                    Console.WriteLine("Press enter...");
                    Console.ReadLine();

                    Environment.Exit(0);
                }

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
        public static string ReadLine()
        {
            if (linesToRead.Any())
            {
                string line = linesToRead.Dequeue();
                Console.WriteLine($"AUTO: {line}");
                return line;
            }
            else
            {
                Console.Write("WRITE: ");
                return Console.ReadLine();
            }
        }
    }
}
