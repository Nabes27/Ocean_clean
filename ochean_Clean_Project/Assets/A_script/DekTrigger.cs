using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DekTrigger : MonoBehaviour
{
    public GameObject dekUI;
    public GameObject playerUI;
    public GameObject promptUI;
    public Camera mainCamera;
    public Camera islandCamera;
    public OrbitalCamera orbitalCamera;

    public Image transisiHitam; // Drag image hitam dari canvas transisi

    private bool isPlayerNearby = false;
    private bool isInDek = false;

    private PlayerBoat playerBoat;
    private Rigidbody playerRb;

    void Start()
    {
        mainCamera.gameObject.SetActive(true);
        islandCamera.gameObject.SetActive(false);

        if (playerUI != null) playerUI.SetActive(true);
        if (dekUI != null) dekUI.SetActive(false);

        if (transisiHitam != null)
        {
            Color clr = transisiHitam.color;
            clr.a = 0f;
            transisiHitam.color = clr;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            promptUI.SetActive(true);

            playerBoat = other.GetComponent<PlayerBoat>();
            playerRb = other.GetComponent<Rigidbody>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            promptUI.SetActive(false);

            if (isInDek)
                ToggleDek();
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeTransition());
        }
    }

    IEnumerator FadeTransition()
    {
        // Fade In (0 → 1)
        yield return StartCoroutine(Fade(0f, 1f, 0.5f));

        // Setelah layar hitam penuh, ganti mode
        ToggleDek();

        // Fade Out (1 → 0)
        yield return StartCoroutine(Fade(1f, 0f, 0.5f));
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

    void ToggleDek()
    {
        isInDek = !isInDek;

        dekUI.SetActive(isInDek);
        playerUI.SetActive(!isInDek);
        promptUI.SetActive(!isInDek);

        if (isInDek)
        {
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerBoat.enabled = false;

            mainCamera.gameObject.SetActive(false);
            islandCamera.gameObject.SetActive(true);

            if (orbitalCamera != null)
                orbitalCamera.enabled = false;
        }
        else
        {
            playerBoat.enabled = true;

            mainCamera.gameObject.SetActive(true);
            islandCamera.gameObject.SetActive(false);

            if (orbitalCamera != null)
                orbitalCamera.enabled = true;
        }
    }
}
