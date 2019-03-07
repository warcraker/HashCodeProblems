using GlobalUtils;
using HashCode.Common;
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
            ProblemFiles files = new ProblemFiles(Utils.GetAppRootFolder());

            foreach (InputFile file in files.InputFiles)
            {
                Console.WriteLine($"{file.FileName} @ {file.FullPath}");
            }

            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }

        public static bool ValidateResult(string inputFilePath, string outputFilePath, out IList<string> errors)
        {
            errors = new List<string>();

            string[] inputLines = Utils.GetFileLines(inputFilePath);
            string[] outputLines = Utils.GetFileLines(outputFilePath);

            string[] inputFirstLineSplited = inputLines[0].Split(' ');
            int r = int.Parse(inputFirstLineSplited[0]);
            int c = int.Parse(inputFirstLineSplited[1]);

            int s;
            bool isFirstLineValidNumber = int.TryParse(outputLines[0], out s);

            if (!isFirstLineValidNumber)
            {
                errors.Add("First line must be one line containing a single natural number ​S (0 ≤ S ≤ RxC), representing the total number of slices to be cut");
            }
            else if (s < 0)
            {
                errors.Add("S cannot be a negative number");
            }
            else if (s > r * c)
            {
                errors.Add("S cannot be greater than R x C");
            }
            else if (s != outputLines.Length - 1)
            {
                errors.Add($"S must be the number of slices. S = {s}. Number of slices = {outputLines.Length - 1}");
            }

            bool[][] cellsInSlice = Utils.InitializeDefault2DVector<bool>(r, c);

            for (int lineNumber = 1; lineNumber < outputLines.Length; lineNumber++)
            {
                string line = outputLines[lineNumber];
                string[] lineSplitted = line.Split(' ');

                if (lineSplitted.Length != 4)
                {
                    errors.Add($"Line #{lineNumber} must be 4 numbers separated by one space. Text: '{line}'");
                }
                else
                {
                    int r1, c1, r2, c2;
                    bool r1Ok = int.TryParse(lineSplitted[0], out r1);
                    bool c1Ok = int.TryParse(lineSplitted[1], out c1);
                    bool r2Ok = int.TryParse(lineSplitted[2], out r2);
                    bool c2Ok = int.TryParse(lineSplitted[3], out c2);

                    if (!(r1Ok && c1Ok && r2Ok && c2Ok))
                    {
                        errors.Add($"Line #{lineNumber} have an invalid format. Must be 'R1 C1 R2 C2'. Text: '{line}'");
                    }
                    else
                    {
                        bool coordinateOutsideOfRange = false;

                        if (!(0 <= r1 && r1 < r))
                        {
                            errors.Add($"Line #{lineNumber}. R1 is outside of range (0 <= R1 < R). R1 = {r1}. R = {r}");
                            coordinateOutsideOfRange = true;
                        }
                        if (!(0 <= c1 && c1 < c))
                        {
                            errors.Add($"Line #{lineNumber}. C1 is outside of range (0 <= C1 < C). C1 = {c1}. C = {c}");
                            coordinateOutsideOfRange = true;
                        }
                        if (!(0 <= r2 && r2 < r))
                        {
                            errors.Add($"Line #{lineNumber}. R2 is outside of range (0 <= R2 < R). R2 = {r2}. R = {r}");
                            coordinateOutsideOfRange = true;
                        }
                        if (!(0 <= c2 && c2 < c))
                        {
                            errors.Add($"Line #{lineNumber}. C2 is outside of range (0 <= C2 < C). C2 = {c2}. C = {c}");
                            coordinateOutsideOfRange = true;
                        }

                        if (!coordinateOutsideOfRange)
                        {
                            bool sliceOverlaps = false;

                            
                        }
                    }
                }
            }

            return errors.Count == 0;
        }
    }
}
