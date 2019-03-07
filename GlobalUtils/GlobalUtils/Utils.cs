using System.IO;
using System.Linq;

namespace HashCode.Common
{
    public class Utils
    {
        public static string GetAppRootFolder()
        {
            string executingPath = System.Reflection.Assembly.GetCallingAssembly().Location;
            DirectoryInfo executingPathInfo = new DirectoryInfo(executingPath);
            DirectoryInfo problemRootPathInfo = executingPathInfo.Parent.Parent.Parent.Parent;

            return problemRootPathInfo.FullName;
        }
        public static string[] GetFileLines(string filePath)
        {
            return File.ReadAllLines(filePath).ToArray();
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
