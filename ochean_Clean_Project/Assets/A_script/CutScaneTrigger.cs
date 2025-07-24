using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class CutScaneTrigger : MonoBehaviour

{
    public GameObject dekUI;
    public GameObject playerUI;
    public GameObject PlayerSound;
    public GameObject promptUI;
    public Camera mainCamera;
    //public Camera islandCamera;
    public OrbitalCamera orbitalCamera;

    public Image transisiHitam;

    public Transform islandViewTransform; // posisi target saat masuk dek
    private Vector3 defaultCamPos;
    private Quaternion defaultCamRot;


    private bool isPlayerNearby = false;
    private bool isInDek = false;

    private PlayerBoat playerBoat;
    private Rigidbody playerRb;

    private Vector3 lastOrbitalCamPos;
    private Quaternion lastOrbitalCamRot;

    void Start()
    {
        mainCamera.gameObject.SetActive(true);

        if (playerUI != null) playerUI.SetActive(true);
        if (dekUI != null) dekUI.SetActive(false);
        if (PlayerSound != null) PlayerSound.SetActive(true);

        if (transisiHitam != null)
        {
            transisiHitam.gameObject.SetActive(false);
        }

        // Simpan posisi awal kamera
        defaultCamPos = mainCamera.transform.position;
        defaultCamRot = mainCamera.transform.rotation;
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
    //
    IEnumerator FadeTransition()
    {
        if (transisiHitam != null)
            transisiHitam.gameObject.SetActive(true); // Aktifkan sebelum fade

        // Fade In (0 → 1)
        yield return StartCoroutine(Fade(0f, 1f, 0.5f));

        // Ganti mode
        ToggleDek();

        // Fade Out (1 → 0)
        yield return StartCoroutine(Fade(1f, 0f, 0.5f));

        if (transisiHitam != null)
            transisiHitam.gameObject.SetActive(false); // Matikan kembali setelah fade
    }

    //

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
    //
    {
        isInDek = !isInDek;

        dekUI.SetActive(isInDek);
        playerUI.SetActive(!isInDek);
        promptUI.SetActive(!isInDek);
        PlayerSound.SetActive(!isInDek);

        if (isInDek)
        {
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerBoat.enabled = false;

            // Simpan posisi terakhir sebelum nonaktifkan orbital
            lastOrbitalCamPos = mainCamera.transform.position;
            lastOrbitalCamRot = mainCamera.transform.rotation;


            // Pindahkan kamera ke islandViewTransform
            if (islandViewTransform != null)
            {
                mainCamera.transform.position = islandViewTransform.position;
                mainCamera.transform.rotation = islandViewTransform.rotation;
            }

            if (orbitalCamera != null)
                orbitalCamera.enabled = false;
        }
        else
        {
            playerBoat.enabled = true;

            // Kembalikan posisi kamera
            mainCamera.transform.position = lastOrbitalCamPos;
            mainCamera.transform.rotation = lastOrbitalCamRot;
            //

            if (orbitalCamera != null)
            {
                orbitalCamera.enabled = true;
                orbitalCamera.SetOrbitalActive(true);
            }


            if (orbitalCamera != null)
                orbitalCamera.enabled = true;
        }
    }
    //

}

