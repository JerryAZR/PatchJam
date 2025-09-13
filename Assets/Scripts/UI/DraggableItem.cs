using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform dragIcon;
    private Canvas dragCanvas;

    void Start()
    {
        dragCanvas = DragLayer.Instance.GetComponent<Canvas>(); 
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragIcon = Instantiate(transform as RectTransform, dragCanvas.transform);
        dragIcon.SetAsLastSibling();
        dragIcon.GetComponent<CanvasGroup>().blocksRaycasts = false;

        GetComponent<CanvasGroup>().alpha = 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
            dragIcon.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().alpha = 1f;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var r in results)
        {
            GridUI grid = r.gameObject.GetComponent<GridUI>();
            if (grid != null)
            {
                grid.OnDropItem(transform.GetComponent<GridUI>().containerBase, transform.GetComponent<GridUI>().index);
                //Destroy(dragIcon.gameObject);
                return;
            }
        }

        Destroy(dragIcon.gameObject);
    }
}
