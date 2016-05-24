namespace AvlTreeLab
{
    using System;
    
    public class AvlTree<T> where T : IComparable<T>
    {
        private Node<T> root;

        public int Count { get; private set; }
        
        public void Add(T item)
        {
            var inserted = true;
            if (this.root == null)
            {
                this.root = new Node<T>(item);
            }
            else
            {
                inserted = this.InsertInternal(this.root, item);
            }

            if (inserted)
            {
                this.Count++;
            }
        }

        public bool Contains(T item)
        {
            var currentNode = this.root;
            while (currentNode != null)
            {
                if (item.CompareTo(currentNode.Value) < 0)
                {
                    currentNode = currentNode.LeftChild;
                }
                else if (item.CompareTo(currentNode.Value) > 0)
                {
                    currentNode = currentNode.RightChild;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public void ForeachDfs(Action<int, T> action)
        {
            if (this.Count == 0)
            {
                return;
            }

            this.InOrderDfs(this.root, 1, action);
        }

        private void InOrderDfs(Node<T> node, int depth, Action<int, T> action)
        {
            if (node.LeftChild != null)
            {
                this.InOrderDfs(node.LeftChild, depth + 1, action);
            }

            action.Invoke(depth, node.Value);

            if (node.RightChild != null)
            {
                this.InOrderDfs(node.RightChild, depth + 1, action);
            }
        }

        private bool InsertInternal(Node<T> node, T item)
        {
            var currentNode = node;
            var nodeToInsert = new Node<T>(item);
            var shouldRetrace = false;
            while (true)
            {
                if (item.CompareTo(currentNode.Value) < 0)
                {
                    if (currentNode.LeftChild == null)
                    {
                        currentNode.LeftChild = nodeToInsert;

                        currentNode.BalanceFactor++;
                        if (currentNode.BalanceFactor != 0)
                        {
                            shouldRetrace = true;
                        }
                        
                        break;
                    }

                    currentNode = currentNode.LeftChild;
                }
                else if (item.CompareTo(currentNode.Value) > 0)
                {
                    if (currentNode.RightChild == null)
                    {
                        currentNode.RightChild = nodeToInsert;

                        currentNode.BalanceFactor--;
                        if (currentNode.BalanceFactor != 0)
                        {
                            shouldRetrace = true;
                        }

                        break;
                    }

                    currentNode = currentNode.RightChild;
                }
                else
                {
                    return false;
                }
            }

            if (shouldRetrace)
            {
                this.RetraceInsert(currentNode);
            }

            return true;
        }

        private void RetraceInsert(Node<T> node)
        {
            var parent = node.Parent;
            while (parent != null)
            {
                if (node.IsLeftChild())
                {
                    if (parent.BalanceFactor == 1)
                    {
                        parent.BalanceFactor++;
                        
                        if (node.BalanceFactor == -1) // Left -> Right case
                        {
                            this.RotateLeft(node);
                        }

                        // Left -> Left case
                        this.RotateRight(parent);
                    }
                    else if (parent.BalanceFactor == -1) // The parent is now balanced no need to go up
                    {
                        parent.BalanceFactor = 0;

                        break;
                    }
                    else // The parent balance factor increase because left sub tree grows
                    {
                        parent.BalanceFactor = 1;
                    }
                }
                else
                {
                    if (parent.BalanceFactor == 1)
                    {
                        parent.BalanceFactor--;

                        if (node.BalanceFactor == 1) // Right -> Left case
                        {
                            this.RotateRight(node);
                        }

                        // Right -> Right case
                        this.RotateLeft(parent);
                    }
                    else if (parent.BalanceFactor == 1) // The parent is now balanced no need to go up
                    {
                        parent.BalanceFactor = 0;

                        break;
                    }
                    else // The parent balance factor decrease because the right sub tree grows
                    {
                        parent.BalanceFactor = -1;
                    }
                }

                node = parent;
                parent = node.Parent;
            }


        }

        private void RotateRight(Node<T> node)
        {
            var parent = node.Parent;
            var child = node.LeftChild;
            if (parent != null)
            {
                if (node.IsRightChild())
                {
                    parent.RightChild = child;
                }
                else
                {
                    parent.LeftChild = child;
                }
            }
            else
            {
                this.root = child;
                this.root.Parent = null;
            }

            if (child != null)
            {
                node.LeftChild = child.RightChild;
                child.RightChild = node;
            }

            node.BalanceFactor -= 1 + Math.Max(child.BalanceFactor, 0);
            child.BalanceFactor -= 1 - Math.Min(node.BalanceFactor, 0);
        }

        private void RotateLeft(Node<T> node)
        {
            var parent = node.Parent;
            var child = node.RightChild;
            if (parent != null)
            {
                // Link parent with new node
                if (node.IsLeftChild())
                {
                    parent.LeftChild = child;
                }
                else
                {
                    parent.RightChild = child;
                }
            }
            else
            {
                // Child becomes new root
                this.root = child;
                this.root.Parent = null;
            }
             
            // Rotate left -> node becomes left child of his own right child
            if (child != null)
            {
                node.RightChild = child.LeftChild;
                child.LeftChild = node;
            }


            node.BalanceFactor += 1 - Math.Min(child.BalanceFactor, 0);
            child.BalanceFactor += 1 + Math.Max(node.BalanceFactor, 0);
        }
    }
}
