using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarSystem : MonoBehaviour
{
    public float radarRadius = 50f;           // Radius radar dalam satuan dunia
    public Transform playerTransform;        // Referensi transform pemain
    public GameObject radarIconPrefab;       // Prefab ikon untuk objek di radar
    public RectTransform radarContainer;     // Container radar di UI
    public Transform cameraTransform;        // Transform kamera orbital
    public float scanInterval = 1f;          // Interval pemindaian radar (dalam detik)
    public float fadeDuration = 1f;          // Durasi untuk memudarkan ikon radar

    private Dictionary<GameObject, GameObject> radarIcons = new Dictionary<GameObject, GameObject>(); // Map objek ke ikon
    private List<GameObject> fadingIcons = new List<GameObject>(); // Ikon yang sedang memudar

    private void Start()
    {
        // Mulai coroutine untuk pemindaian radar
        StartCoroutine(ScanRadarRoutine());
    }

    private IEnumerator ScanRadarRoutine()
    {
        while (true)
        {
            ScanRadar();
            yield return new WaitForSeconds(scanInterval);
        }
    }

    private void ScanRadar()
    {
        // Tandai semua ikon lama untuk diproses (fade-out)
        foreach (var icon in radarIcons.Values)
        {
            if (!fadingIcons.Contains(icon))
            {
                fadingIcons.Add(icon);
                StartCoroutine(FadeOutAndDestroy(icon));
            }
        }
        radarIcons.Clear(); // Kosongkan map ikon untuk data baru

        // Gunakan OverlapSphere untuk mendeteksi semua objek dalam radius
        Collider[] hits = Physics.OverlapSphere(playerTransform.position, radarRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Objek"))
            {
                GameObject target = hit.gameObject;

                // Hitungan posisi hanya berdasarkan X dan Z (abaikan Y)
                Vector3 flatWorldPosition = new Vector3(target.transform.position.x, playerTransform.position.y, target.transform.position.z);
                Vector3 offset = flatWorldPosition - playerTransform.position;

                float distance = offset.magnitude;
                if (distance > radarRadius) continue;

                Vector2 radarPosition = new Vector2(offset.x, offset.z) / radarRadius * radarContainer.rect.width / 2;

                // Buat ikon baru untuk objek yang terdeteksi
                GameObject icon = Instantiate(radarIconPrefab, radarContainer);
                icon.GetComponent<RectTransform>().anchoredPosition = radarPosition;
                radarIcons[target] = icon;
            }
        }
    }

    private IEnumerator FadeOutAndDestroy(GameObject icon)
    {
        Image iconImage = icon.GetComponent<Image>();
        Color originalColor = iconImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            iconImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        fadingIcons.Remove(icon);
        Destroy(icon);
    }

    private void LateUpdate()
    {
        UpdateRadarRotation();
    }

    private void UpdateRadarRotation()
    {
        if (cameraTransform != null)
        {
            // Rotasi radar mengikuti kamera orbital
            radarContainer.localRotation = Quaternion.Euler(0, 0, cameraTransform.eulerAngles.y);
        }
    }
}
