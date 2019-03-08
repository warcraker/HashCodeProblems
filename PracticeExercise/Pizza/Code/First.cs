using HashCode.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static HashPizza.OldUtils; // TODO REMOVE
using static HashCode.Common.Utils;


namespace HashPizza
{
    class First
    {
        // 'T' => true Tomato
        // 'M' => false Mushroom

        static void Main(string[] args)
        {
            BeginSection("File load");
            Pizza P;
            {
                // TODO use ProblemFiles class
                Console.WriteLine("[1] example.in");
                Console.WriteLine("[2] small.in");
                Console.WriteLine("[3] medium.in");
                Console.WriteLine("[4] big.in");

                string path = Path.Combine(args[0], "input");
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
                P = ReadInputFile(path);
            }
#if DEBUG

            if (BeginConditionalSection("Print file loaded"))
            {
                Console.WriteLine($"R:{P.R}; C:{P.C}; L:{P.L}; H:{P.H}");
                PrintSlice(P.WholePizza, 0);
            }

#endif

            BeginSection("Valid slices creation");
            bool[][][] validSlices; // [SliceNumber][row][col]
            {
                List<bool[][]> validSlicesList = new List<bool[][]>();
                short minimumSliceArea = (short)(P.L * 2);
                for (short h = 1; h <= P.R; h++)
                {
                    for (short w = 1; w <= P.C; w++)
                    {
                        int sliceArea = h * w;

                        if (minimumSliceArea <= sliceArea && sliceArea <= P.H)
                        {
                            validSlicesList.AddRange(GenerateSlices(h, w, P.L));
                        }
                    }
                }

                validSlices = validSlicesList.ToArray();
                Console.WriteLine($"Valid slices = {validSlices.Length}");
            }

            BeginSection("Legal slices placement");
            int[][][] legalSlices; // [row][col][SliceNumbers]
            {
                legalSlices = new int[P.R][][];

                for (int row = 0; row < P.R; row++)
                {
                    legalSlices[row] = new int[P.C][]; 
                }

                for (int pizzaRow = 0; pizzaRow < P.R; pizzaRow++)
                {
                    Console.WriteLine($"Row #{pizzaRow}");
                    for (int pizzaCol = 0; pizzaCol < P.C; pizzaCol++)
                    {
                        List<int> cellValidPermutations = new List<int>();
                        for (int sliceNumber = 0; sliceNumber < validSlices.Length; sliceNumber++)
                        {
                            bool[][] slice = validSlices[sliceNumber];

                            int height = slice.Length;
                            int width = slice[0].Length;

                            if (height + pizzaRow <= P.R && width + pizzaCol <= P.C)
                            {
                                bool isValid = true;

                                for (int sliceRow = 0; sliceRow < height; sliceRow++)
                                {
                                    for (int sliceCol = 0; sliceCol < width; sliceCol++)
                                    {
                                        if (slice[sliceRow][sliceCol] != P[pizzaRow + sliceRow][pizzaCol + sliceCol])
                                        {
                                            isValid = false;
                                            break;
                                        }
                                    }
                                    if (!isValid)
                                    {
                                        break;
                                    }
                                }

                                if (isValid)
                                {
                                    cellValidPermutations.Add(sliceNumber);
                                }
                            }
                        }
                        legalSlices[pizzaRow][pizzaCol] = cellValidPermutations.ToArray();
                    }
                }
            }

#if DEBUG

            if (BeginConditionalSection("Print valid slice permutations"))
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

#endif

            BeginSection("Save all slices to file");
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
            int seed = 1 << (height * width);
            int initialMask = seed >> 1;
            seed--;

            do
            {
                short mushrooms = 0;
                short tomatoes = 0;
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
