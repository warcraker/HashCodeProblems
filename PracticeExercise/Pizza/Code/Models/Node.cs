using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza.Models
{
    public class Node
    {
        public Node Parent { get; private set; }
        public Node Left { get; private set; }
        public Node Right { get; private set; }
        public Slice Slice { get; private set; }

        private Node()
        {
            ;
        }

        public static Node CreateRoot(Slice slice)
        {
            return new Node
            {
                Slice = slice,
            };
        }
        public Node CreateLeft(Slice slice)
        {
            this.Left = new Node
            {
                Parent = this,
                Slice = slice,
            };

            return this.Left;
        }
        public Node CreateRight(Slice slice)
        {
            this.Right = new Node
            {
                Parent = this,
                Slice = slice,
            };

            return this.Right;
        }
    }
}
