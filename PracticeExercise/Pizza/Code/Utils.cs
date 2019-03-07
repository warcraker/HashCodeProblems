using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class Utils
    {
        static Utils()
        {
            _globalTimer = Stopwatch.StartNew();
        }

        static Stopwatch _globalTimer;
        static Stopwatch _sectionTimer = new Stopwatch();
        static string _currentSection;

        public static Pizza ReadInputFile(string rootPath)
        {
            Console.WriteLine("[1] example.in");
            Console.WriteLine("[2] small.in");
            Console.WriteLine("[3] medium.in");
            Console.WriteLine("[4] big.in");

            string path = Path.Combine(rootPath, "input");
            string filename = "";
            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    filename = "example.in";
                    break;
                case '2':
                    filename = "small.in";
                    break;
                case '3':
                    filename = "medium.in";
                    break;
                case '4':
                    filename = "big.in";
                    break;
                default:
                    throw new Exception("Invalid option");
            }
            Console.WriteLine();
            path = Path.Combine(path, filename);

            string[] lines = File.ReadAllLines(path);
            string[] inputValues = lines[0].Split(' ');

            Pizza P = new Pizza(
                ParseNumber(inputValues[0]),
                ParseNumber(inputValues[1]),
                ParseNumber(inputValues[2]),
                ParseNumber(inputValues[3]));

            string[] pizzaLines = lines.Skip(1).ToArray();
            for (short row = 0; row < P.R; row++)
            {
                char[] rowChars = pizzaLines[row].ToCharArray();
                for (short col = 0; col < P.C; col++)
                {
                    P[row][col] = rowChars[col] == 'T';
                }
            }

            return P;
        }

        public static void PrintPreviousSectionEnd()
        {
            SetColors(ConsoleColor.Green, ConsoleColor.Black);
            Console.WriteLine($"[{_globalTimer.Elapsed}] FINISHED {_currentSection}");
            Console.WriteLine($"TIME: {_sectionTimer.Elapsed}");
            Console.WriteLine();
            ResetColors();
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
        public static void SetColors(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }
        public static void ResetColors()
        {
            Console.ResetColor();
        }
        public static void EndProgram()
        {
            Console.WriteLine($"[{_globalTimer.Elapsed}] FINISHED {_currentSection}");
            Console.WriteLine($"TIME: {_sectionTimer.Elapsed}");
            Console.WriteLine();

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("--- FINISHED ---");
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
        public static short ParseNumber(string text)
        {
            return short.Parse(text);
        }

    }
}
