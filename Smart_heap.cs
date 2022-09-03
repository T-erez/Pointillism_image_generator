using System;
using System.Collections.Generic;

namespace Pointillism_image_generator
{
    public class Node : IComparable<Node>
    {
        public readonly Pattern pattern;
        public int improvement;
        public readonly int backgroundContribution;
        public readonly int error;


        public Node(Pattern pattern, int improvement, int backgroundContribution, int error)
        {
            this.pattern = pattern;
            this.improvement = improvement;
            this.backgroundContribution = backgroundContribution;
            this.error = error;
        }

        public int CompareTo(Node otherNode)
        {
            if (otherNode == null)
            {
                return 1;
            }

            int higherPriority = this.backgroundContribution.CompareTo(otherNode.backgroundContribution);
            if (higherPriority == -1)
            {
                return -1;
            }
            else if (higherPriority == 0)
            {
                return this.improvement.CompareTo(otherNode.improvement);
            }
            else
            {
                return 1;
            }
        }

        public override bool Equals(object obj) => this.Equals(obj as Node);

        public bool Equals(Node otherNode)
        {
            if (otherNode == null)
            {
                return false;
            }

            if (this.GetType() != otherNode.GetType())
            {
                return false;
            }

            return (improvement == otherNode.improvement) && (backgroundContribution == otherNode.backgroundContribution);
        }


        public static bool operator ==(Node leftNode, Node rightNode)
        {
            if (leftNode is null)
            {
                if (rightNode is null)
                {
                    return true;
                }
                return false;
            }

            return leftNode.Equals(rightNode);
        }

        public static bool operator !=(Node leftNode, Node rightNode) => !(leftNode == rightNode);

        public static bool operator <(Node leftNode, Node rightNode)
        {
            if (leftNode == null || rightNode == null)
            {
                return false;
            }
            if (leftNode.backgroundContribution < rightNode.backgroundContribution)
            {
                return true;
            }
            else if (leftNode.backgroundContribution == rightNode.backgroundContribution)
            {
                if (leftNode.improvement < rightNode.improvement)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool operator >(Node leftNode, Node rightNode) => !(leftNode < rightNode | leftNode == rightNode);

        public override int GetHashCode() => (pattern.xIndex, pattern.yIndex, improvement).GetHashCode();
    }

    public class SmartHeap
    {
        public int capacity;
        public int count;
        Node[] heap;
        Dictionary<(int, int), int> heapIndexes;

        public SmartHeap(int capacity)
        {
            this.capacity = capacity;
            heap = new Node[capacity];
            heapIndexes = new Dictionary<(int, int), int>(capacity);
            count = 0;
        }

        public void Add(Node node, int error0)
        {
            if (count == capacity)
            {
                throw new IndexOutOfRangeException(String.Format("Heap is full."));
            }
            node.improvement = error0 - node.error;
            heap[count] = node;
            heapIndexes[(node.pattern.xIndex, node.pattern.yIndex)] = count;
            count++;
            BubbleUp(count - 1);
        }

        public Node GetMax()
        {
            if (count == 0)
            {
                return null;
            }
            return heap[0];
        }

        public void Change(Node node)
        {
            int nodeIndex = heapIndexes[(node.pattern.xIndex, node.pattern.yIndex)];

            Node beforeChange = heap[nodeIndex];
            node.improvement = beforeChange.error - node.error;
            heap[nodeIndex] = node;

            if (node < beforeChange)
            {
                BubbleDown(nodeIndex);
            }
            else if (node > beforeChange)
            {
                BubbleUp(nodeIndex);
            }
        }

        private int GetLeftChild(int nodeIndex)
        {
            return 2 * nodeIndex + 1;
        }

        private int GetRightChild(int nodeIndex)
        {
            return 2 * nodeIndex + 2;
        }

        private int GetBiggerChild(int nodeIndex)
        {
            int rightChild = GetRightChild(nodeIndex);
            int leftChild = GetLeftChild(nodeIndex);
            if (rightChild < count && heap[rightChild] > heap[leftChild])
            {
                return rightChild;
            }
            else if (leftChild < count)
            {
                return leftChild;
            }
            else
            {
                return -1;
            }
        }

        private int GetParent(int nodeIndex)
        {
            return (nodeIndex - 1) / 2;
        }

        private void BubbleDown(int nodeIndex)
        {
            if (nodeIndex < 0 || nodeIndex > count - 1)
            {
                return;
            }
            int biggerChild = GetBiggerChild(nodeIndex);
            while (biggerChild != -1)
            {
                if (heap[nodeIndex] < heap[biggerChild])
                {
                    Node node = heap[nodeIndex];
                    heap[nodeIndex] = heap[biggerChild];
                    heap[biggerChild] = node;

                    heapIndexes[(heap[nodeIndex].pattern.xIndex, heap[nodeIndex].pattern.yIndex)] = nodeIndex;
                    heapIndexes[(heap[biggerChild].pattern.xIndex, heap[biggerChild].pattern.yIndex)] = biggerChild;

                    nodeIndex = biggerChild;
                    biggerChild = GetBiggerChild(nodeIndex);
                }
                else
                {
                    biggerChild = -1;
                }
            }
        }

        private void BubbleUp(int nodeIndex)
        {
            if (nodeIndex < 0 || nodeIndex > count - 1)
            {
                return;
            }
            int parent = GetParent(nodeIndex);
            while (parent >= 0)
            {
                if (heap[nodeIndex] > heap[parent])
                {
                    Node node = heap[nodeIndex];
                    heap[nodeIndex] = heap[parent];
                    heap[parent] = node;

                    heapIndexes[(heap[nodeIndex].pattern.xIndex, heap[nodeIndex].pattern.yIndex)] = nodeIndex;
                    heapIndexes[(heap[parent].pattern.xIndex, heap[parent].pattern.yIndex)] = parent;

                    nodeIndex = parent;
                    parent = GetParent(nodeIndex);
                }
                else
                {
                    parent = -1;
                }
            }
        }
    }
}