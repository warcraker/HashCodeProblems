using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GlobalUtils;
using HashCode.Common;

namespace HashPizza
{
    public static class First
    {
        // 'T' => true Tomato
        // 'M' => false Mushroom
        // [row][col]

        public static void GenerateFileWithAllSlices(string inputFilePath, string outputFolderPath)
        {

            Utils.BeginSection("File load");
            Pizza P = LoadPizza();


            Utils.BeginSection("Valid slices creation");
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

            Utils.BeginSection("Legal slices placement");
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

            }


            Utils.BeginSection("Save all slices to file");
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

            Utils.EndProgram();
        }

        private static Pizza LoadPizza()
        {
            string rootPath = Utils.GetAppRootFolder();
            ProblemFiles files = new ProblemFiles(rootPath);
            InputFile[] inputFiles = files.InputFiles.ToArray();

            int selectedFileIndex = Utils.SelectOption(inputFiles.Select(f => f.FileName).ToArray());
            string selectedFilePath = inputFiles[selectedFileIndex].FullPath;

            return new Pizza(selectedFilePath);
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

            {
        }
    }
}
