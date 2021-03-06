﻿using GlobalUtils;
using HashCode.Common;
using HashPizza.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class Tester
    {
        public static IEnumerable<string> GetOutputErrors(string inputFilePath, string outputFilePath)
        {
            Pizza P = new Pizza(inputFilePath);

            string[] outputLines = FileHelper.GetFileLines(outputFilePath).ToArray();
            int S;

            if (!int.TryParse(outputLines[0], out S))
            {
                yield return "First line must be one line containing a single natural number ​S (0 ≤ S ≤ RxC), representing the total number of slices to be cut";
            }
            else if (S < 0)
            {
                yield return "S (First line) cannot be a negative number";
            }
            else if (S > P.R * P.C)
            {
                yield return "S (First line) cannot be greater than R x C";
            }
            else if (S != outputLines.Length - 1)
            {
                yield return $"S (First line) must be the number of slices. S = {S}. Number of slices = {outputLines.Length - 1}";
            }

            bool[][] cellsInSlice = Utils.InitializeDefault2DVector<bool>(P.R, P.C);

            for (int lineNumber = 1; lineNumber < outputLines.Length; lineNumber++)
            {
                string line = outputLines[lineNumber];
                string[] lineSplitted = line.Split(' ');

                if (lineSplitted.Length != 4)
                {
                    yield return $"Slice #{lineNumber} must be 4 numbers separated by one space. Text: '{line}'";
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
                        yield return $"Slice #{lineNumber} have an invalid format. Must be 'R1 C1 R2 C2'. Text: '{line}'";
                    }
                    else
                    {
                        bool coordinateOutsideOfRange = false;
                        if (!(0 <= r1 && r1 < P.R))
                        {
                            yield return $"Slice #{lineNumber}. R1 is outside of range (0 <= R1 < R). R1 = {r1}. R = {P.R}";
                            coordinateOutsideOfRange = true;
                        }
                        if (!(0 <= c1 && c1 < P.C))
                        {
                            yield return $"Slice #{lineNumber}. C1 is outside of range (0 <= C1 < C). C1 = {c1}. C = {P.C}";
                            coordinateOutsideOfRange = true;
                        }
                        if (!(0 <= r2 && r2 < P.R))
                        {
                            yield return $"Slice #{lineNumber}. R2 is outside of range (0 <= R2 < R). R2 = {r2}. R = {P.R}";
                            coordinateOutsideOfRange = true;
                        }
                        if (!(0 <= c2 && c2 < P.C))
                        {
                            yield return $"Slice #{lineNumber}. C2 is outside of range (0 <= C2 < C). C2 = {c2}. C = {P.C}";
                            coordinateOutsideOfRange = true;
                        }

                        if (!coordinateOutsideOfRange)
                        {
                            int rIni = Math.Min(r1, r2);
                            int rEnd = Math.Max(r1, r2);
                            int cIni = Math.Min(c1, c2);
                            int cEnd = Math.Max(c1, c2);

                            int size = (rEnd - rIni + 1) * (cEnd - cIni + 1);
                            if (size > P.H)
                            {
                                yield return $"Slice #{lineNumber} is bigger than H. Size = {size}. Allowed (H) = {P.H}";
                            }

                            bool sliceOverlaps = false;
                            int tomatoes = 0, mushrooms = 0;
                            for (int row = rIni; row <= rEnd; row++)
                            {
                                for (int col = cIni; col <= cEnd; col++)
                                {
                                    if (cellsInSlice[row][col] == true)
                                    {
                                        sliceOverlaps = true;
                                    }
                                    cellsInSlice[row][col] = true;

                                    if (P[row][col] == true)
                                    {
                                        tomatoes++;
                                    }
                                    else
                                    {
                                        mushrooms++;
                                    }
                                }
                            }
                            if (sliceOverlaps)
                            {
                                yield return $"Line #{lineNumber} overlaps. Text: '{line}'";
                            }
                            if (tomatoes < P.L || mushrooms < P.L)
                            {
                                yield return $"Line #{lineNumber} does not have enough ingredients. Tomatoes = {tomatoes}. Mushrooms = {mushrooms}. L = {P.L}";
                            }
                        }
                    }
                }
            }
        }
        public static int GetPoints(string outputFilePath)
        {
            int points = 0;
            string[] lines = FileHelper.GetFileLines(outputFilePath).Skip(1).ToArray();

            foreach (string line in lines)
            {
                string[] lineSplitted = line.Split(' ');

                int r1 = int.Parse(lineSplitted[0]);
                int c1 = int.Parse(lineSplitted[1]);
                int r2 = int.Parse(lineSplitted[2]);
                int c2 = int.Parse(lineSplitted[3]);

                int r = Math.Abs(r1 - r2) + 1;
                int c = Math.Abs(c1 - c2) + 1;

                points += r * c;
            }

            return points;
        }
    }
}
