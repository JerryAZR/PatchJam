using System;
using System.Collections.Generic;
using UnityEngine;

public class ContainerBase : MonoBehaviour
{
    public string Name { get; }
    public int Capacity { get; private set; }
    public int ItemCount { get; private set; }
    protected ArraySegment<ItemBase> _slots;

    public virtual void Init(ItemBase[] backingStore, int start)
    {
        int tailLength = backingStore.Length - start;
        _slots = new ArraySegment<ItemBase>(backingStore, start, tailLength);
        ItemCount = 0;
    }

    /// <summary>
    /// Transfer all items from this container to the destination.
    /// This is the low-level primitive used by TakeAll/DumpAll.
    /// </summary>
    public virtual void TransferAll(ContainerBase destination)
    {
        Span<ItemBase> srcSpan = _slots.AsSpan();
        for (int i = 0; i < srcSpan.Length && ItemCount > 0; i++)
        {
            if (srcSpan[i] != null)
            {
                ItemBase item = srcSpan[i];
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
        Span<ItemBase> srcSpan = _slots.AsSpan();
        Span<ItemBase> dstSpan = destination._slots.AsSpan();
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
    public virtual void Add(ItemBase item)
    {
        Span<ItemBase> span = _slots.AsSpan();
        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] == null)
            {
                span[i] = item;
                ItemCount++;
                Debug.Log($"Item {item.Name} added to {Name} local index {i}");
            }
        }
    }
}
