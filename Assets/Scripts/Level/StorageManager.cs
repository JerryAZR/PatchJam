using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    [SerializeField] private GameObject _containerRoot;
    private ContainerBase[] _containers;

    private ItemBaseSO[] _backingStore;

    private void Start()
    {
        _containers = _containerRoot.GetComponentsInChildren<ContainerBase>();
        // First pass, calculate total size and allocate array
        int size = _containers.Sum(c => c.Capacity);
        _backingStore = new ItemBaseSO[size];
        // Second pass, initialize containers
        int index = 0;
        int order = 0;
        foreach (ContainerBase container in _containers)
        {
            if (container.IsLive)
            {
                order++;
                container.Init(_backingStore, index, order);
                index += container.Capacity;
            }
            else
            {
                Debug.LogError("StorageManager requires scene objects.");
            }
        }
    }

    private void Update()
    {
        // Quick and dirty debug options
        if (Input.GetKeyDown(KeyCode.T))
        {
            _containers[0].TransferAll(_containers[1]);
            foreach (ContainerBase container in _containers)
            {
                container.DebugPrint();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _containers[1].TransferAll(_containers[0]);
            foreach (ContainerBase container in _containers)
            {
                container.DebugPrint();
            }
        }
    }
}
