﻿using System;

namespace HashPizza.Models
{
    public class Slice : IComparable<Slice>
    {
        public short R1 { get; }
        public short C1 { get; }
        public short R2 { get; }
        public short C2 { get; }
        public int Area
        {
            get
            {
                return CalculateSide(this.R1, this.R2) * CalculateSide(this.C1, this.C2);
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
        public int CompareTo(Slice other)
        {
            return this.Area - other.Area;
        }

        private static int CalculateSide(short a, short b)
        {
            return Math.Abs(a - b) + 1;
        }
    }
}
