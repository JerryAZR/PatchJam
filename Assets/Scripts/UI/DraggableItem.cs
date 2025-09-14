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
        dragIcon.sizeDelta = new Vector2(50, 50);
        dragIcon.SetAsLastSibling();
        dragIcon.anchorMin = new Vector2(0.5f, 0.5f);
        dragIcon.anchorMax = new Vector2(0.5f, 0.5f);
        dragIcon.pivot = new Vector2(0.5f, 0.5f);
        dragIcon.GetComponent<CanvasGroup>().blocksRaycasts = false;
        dragIcon.GetComponent<Image>().enabled = false;

        GetComponent<CanvasGroup>().alpha = 0.5f;
    }

    void OnDisable()
    {
        if (dragIcon != null)
        {
            Destroy(dragIcon.gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(dragCanvas.transform as RectTransform, eventData.position, dragCanvas.worldCamera, out localPoint);
            dragIcon.anchoredPosition = localPoint;
        }
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
                if (grid.containerBase.name == "PlayerBag" || transform.GetComponent<GridUI>().containerBase.name == "PlayerBag")
                {
                    grid.OnDropItem(transform.GetComponent<GridUI>().containerBase, transform.GetComponent<GridUI>().index);
                    Destroy(dragIcon.gameObject);
                    return;
                }
            }
        }

        Destroy(dragIcon.gameObject);
    }
}
