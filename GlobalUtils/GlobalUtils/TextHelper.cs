using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalUtils
{
    public static class TextHelper
    {
        private static Queue<string> linesToRead = new Queue<string>();

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
        public static void EnqueueLinesToRead(IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                linesToRead.Enqueue(line);
            }
        }
    }
}
