using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContainerBase : MonoBehaviour
{
    public string Name => _name;
    [SerializeField] protected string _name;
    public int Capacity => _capacity;
    [SerializeField][Min(0)] protected int _capacity;
    public int ItemCount { get; private set; }
    protected ArraySegment<ItemBaseSO> _slots;

    [SerializeField] protected List<ItemBaseSO> _initialItems;

    public bool IsLive { get; private set; } = false;

    protected virtual void Awake()
    {
        IsLive = true;
    }

    public virtual void Init(ItemBaseSO[] backingStore, int start)
    {
        int tailLength = backingStore.Length - start;
        _slots = new ArraySegment<ItemBaseSO>(backingStore, start, tailLength);


        if (_initialItems.Count > Capacity)
        {
            Debug.LogError($"Too many initial items (capacity {Capacity}, got {_initialItems.Count})");
        }
        else
        {
            _initialItems.CopyTo(backingStore, start);
            ItemCount = _initialItems.Count;
            DebugPrint();
        }
    }

    /// <summary>
    /// Transfer all items from this container to the destination.
    /// This is the low-level primitive used by TakeAll/DumpAll.
    /// </summary>
    public virtual void TransferAll(ContainerBase destination)
    {
        Span<ItemBaseSO> srcSpan = _slots.AsSpan();
        for (int i = 0; i < srcSpan.Length && ItemCount > 0; i++)
        {
            if (srcSpan[i] != null)
            {
                ItemBaseSO item = srcSpan[i];
                // Debug.Log($"Transfering item {item.Name} at {Name}:{i}");
                srcSpan[i] = null;
                ItemCount--;
                destination.Add(item);
            }
        }
    }

    /// <summary>
    /// Swap items between two containers by index.
    /// </summary>
    public virtual void Swap(int srcIdx, ContainerBase destination, int dstIdx)
    {
        Span<ItemBaseSO> srcSpan = _slots.AsSpan();
        Span<ItemBaseSO> dstSpan = destination._slots.AsSpan();
        (srcSpan[srcIdx], dstSpan[dstIdx]) = (dstSpan[dstIdx], srcSpan[srcIdx]);
        if (srcSpan[srcIdx] == null && dstSpan[dstIdx] != null)
        {
            // An item was transferred from src to dst
            ItemCount--;
            destination.ItemCount++;
        }
        else if (srcSpan[srcIdx] != null && dstSpan[dstIdx] == null)
        {
            // An item was transferred from dst to src
            ItemCount++;
            destination.ItemCount--;
        }
    }

    /// <summary>
    /// Put the given item in the first free slot
    /// </summary>
    /// <param name="item"></param>
    public virtual void Add(ItemBaseSO item)
    {
        Span<ItemBaseSO> span = _slots.AsSpan();
        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] == null)
            {
                span[i] = item;
                ItemCount++;
                Debug.Log($"Item {item.Name} added to {Name} local index {i}");
                return;
            }
        }
    }

    public void DebugPrint()
    {
        string[] contents = Enumerable
            .Range(0, Capacity) // only within capacity
            .Select(i => _slots[i] == null ? "_" : _slots[i]!.Name)
            .ToArray();

        Debug.Log($"{Name}({ItemCount}): [{string.Join(",", contents)}]");
    }

}

