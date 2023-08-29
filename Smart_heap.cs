using System;
using System.Collections.Generic;

namespace Pointillism_image_generator;
#nullable enable

/// <summary>
/// Defines a property that a value type or class implements to identify its instances.
/// </summary>
/// <typeparam name="TId">Identifier.</typeparam>
public interface IHasId<TId>
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public TId Id { get; }
}


/// <summary>
/// Defines a method that a value type or class implements to update its instances according to an old instance.
/// </summary>
public interface IUpdatable<TSelf>
{
    /// <summary>
    /// Updates itself according to an old instance.
    /// </summary>
    /// <param name="oldOne">old instance</param>
    public void Update(TSelf oldOne);
}

    
/// <summary>
/// Class SmartHeap includes a max heap and a dictionary which helps to change nodes in the heap faster.
/// </summary>
public class SmartHeap<TNode, TNodeId> where TNode: IComparable<TNode>, IUpdatable<TNode>, IHasId<TNodeId> where TNodeId : notnull
{
    private readonly int? _capacity;
    private int _count;
    private readonly List<TNode> _heap;
    private readonly Dictionary<TNodeId, int> _nodeIdToHeapIndices;

    /// <summary>Initializes SmartHeap.</summary>
    /// <param name="capacity">capacity of heap and dictionary</param>
    public SmartHeap(int capacity)
    {
        _capacity = capacity;
        _heap = new List<TNode>(capacity);
        _nodeIdToHeapIndices = new Dictionary<TNodeId, int>(capacity);
        _count = 0;
    }

    /// <summary>Initializes SmartHeap.</summary>
    public SmartHeap()
    {
        _capacity = null;
        _heap = new List<TNode>();
        _nodeIdToHeapIndices = new Dictionary<TNodeId, int>();
        _count = 0;
    }

    /// <summary>Adds an element to the heap.</summary>
    /// <param name="node">node to add</param>
    public void Add(TNode node)
    {
        if (_count == _capacity)
            throw new IndexOutOfRangeException("Heap has reached its capacity.");
        if (_nodeIdToHeapIndices.ContainsKey(node.Id))
            throw new ArgumentException("Node is already in the heap.");
            
        _heap.Add(node);
        _nodeIdToHeapIndices[node.Id] = _count;
        _count++;
        BubbleUp(_count - 1);
    }

    /// <summary>Returns a node without removing it from the heap. </summary>
    /// <returns>Node with the highest priority.</returns>
    public TNode? PeekMax()
    {
        return _count == 0 ? default : _heap[0];
    }

    /// <summary>Replaces a node in the heap with 'newNode'. The old node is found by node id.
    /// 'newNode' is updated according to the old node.</summary>
    /// <param name="newNode">new node</param>
    public void Update(TNode newNode)
    {
        int nodeIndex = _nodeIdToHeapIndices[newNode.Id];
        TNode oldNode = _heap[nodeIndex];
        newNode.Update(oldNode);
        _heap[nodeIndex] = newNode;

        if (newNode.CompareTo(oldNode) < 0)
            BubbleDown(nodeIndex);
        else if (newNode.CompareTo(oldNode) > 0)
            BubbleUp(nodeIndex);
    }

    private static int GetLeftChild(int nodeIndex)
    {
        return 2 * nodeIndex + 1;
    }

    private static int GetRightChild(int nodeIndex)
    {
        return 2 * nodeIndex + 2;
    }

    private int GetBiggerChild(int nodeIndex)
    {
        int rightChild = GetRightChild(nodeIndex);
        int leftChild = GetLeftChild(nodeIndex);
            
        if (rightChild < _count && _heap[rightChild].CompareTo(_heap[leftChild]) > 0)
            return rightChild;

        if (leftChild < _count)
            return leftChild;

        return -1;
    }

    private static int GetParent(int nodeIndex)
    {
        return (nodeIndex - 1) / 2;
    }

    private void BubbleDown(int nodeIndex)
    {
        if (nodeIndex < 0 || nodeIndex > _count - 1)
            return;
            
        int biggerChild = GetBiggerChild(nodeIndex);
        while (biggerChild != -1)
        {
            if (_heap[nodeIndex].CompareTo(_heap[biggerChild]) < 0)
            {
                (_heap[nodeIndex], _heap[biggerChild]) = (_heap[biggerChild], _heap[nodeIndex]);

                _nodeIdToHeapIndices[_heap[nodeIndex].Id] = nodeIndex;
                _nodeIdToHeapIndices[_heap[biggerChild].Id] = biggerChild;

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
        if (nodeIndex < 0 || nodeIndex > _count - 1)
            return;
            
        int parent = GetParent(nodeIndex);
        while (parent >= 0)
        {
            if (_heap[nodeIndex].CompareTo(_heap[parent]) > 0)
            {
                (_heap[nodeIndex], _heap[parent]) = (_heap[parent], _heap[nodeIndex]);

                _nodeIdToHeapIndices[_heap[nodeIndex].Id] = nodeIndex;
                _nodeIdToHeapIndices[_heap[parent].Id] = parent;

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