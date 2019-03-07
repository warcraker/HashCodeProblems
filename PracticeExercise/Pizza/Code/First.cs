using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static HashPizza.Utils;

namespace HashPizza
{
    class First
    {
        // 'T' => true Tomato
        // 'M' => false Mushroom

        static void Main(string[] args)
        {
            BeginSection("File load");
            Pizza P = ReadInputFile(args[0]);           

            if (BeginConditionalSection("Print file loaded"))
            {
                Console.WriteLine($"R:{P.R}; C:{P.C}; L:{P.L}; H:{P.H}");
                PrintSlice(P.WholePizza, 0);
            }

            BeginSection("Valid slices creation");
            bool[][][] validSlices; // [SliceNumber][row][col]
            {
                List<bool[][]> validSlicesList = new List<bool[][]>();
                short minimumSliceArea = (short)(P.L * 2);
                for (short h = 1; h <= P.R; h++)
                {
                    for (short w = 1; w <= P.C; w++)
                    {
                        if ((h * w) < minimumSliceArea) continue;

                        if ((h * w) > P.H) continue;

                        validSlicesList.AddRange(GenerateSlices(h, w, P.L));
                    }
                }

                validSlices = validSlicesList.ToArray();
                Console.WriteLine($"Valid slices = {validSlices.Length}");
            }

            BeginSection("Legal slices");
            int[][][] legalSlices; // [row][col][SliceNumbers]
            {
                legalSlices = new int[P.R][][];

                for (int row = 0; row < P.R; row++)
                {
                    legalSlices[row] = new int[P.C][];
                }

                for (int pRow = 0; pRow < P.R; pRow++)
                {
                    Console.WriteLine($"Row #{pRow}");
                    for (int pCol = 0; pCol < P.C; pCol++)
                    {
                        List<int> cellValidPermutations = new List<int>();
                        for (int sliceNumber = 0; sliceNumber < validSlices.Length; sliceNumber++)
                        {
                            bool[][] slice = validSlices[sliceNumber];

                            int height = slice.Length;
                            if (height + pRow > P.R) continue;

                            int width = slice[0].Length;
                            if (width + pCol > P.C) continue;

                            bool isValid = true;

                            for (int sRow = 0; sRow < height; sRow++)
                            {
                                for (int sCol = 0; sCol < width; sCol++)
                                {
                                    if (slice[sRow][sCol] != P[pRow + sRow][pCol + sCol])
                                    {
                                        isValid = false;
                                        break;
                                    }

                                }
                                if (!isValid) break;
                            }

                            if (isValid)
                            {
                                cellValidPermutations.Add(sliceNumber);
                            }
                        }
                        legalSlices[pRow][pCol] = cellValidPermutations.ToArray();
                    }
                }
            }

            if(BeginConditionalSection("Print valid slice permutations"))
            {
                for (int row = 0; row < P.R; row++)
                {
                    for (int col = 0; col < P.C; col++)
                    {
                        Console.WriteLine($"Cell [{row},{col}]");
                        foreach (int sliceNumber in legalSlices[row][col])
                        {
                            PrintSlice(validSlices[sliceNumber], sliceNumber);
                        }
                    }
                }
            }

            if(BeginConditionalSection("Save all slices to file"))
            {
                string outPath = Path.Combine(args[0], "output");
                Directory.CreateDirectory(outPath);
                DateTime now = DateTime.Now;
                outPath = Path.Combine(outPath, $"{now.Month}_{now.Day}_{now.Hour}.{now.Minute}.{now.Second}.txt");
                Console.WriteLine($"Output file: {outPath}");
                using (StreamWriter stream = File.CreateText(outPath))
                {
                    stream.NewLine = "\n";
                    for (int row = 0; row < P.R; row++)
                    {
                        Console.WriteLine($"Row #{row}");
                        for (short col = 0; col < P.C; col++)
                        {
                            int[] sliceNumbers = legalSlices[row][col];
                            for (short sliceIndex = 0; sliceIndex < sliceNumbers.Length; sliceIndex++)
                            {
                                int sliceNumber = sliceNumbers[sliceIndex];
                                bool[][] slice = validSlices[sliceNumber];
                                string line = $"{row} {col} {row + slice.Length - 1} {col + slice[0].Length - 1}";
                                stream.WriteLine(line);
                            }
                        }
                    }
                }
            }

            EndProgram();
        }

        static IEnumerable<bool[][]> GenerateSlices(short height, short width, short minIngredients)
        {
            int seed = 1;
            seed = seed << (height * width);
            int initialMask = seed >> 1;
            seed--;

            do
            {
                ushort mushrooms = 0;
                ushort tomatoes = 0;
                bool[][] slice = new bool[height][];
                for (int row = 0; row < height; row++)
                {
                    slice[row] = new bool[width];
                }

                int mask = initialMask;
                for (int row = 0; row < height; row++)
                {
                    for (int col = 0; col < width; col++)
                    {
                        if ((seed & mask) == mask)
                        {
                            slice[row][col] = true;
                            tomatoes++;
                        }
                        else
                        {
                            mushrooms++;
                        }
                        mask = mask >> 1;
                    }
                }

                if (mushrooms >= minIngredients && tomatoes >= minIngredients)
                {
                    yield return slice;
                }

                seed--;
            } while (seed >= 0);
        }
        static void PrintSlice(bool[][] slice, int sliceNumber)
        {
            Console.WriteLine($"Slice #{sliceNumber}");
            for (int r = 0; r < slice.Length; r++)
            {
                bool[] row = slice[r];
                for (int c = 0; c < row.Length; c++)
                {
                    Console.Write(row[c] ? 'T' : 'M');
                }
                Console.WriteLine();
            }
        }
    }
}
