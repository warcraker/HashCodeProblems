﻿using HashCode.Common;
using HashCode.Common.Models;
using HashPizza.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class Greedy
    {
        public static string GenerateGreedySolution(InputFile inputFile, string withAllSlicesFilePath, string outputFolderPath)
        {
            Pizza p = new Pizza(inputFile.FullPath);
            bool[][] usedCells = Utils.InitializeDefault2DVector<bool>(p.R, p.C);

            IEnumerable<Slice> allSlices = FileHelper.GetFileLines(inputFile.FullPath).Skip(1).Select(line =>
            {
                string[] lineSplitted = line.Split(' ');

                int r1 = int.Parse(lineSplitted[0]);
                int c1 = int.Parse(lineSplitted[1]);
                int r2 = int.Parse(lineSplitted[2]);
                int c2 = int.Parse(lineSplitted[3]);

                return new Slice(r1, c1, r2, c2);
            });
            IEnumerable<Slice> orderedSlices = allSlices.OrderBy(s => s.Area);
            List<Slice> usedSlices = new List<Slice>();

            foreach (Slice slice in orderedSlices)
            {
                if(!SliceOverlaps(usedCells, slice))
                {
                    usedSlices.Add(slice);
                    SetUsedCells(usedCells, slice);
                }
            }

            List<string> linesToWrite = new List<string>
            {
                usedSlices.Count.ToString()
            };
            linesToWrite.AddRange(usedSlices.Select(s => s.ToString()));

            string fileName = $"[{inputFile.FileName}][Greedy][{DateTime.Now:yyyy-MM-dd_HH-mm-ss}].txt";
            string fileFullPath = Path.Combine(outputFolderPath, fileName);
            File.WriteAllLines(fileFullPath, linesToWrite);

            return fileFullPath;
        }

        private static bool SliceOverlaps(bool[][] usedCells, Slice slice)
        {
            bool overlaps = false;
            for (int r = slice.R1; r < slice.R2; r++)
            {
                for (int c = slice.C1; r < slice.C2; r++)
                {
                    if (usedCells[r][c] == true)
                    {
                        overlaps = true;
                        break;
                    }
                }
                if (overlaps)
                {
                    break;
                }
            }

            return overlaps;
        }
        private static void SetUsedCells(bool[][] usedCells, Slice slice)
        {
            for (int r = slice.R1; r < slice.R2; r++)
            {
                for (int c = slice.C1; r < slice.C2; r++)
                {
                    usedCells[r][c] = true;
                }
            }
        }
    }
}
