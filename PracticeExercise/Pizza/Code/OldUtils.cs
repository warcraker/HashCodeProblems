using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HashCode.Common.Utils;

namespace HashPizza
{
    public class OldUtils
    {
        static OldUtils()
        {
            _globalTimer = Stopwatch.StartNew();
        }

        static Stopwatch _globalTimer;
        static Stopwatch _sectionTimer = new Stopwatch();
        static string _currentSection;

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
