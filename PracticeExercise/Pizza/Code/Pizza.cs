namespace HashPizza
{
    public class Pizza
    {
        public short R { get; }
        public short C { get; }
        public short L { get; }
        public short H { get; }
        public bool[][] WholePizza { get { return (bool[][])this.cells.Clone(); } }

        private readonly bool[][] cells;

        public bool[] this[int r]
        {
            get { return this.cells[r]; }
        }

        public Pizza(short r, short c, short l, short h)
        {
            this.R = r;
            this.C = c;
            this.L = l;
            this.H = h;

            this.cells = new bool[r][];
            for (int i = 0; i < r; i++)
            {
                this.cells[i] = new bool[c];
            }
        }
    }
}
