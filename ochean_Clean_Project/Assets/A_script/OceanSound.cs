using UnityEngine;
using System.Collections;

public class OceanSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip beachSound;
    public AudioClip oceanWaveSound;
    public LayerMask beachLayer; // Layer untuk area pantai
    private bool isInBeachArea = false;
    public float fadeDuration = 1.0f; // Durasi fade in/out

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = true;
        
        // Cek posisi awal apakah dalam zona pantai atau tidak
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f, beachLayer);
        if (colliders.Length > 0)
        {
            isInBeachArea = true;
            audioSource.volume = 1;
            audioSource.clip = beachSound;
        }
        else
        {
            isInBeachArea = false;
            audioSource.volume = 1;
            audioSource.clip = oceanWaveSound;
        }
        audioSource.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & beachLayer) != 0)
        {
            isInBeachArea = true;
            StartCoroutine(FadeToNewSound(beachSound));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & beachLayer) != 0)
        {
            isInBeachArea = false;
            StartCoroutine(FadeToNewSound(oceanWaveSound));
        }
    }

    IEnumerator FadeToNewSound(AudioClip newClip)
    {
        yield return StartCoroutine(FadeOut());
        audioSource.clip = newClip;
        audioSource.Play();
        yield return StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0, 1, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 1;
    }

    IEnumerator FadeOut()
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(1, 0, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0;
    }
}
