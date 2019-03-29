using System;

namespace HashCode.Common
{
    public class Utils
    {
        public static void EndProgram()
        {
            Console.WriteLine("--- FINISHED ---");
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }

        public static T[][] InitializeDefault2DVector<T>(int rows, int columns)
        {
            T[][] vector = new T[rows][];
            for (int row = 0; row < rows; row++)
            {
                vector[row] = new T[columns];
            }

            return vector;
        }
    }
}
