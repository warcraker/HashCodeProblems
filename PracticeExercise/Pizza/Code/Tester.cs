using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    class Tester
    {
        static void Main(string[] args)
        {
            string inputPath = Path.Combine(args[0], "input");




            string outputPath = Path.Combine(args[0], "output");
            DirectoryInfo dir = new DirectoryInfo(outputPath);
            List<FileInfo> files = dir.EnumerateFiles().ToList();

            throw new NotImplementedException("=)");
        }
    }
}
