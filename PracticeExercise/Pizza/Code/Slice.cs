using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class Slice
    {
        public short R1 { get; }
        public short C1 { get; }
        public short R2 { get; }
        public short C2 { get; }
        public int Width
        {
            get
            {
                return Math.Abs(this.R1 - this.R2) + 1;
            }
        }
        public int Heigth
        {
            get
            {
                return Math.Abs(this.C1 - this.C2) + 1;
            }
        }

        public Slice(int r1, int c1, int r2, int c2)
        {
            this.R1 = (short)r1;
            this.C1 = (short)c1;
            this.R2 = (short)r2;
            this.C2 = (short)c2;
        }

        public override string ToString()
        {
            return $"{this.R1} {this.C1} {this.R2} {this.C2}";
        }
    }
}
