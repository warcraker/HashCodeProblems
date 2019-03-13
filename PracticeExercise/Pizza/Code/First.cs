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

            Utils.BeginSection("Load pizza from file");
            Pizza P = new Pizza(inputFilePath);

            Utils.BeginSection("Generate possible slices");
            List<bool[][]> sliceTypes;
            {
                sliceTypes = new List<bool[][]>();
                int minimumSliceArea = P.L * 2;
                for (short h = 1; h <= P.R; h++)
                {
                    for (short w = 1; w <= P.C; w++)
                    {
                        int sliceArea = h * w;

                        if (minimumSliceArea <= sliceArea && sliceArea <= P.H)
                        {
                            sliceTypes.AddRange(GenerateSlicesTypes(h, w, P.L));
                        }
                    }
                }

                Console.WriteLine($"Slice types = {sliceTypes.Count}");
            }

            Utils.BeginSection("Locate slices");
            List<Slice> allSlices;
            {
                allSlices = new List<Slice>();
                int[][][] placedSliceIds = Utils.InitializeDefault2DVector<int[]>(P.R, P.C);

                for (int pizzaRow = 0; pizzaRow < P.R; pizzaRow++)
                {
                    Console.WriteLine($"Row #{pizzaRow}");
                    for (int pizzaCol = 0; pizzaCol < P.C; pizzaCol++)
                    {
                        placedSliceIds[pizzaRow][pizzaCol] = GetSliceIdsAtPosition(sliceTypes, P, pizzaRow, pizzaCol);
                    }
                }

                for (int row = 0; row < placedSliceIds.Length; row++)
                {
                    for (int col = 0; col < placedSliceIds[row].Length; col++)
                    {
                        foreach (int sliceTypeId in placedSliceIds[row][col])
                        {
                            bool[][] sliceType = sliceTypes[sliceTypeId];
                            allSlices.Add(new Slice(
                                row, 
                                col, 
                                row + sliceType.Length - 1, 
                                col + sliceType[0].Length - 1));
                        }
                    }
                }
            }

            Utils.BeginSection("Save slices to file");
            {
                Directory.CreateDirectory(outputFolderPath);
                string outputFilePath = Path.Combine(outputFolderPath, GetOutputFileName(P.InputFileName));

                Console.WriteLine($"Output file: {outputFilePath}");
                using (StreamWriter stream = File.CreateText(outputFilePath))
                {
                    stream.WriteLine(allSlices.Count);
                    foreach (Slice slice in allSlices)
                    {
                        stream.WriteLine(slice.ToString());
                    }
                }
            }

            Utils.EndProgram();
        }

        private static int[] GetSliceIdsAtPosition(List<bool[][]> genericValidSlices, Pizza pizza, int pizzaRow, int pizzaCol)
        {
            List<int> cellValidPermutations = new List<int>();
            for (int sliceNumber = 0; sliceNumber < genericValidSlices.Count; sliceNumber++)
            {
                bool[][] slice = genericValidSlices[sliceNumber];

                int height = slice.Length;
                int width = slice[0].Length;

                if (height + pizzaRow <= pizza.R && width + pizzaCol <= pizza.C)
                {
                    bool isValid = true;

                    for (int sliceRow = 0; sliceRow < height; sliceRow++)
                    {
                        for (int sliceCol = 0; sliceCol < width; sliceCol++)
                        {
                            if (slice[sliceRow][sliceCol] != pizza[pizzaRow + sliceRow][pizzaCol + sliceCol])
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

            return cellValidPermutations.ToArray();
        }

        private static Pizza LoadPizza(ProblemFiles files)
        {
            return 
        }

        private static IEnumerable<bool[][]> GenerateSlicesTypes(short height, short width, short minIngredients)
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

        private static string GetOutputFileName(string inputFileName)
        {
            return $"[{inputFileName}][{DateTime.Now:yyyy-MM-dd_HH-mm-ss}].txt";
        }
    }
}
