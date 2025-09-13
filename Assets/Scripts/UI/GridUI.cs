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
        if (containerBase != null)
        {
            container.Swap(srcIndex, containerBase, index);
            containerBase.GetComponent<BoxUI>()?.UpdateGrids();
            container.GetComponent<BoxUI>().UpdateGrids();
        }

    }
}
