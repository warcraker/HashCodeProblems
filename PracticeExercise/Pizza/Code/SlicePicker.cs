using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza
{
    class SlicePicker
    {
        static void Main(string[] args)
        {
            string path;

            {
                List<string> files = Directory.EnumerateFiles(Path.Combine(args[0], "output")).ToList();

                for (int i = 0; i < files.Count; i++)
                {
                    Console.WriteLine($"[{i}] {files[i]}");
                }

                string selectedIndexAsText = Console.ReadLine();
                int selectedIndex = int.Parse(selectedIndexAsText);

                string file = files[selectedIndex];
                path = Path.Combine(args[0], file);
            }

            Console.WriteLine(path);

            Console.ReadLine();
        }
    }
}
