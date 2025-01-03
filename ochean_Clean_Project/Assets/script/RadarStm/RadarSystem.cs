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
        // Simpan ikon lama untuk diproses (fade-out)
        List<GameObject> previousIcons = new List<GameObject>(radarIcons.Keys);

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

                // Jika ikon sudah ada, perbarui posisinya
                if (radarIcons.ContainsKey(target))
                {
                    radarIcons[target].GetComponent<RectTransform>().anchoredPosition = radarPosition;
                    previousIcons.Remove(target); // Ikon tetap di radar
                }
                else
                {
                    // Buat ikon baru
                    GameObject icon = Instantiate(radarIconPrefab, radarContainer);
                    icon.GetComponent<RectTransform>().anchoredPosition = radarPosition;
                    radarIcons[target] = icon;
                }
            }
        }

        // Proses ikon yang tidak lagi terdeteksi
        foreach (GameObject lostTarget in previousIcons)
        {
            if (radarIcons.ContainsKey(lostTarget))
            {
                GameObject icon = radarIcons[lostTarget];
                radarIcons.Remove(lostTarget);
                StartCoroutine(FadeOutAndDestroy(icon));
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

        Destroy(icon);
    }

    private void UpdateRadarRotation()
    {
        if (cameraTransform != null)
        {
            // Sesuaikan rotasi radar dengan rotasi horizontal kamera
            radarContainer.localRotation = Quaternion.Euler(0, 0, cameraTransform.eulerAngles.y);
        }
    }
}
