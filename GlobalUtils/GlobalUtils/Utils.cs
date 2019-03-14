using GlobalUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace HashCode.Common
{
    public class Utils
    {
        private static string _currentSection;
        private static Stopwatch _globalTimer;
        private static Stopwatch _sectionTimer = new Stopwatch();
        private static Queue<string> _linesToRead = new Queue<string>();

        static Utils()
        {
            _globalTimer = Stopwatch.StartNew();
        }

        public static void Init()
        {
            ;
        }

        public static void BeginSection(string section)
        {
            PrintPreviousSectionEnd();

            SetColors(ConsoleColor.DarkGreen, ConsoleColor.Black);
            Console.WriteLine($"BEGIN {section}");
            ResetColors();

            _currentSection = section;
            _sectionTimer.Restart();
        }
        public static bool BeginConditionalSection(string section)
        {
            PrintPreviousSectionEnd();

            _globalTimer.Stop();
            _sectionTimer.Stop();
            SetColors(ConsoleColor.DarkRed, ConsoleColor.Gray);
            Console.WriteLine("TIMERS PAUSED");
            SetColors(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine($"{section}? [y/n]");
            ResetColors();

            bool execute = Console.ReadKey().KeyChar == 'y';
            Console.WriteLine();

            if (execute)
            {
                SetColors(ConsoleColor.DarkGreen, ConsoleColor.Black);
                Console.WriteLine($"BEGIN {section}");
                _sectionTimer.Restart();
            }
            else
            {
                SetColors(ConsoleColor.Red, ConsoleColor.Black);
                Console.WriteLine($"ABORT {section}");
            }
            ResetColors();
            _currentSection = section;
            _globalTimer.Start();
            return execute;
        }
        public static void EndProgram()
        {
            PrintPreviousSectionEnd();

            Console.WriteLine("--- FINISHED ---");
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
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

        public static string ReadLine()
        {
            if (_linesToRead.Any())
            {
                string line = _linesToRead.Dequeue();
                Console.WriteLine($"AUTO: {line}");
                return line;
            }
            else
            {
                Console.Write("WRITE: ");
                return Console.ReadLine();
            }
        }
        public static void SetLinesToRead(IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                _linesToRead.Enqueue(line);
            }
        }

        public static void ResetColors()
        {
            Console.ResetColor();
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

                string line = ReadLine();

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

        private static void PrintPreviousSectionEnd()
        {
            SetColors(ConsoleColor.Green, ConsoleColor.Black);
            Console.WriteLine($"[{_globalTimer.Elapsed}] FINISHED {_currentSection}");
            Console.WriteLine($"TIME: {_sectionTimer.Elapsed}");
            Console.WriteLine();
            ResetColors();
        }
        private static void SetColors(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }
    }
}
