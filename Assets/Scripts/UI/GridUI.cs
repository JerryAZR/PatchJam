using UnityEngine;
using UnityEngine.UI;

public class GridUI : MonoBehaviour
{
    public ContainerBase containerBase;
    public BoxUI boxUI;
    public ItemBaseSO so;
    public int index;

    public void OnDropItem(ContainerBase container, int srcIndex = 0)
    {
        Debug.Log($"On {name}");
        Debug.Log($"containerBase={containerBase.name}, container={container.name}");
        if (containerBase != null)
        {
            container.Swap(srcIndex, containerBase, index);
            containerBase.GetComponentInChildren<BoxUI>().UpdateGrids();
            container.GetComponentInChildren<BoxUI>().UpdateGrids();
        }
    }
}
