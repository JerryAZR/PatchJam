# Container Interface

```csharp
public interface IContainer
{
    string Name { get; }
    int Capacity { get; }
    int ItemCount { get; }
    IReadOnlyList<string?> Slots { get; }

    /// <summary>
    /// Transfer all items from this container to the target.
    /// This is the low-level primitive used by TakeAll/DumpAll.
    /// </summary>
    void TransferAll(IContainer target);

    /// <summary>
    /// Swap items between two containers by index.
    /// </summary>
    void Swap(int sourceIndex, IContainer target, int targetIndex);
}
```
