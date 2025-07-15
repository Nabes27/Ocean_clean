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

    [Header("Map Blur Overlay")]
    public Image blurOverlayImage; // GameObject Image (UI) yang ingin diblur alpha-nya
    [Range(0, 255)]
    public byte blurAlphaOnMap = 114; // Alpha ketika map aktif

    [Range(0f, 5f)]
    public float blurFadeDuration = 0.5f; // Durasi transisi fade (dalam detik)

    private float currentBlurAlpha = 0f;       // Alpha saat ini (0–1)
    private float targetBlurAlpha = 0f;        // Alpha tujuan
    private float blurFadeTimer = 0f;          // Timer untuk transisi
    private bool isFadingBlur = false;         // Apakah sedang fading

    private bool isDraggingArrow = false;


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

        // Set awal alpha blur ke 0 saat game mulai
        if (blurOverlayImage != null)
        {
            Color c = blurOverlayImage.color;
            c.a = 0f;
            blurOverlayImage.color = c;
        }
        // Nonaktifkan objek UI map dan blur overlay di awal
        if (mapUI != null)
            mapUI.SetActive(false);

        if (blurOverlayImage != null)
            blurOverlayImage.gameObject.SetActive(false);

    }

    // Fungsi untuk tombol UI (OnClick)
    public void ToggleMapUI()
    {
        ToggleMap(); // Pakai fungsi toggle yang sama
    }


    void Update()
    {
        // Toggle UI map dengan tombol M
        //-
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();

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

            //
            if (blurOverlayImage != null)
            {
                targetBlurAlpha = isMapActive ? (blurAlphaOnMap / 255f) : 0f;
                currentBlurAlpha = blurOverlayImage.color.a;
                blurFadeTimer = 0f;
                isFadingBlur = true;
            }
            //
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

        // -- Proses fade blur alpha --
        if (isFadingBlur && blurOverlayImage != null)
        {
            blurFadeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(blurFadeTimer / blurFadeDuration);
            float newAlpha = Mathf.Lerp(currentBlurAlpha, targetBlurAlpha, t);

            Color c = blurOverlayImage.color;
            c.a = newAlpha;
            blurOverlayImage.color = c;

            if (Mathf.Approximately(newAlpha, targetBlurAlpha))
            {
                isFadingBlur = false;
            }
        }
        // Nonaktifkan setelah peta tertutup dan blur selesai
        if (!isMapActive && !isFadingBlur)
        {
            if (mapUI != null && mapUI.activeSelf)
                mapUI.SetActive(false);

            if (blurOverlayImage != null && blurOverlayImage.gameObject.activeSelf)
                blurOverlayImage.gameObject.SetActive(false);
        }


    }

    void ToggleMap()
    {
        isMapActive = !isMapActive;

        // Aktifkan game object-nya di awal toggle ON
        if (isMapActive)
        {
            if (mapUI != null) mapUI.SetActive(true);
            if (blurOverlayImage != null) blurOverlayImage.gameObject.SetActive(true);
        }

        // Set posisi target map (naik atau turun)
        if (mapUIRect != null)
        {
            targetMapPos = new Vector2(mapUIRect.anchoredPosition.x, isMapActive ? 0f : hiddenPosY);
        }

        // Matikan/aktifkan orbital camera
        if (orbitalCamera != null)
        {
            orbitalCamera.SetOrbitalActive(!isMapActive);
        }

        // Depth of Field (jika pakai URP)
        if (dof != null)
        {
            dof.active = isMapActive;
        }

        // Atur target alpha blur
        if (blurOverlayImage != null)
        {
            targetBlurAlpha = isMapActive ? (blurAlphaOnMap / 255f) : 0f;
            currentBlurAlpha = blurOverlayImage.color.a;
            blurFadeTimer = 0f;
            isFadingBlur = true;
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
        
    public void OnBeginDragArrow()
        {
            isDraggingArrow = true;
        }

    public void OnDragArrow(Vector2 dragPosition)
    {
        if (!isDraggingArrow) return;

        // Hitung offset di UI map → dunia 3D
        Vector2 offset = dragPosition / mapScale;
        Vector3 newPosition = initialPlayerPosition + new Vector3(offset.x, 0f, offset.y);

        // Ubah posisi player di dunia
        if (playerTransform != null)
            playerTransform.position = newPosition;

        // Perbarui ikon panah di minimap
        UpdateArrowPosition();
    }

    public void OnEndDragArrow()
    {
        isDraggingArrow = false;
    }


}