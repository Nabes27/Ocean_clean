using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTeleport : MonoBehaviour
{
    [Header("Pengaturan Teleport")]
    public float holdDuration = 2.0f;
    public string teleportTag = "TeleportPoint";
    public float searchRadius = 100f;

    [Header("Transisi Visual")]
    public Image transisiHitam;

    private float holdTimer = 0f;
    private bool isHolding = false;
    private Rigidbody rb;

    public float stopDecelerationValue = 5f;
    public float stopDuration = 1f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (transisiHitam != null)
            transisiHitam.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdDuration && !isHolding)
            {
                isHolding = true;
                StartCoroutine(FadeTeleport());
            }
        }
        else
        {
            holdTimer = 0f;
            isHolding = false;
        }
    }

    IEnumerator FadeTeleport()
    {
        if (transisiHitam != null)
            transisiHitam.gameObject.SetActive(true);

        yield return StartCoroutine(Fade(0f, 1f, 0.5f)); // Fade to black

        TeleportToNearestPoint();

        yield return StartCoroutine(Fade(1f, 0f, 0.5f)); // Fade back

        if (transisiHitam != null)
            transisiHitam.gameObject.SetActive(false);
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color clr = transisiHitam.color;

        while (elapsed < duration)
        {
            float a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            clr.a = a;
            transisiHitam.color = clr;

            elapsed += Time.deltaTime;
            yield return null;
        }

        clr.a = endAlpha;
        transisiHitam.color = clr;
    }

    void TeleportToNearestPoint()
    {
        GameObject[] teleportPoints = GameObject.FindGameObjectsWithTag(teleportTag);
        if (teleportPoints.Length == 0) return;

        GameObject nearestPoint = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject point in teleportPoints)
        {
            float distance = Vector3.Distance(transform.position, point.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestPoint = point;
            }
        }

        if (nearestPoint != null)
        {
            // Aktifkan dulu jika disable
            if (!nearestPoint.activeInHierarchy)
                nearestPoint.SetActive(true);

            // Ambil komponen rotasi
            TeleportPoint tp = nearestPoint.GetComponent<TeleportPoint>();

            // Reset kecepatan Rigidbody
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Teleport ke posisi dan rotasi
            transform.position = nearestPoint.transform.position;
            transform.rotation = tp != null ? tp.GetRotation() : nearestPoint.transform.rotation;

            Debug.Log("Teleported to: " + nearestPoint.name);
        }

        PlayerBoat boat = GetComponent<PlayerBoat>();
        if (boat != null)
        {
            boat.SetTemporaryDeceleration(5f, 1.0f); // contoh: rem keras selama 1 detik
        }


    }
}
