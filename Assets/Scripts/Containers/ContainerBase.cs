using System;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.CorgiEngine;
using UnityEngine;
using UnityEngine.Splines;

public class ContainerBase : MonoBehaviour, IWeighted
{
    public string Name => name;
    public int Capacity => _capacity;
    [SerializeField][Min(0)] protected int _capacity;
    public int ItemCount { get; private set; }
    protected ArraySegment<ItemBaseSO> _slots;

    public ArraySegment<ItemBaseSO> Slots => _slots;

    protected ArraySegment<ItemBaseSO> _viewSlots;

    public ArraySegment<ItemBaseSO> ViewSlots => _viewSlots;

    public int Order { get; private set; } = 0;

    [SerializeField] protected List<ItemBaseSO> _initialItems;

    public bool IsLive { get; private set; } = false;

    public float Weight => _weight;
    private float _weight;

    public virtual Vector3 Position => transform.position;

    public static event Action OnOverflow;
    public static event Action OnTransferReject;

    protected virtual void Awake()
    {
        IsLive = true;
    }

    protected virtual void Start()
    {
#if UNITY_EDITOR
        // Validate hierarchy
        if (GetComponentInParent<ContainerRoot>() == null)
        {
            Debug.LogError($"Container {name} must be placed under the ContainerRoot.");
        }
#endif
    }

    protected virtual void OnDestroy()
    {
    }

    public virtual void Init(ItemBaseSO[] backingStore, int start, int order)
    {
        int tailLength = backingStore.Length - start;
        _slots = new ArraySegment<ItemBaseSO>(backingStore, start, tailLength);
        _viewSlots = new ArraySegment<ItemBaseSO>(backingStore, start, Capacity);
        Order = order;

        if (_initialItems.Count > Capacity)
        {
            Debug.LogError($"Too many initial items (capacity {Capacity}, got {_initialItems.Count})");
        }
        else
        {
            _initialItems.CopyTo(backingStore, start);
            ItemCount = _initialItems.Count;
            DebugPrint();
            UpdateInternals();
        }
    }

    /// <summary>
    /// Transfer all items from this container to the destination.
    /// This is the low-level primitive used by TakeAll/DumpAll.
    /// </summary>
    public virtual void TransferAll(ContainerBase destination)
    {
        // Allow only if two containers are nearby
        if (Vector3.Distance(Position, destination.Position) > 1.5f)
        {
            Debug.Log("TransferAll rejected because two containers are too far apart");
            OnTransferReject?.Invoke();
            return;
        }
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
        OnOverflow?.Invoke();
    }

    /// <summary>
    /// Swap items between two containers by index.
    /// </summary>
    public virtual void Swap(int srcIdx, ContainerBase destination, int dstIdx)
    {
        Vector3 playerPosition = LevelManager.Instance.Players[0].transform.position;

        if (Vector3.Distance(Position, playerPosition) > 1.5f)
        {
            Debug.Log("TransferAll rejected because player is too far away");
            OnTransferReject?.Invoke();
            return;
        }
        else if (Vector3.Distance(destination.Position, playerPosition) > 1.5f)
        {
            Debug.Log("TransferAll rejected because player is too far away");
            OnTransferReject?.Invoke();
            return;
        }

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
        UpdateInternals();
        destination.UpdateInternals();
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

    public virtual void UpdateInternals()
    {
        _weight = ViewSlots.Where(item => item != null).Sum(item => item.Weight);
        Debug.Log($"Weight of {Name} is now {_weight}");
    }
}

