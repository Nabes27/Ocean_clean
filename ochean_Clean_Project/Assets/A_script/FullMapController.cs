using UnityEngine;
using UnityEngine.UI;
// Tambahkan ini di atas
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



public class FullMapController : MonoBehaviour
{
    [Header("UI")]
    public GameObject mapUI;                  // Panel/canvas map UI
    public RectTransform arrowIcon;          // Ikon player (arrow) di UI map

    [Header("Player")]
    public Transform playerTransform;        // Transform player

    [Header("Settings")]
    public float mapScale = 1f;              // Skala konversi world → UI map (1f = 1 world unit = 1 UI unit)

    private bool isMapActive = false;
    private Vector3 initialPlayerPosition;

    [Header("Camera Control")]
    public OrbitalCamera orbitalCamera;  // Drag komponen OrbitalCamera ke sini via Inspector

    public bool IsMapActive()
    {
        return isMapActive;
    }

    [Header("Map UI Animation")]
    public RectTransform mapUIRect;          // RectTransform dari mapUI
    public float hiddenPosY = -700f;         // Posisi Y saat disembunyikan
    public float transitionSpeed = 800f;     // Kecepatan transisi posisi
    private Vector2 targetMapPos;            // Posisi tujuan map UI

    [Header("Post Processing (URP)")]
    public Volume postProcessingVolume;  // Drag volume dari inspector sini
    private DepthOfField dof;             // Cache komponen Depth of Field


    void Start()
    {
        if (mapUIRect != null)
        {
            Vector2 startPos = mapUIRect.anchoredPosition;
            startPos.y = hiddenPosY;
            mapUIRect.anchoredPosition = startPos;
            targetMapPos = startPos;
        }

        if (playerTransform != null)
            initialPlayerPosition = playerTransform.position;

        // -- Tambahan untuk URP Volume --
        if (postProcessingVolume != null)
        {
            if (postProcessingVolume.profile.TryGet<DepthOfField>(out dof))
            {
                dof.active = false; // Awalannya blur OFF
            }
        }
    }



    void Update()
    {
        // Toggle UI map dengan tombol M
        //-
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMapActive = !isMapActive;

            // Set posisi target map (naik atau turun)
            if (mapUIRect != null)
            {
                targetMapPos = new Vector2(mapUIRect.anchoredPosition.x, isMapActive ? 0f : hiddenPosY);
            }

            // Nonaktifkan orbital kamera saat map aktif
            if (orbitalCamera != null)
            {
                orbitalCamera.SetOrbitalActive(!isMapActive);
            }

            // -- Tambahan untuk mengaktifkan Depth of Field --
            if (dof != null)
            {
                dof.active = isMapActive; // Aktifkan blur saat map aktif
            }
        }

        //-

        // Update posisi dan rotasi ikon jika map aktif
        if (isMapActive && arrowIcon != null && playerTransform != null)
        {
            UpdateArrowPosition();
            UpdateArrowRotation();
        }

        // Animasi transisi posisi map UI
        if (mapUIRect != null)
        {
            Vector2 currentPos = mapUIRect.anchoredPosition;
            mapUIRect.anchoredPosition = Vector2.MoveTowards(currentPos, targetMapPos, transitionSpeed * Time.deltaTime);
        }

    }

    void UpdateArrowPosition()
    {
        // Hitung offset posisi player dari posisi awal (X dan Z)
        Vector3 offset = playerTransform.position - initialPlayerPosition;
        Vector2 mapOffset = new Vector2(offset.x, offset.z) * mapScale;

        // Atur posisi arrow di map (anchoredPosition = posisi relatif terhadap tengah)
        arrowIcon.anchoredPosition = mapOffset;
    }

    void UpdateArrowRotation()
    {
        // Putar ikon mengikuti arah rotasi Y player (dikalikan -1 agar arah sesuai tampilan map)
        float angle = playerTransform.eulerAngles.y;
        arrowIcon.localRotation = Quaternion.Euler(0, 0, -angle);
    }
}