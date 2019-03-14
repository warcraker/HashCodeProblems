using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza.Models
{
    public class Tree
    {
        public Node Root { get; set; }

        public Tree(Slice[] slices)
        {
            bool isFinished = false;
            Node currentNode = Node.CreateRoot(slices[0]);

            do
            {
                Slice slice = null;

                if (currentNode.Left == null && leftNeedsToBeCreated())
                {
                    currentNode.CreateLeft(slice);
                    currentNode = currentNode.Left;
                }
                else if (currentNode.Right == null && rightNeedsToBeCreated())
                {
                    currentNode.CreateRight(slice);
                    currentNode = currentNode.Right;
                }
                else if (currentNode.Parent != null)
                {
                    currentNode = currentNode.Parent;
                }
                else
                {
                    isFinished = true;
                }
            } while (!isFinished);

            this.Root = currentNode;
        }

        private bool leftNeedsToBeCreated()
        {
            throw new NotImplementedException();
        }
        private bool rightNeedsToBeCreated()
        {
            throw new NotImplementedException();
        }
    }
}
