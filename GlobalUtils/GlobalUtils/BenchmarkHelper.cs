using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode.Common
{
    public static class BenchmarkHelper
    {
        private static string _currentSection;
        private static Stopwatch _globalTimer;
        private static Stopwatch _sectionTimer = new Stopwatch();

        static BenchmarkHelper()
        {
            _globalTimer = Stopwatch.StartNew();
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

        private static void PrintPreviousSectionEnd()
        {
            SetColors(ConsoleColor.Green, ConsoleColor.Black);
            Console.WriteLine($"[{_globalTimer.Elapsed}] FINISHED {_currentSection}");
            Console.WriteLine($"TIME: {_sectionTimer.Elapsed}");
            Console.WriteLine();
            ResetColors();
        }
        private static void ResetColors()
        {
            Console.ResetColor();
        }
        private static void SetColors(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }
    }
}
