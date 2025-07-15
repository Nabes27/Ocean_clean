
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapInteractionController : MonoBehaviour, IDragHandler, IScrollHandler
{
    [Header("UI Elements")]
    public RectTransform mapImage;     // Gambar peta besar
    public RectTransform mapViewport;  // Batas layar HP (viewport, dengan masker)

    [Header("Zoom Settings")]
    public float zoomSpeed = 0.1f;
    public float minZoom = 0.5f;
    public float maxZoom = 2.5f;

    [Header("Pan Settings")]
    public float panSpeed = 1f;

    private float currentZoom = 1f;

    public void OnScroll(PointerEventData eventData)
    {
        // Zoom in/out
        float scrollDelta = eventData.scrollDelta.y;
        currentZoom += scrollDelta * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        mapImage.localScale = Vector3.one * currentZoom;

        ClampMapPosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Geser (pan/swipe)
        Vector2 delta = eventData.delta * panSpeed;
        mapImage.anchoredPosition += delta;

        ClampMapPosition();
    }

    void Start()
    {
        currentZoom = mapImage.localScale.x;
    }

    void ClampMapPosition()
    {
        // Hitung ukuran viewport dan map (dalam skala zoom)
        Vector2 viewSize = mapViewport.rect.size;
        Vector2 mapSize = mapImage.rect.size * currentZoom;

        // Hitung jarak max geser (biar map tetap di dalam viewport)
        float maxX = Mathf.Max(0, (mapSize.x - viewSize.x) / 2f);
        float maxY = Mathf.Max(0, (mapSize.y - viewSize.y) / 2f);

        // Clamp posisi anchored
        Vector2 pos = mapImage.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
        pos.y = Mathf.Clamp(pos.y, -maxY, maxY);

        mapImage.anchoredPosition = pos;
    }
}