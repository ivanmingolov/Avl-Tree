namespace AvlTreeLab
{
    using System;

    public class Node<T> where T : IComparable<T>
    {
        private Node<T> leftChild;
        private Node<T> rightChild;

        public Node(T value)
        {
            this.Value = value;
        }

        public T Value { get; set; }

        public Node<T> LeftChild
        {
            get { return this.leftChild; }

            set
            {
                if (value != null)
                {
                    value.Parent = this;
                }

                this.leftChild = value;
            }
        }

        public Node<T> RightChild
        {
            get { return this.rightChild; }

            set
            {
                if (value != null)
                {
                    value.Parent = this;
                }

                this.rightChild = value;
            }
        }

        public Node<T> Parent { get; set; }

        public int BalanceFactor { get; set; }

        public bool IsLeftChild()
        {
            if (this.Parent.LeftChild?.Value.CompareTo(this.Value) == 0)
            {
                return true;
            }

            return false;
        }

        public bool IsRightChild()
        {
            if (this.Parent.RightChild?.Value.CompareTo(this.Value) == 0)
            {
                return true;
            }

            return false;
        }

        public int ChildrenCount
        {
            get
            {
                // TODO
                throw new NotImplementedException();
            }
        }

        public int CompareTo(Node<T> other)
        {
            return this.Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}

