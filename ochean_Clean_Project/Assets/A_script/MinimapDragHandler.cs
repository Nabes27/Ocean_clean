using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public FullMapController mapController;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mapController.OnBeginDragArrow();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
        mapController.OnDragArrow(localPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        mapController.OnEndDragArrow();
    }
}
