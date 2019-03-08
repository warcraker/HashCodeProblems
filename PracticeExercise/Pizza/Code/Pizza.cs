using HashCode.Common;
using System.IO;
using System.Linq;

namespace HashPizza
{
    public class Pizza
    {
        public short R { get; }
        public short C { get; }
        public short L { get; }
        public short H { get; }
        public bool[][] Cells { get; }

        public bool[] this[int r]
        {
            get { return this.Cells[r]; }
        }

        public Pizza(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            string[] inputValues = lines[0].Split(' ');

            this.R = short.Parse(inputValues[0]);
            this.C = short.Parse(inputValues[1]);
            this.L = short.Parse(inputValues[2]);
            this.H = short.Parse(inputValues[3]);

            this.Cells = Utils.InitializeDefault2DVector<bool>(this.R, this.C);

            string[] pizzaLines = lines.Skip(1).ToArray();
            for (short row = 0; row < this.R; row++)
            {
                char[] rowChars = pizzaLines[row].ToCharArray();
                for (short col = 0; col < this.C; col++)
                {
                    this.Cells[row][col] = rowChars[col] == 'T';
                }
            }

        }
    }
}
