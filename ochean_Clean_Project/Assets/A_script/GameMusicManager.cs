using UnityEngine;
using System.Collections;

public class GameMusicManager : MonoBehaviour
{
    public AudioSource[] musicTracks;
    public float delayBetweenTracks = 1f;

    private int currentTrackIndex = 0;
    private Coroutine musicCoroutine;
    private bool isFadingOut = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (musicTracks == null || musicTracks.Length == 0)
        {
            Debug.LogWarning("No music tracks assigned!");
            return;
        }

        foreach (AudioSource source in musicTracks)
        {
            source.playOnAwake = false;
        }
    }

    void Start()
    {
        musicCoroutine = StartCoroutine(PlayMusicLoop());
    }

    IEnumerator PlayMusicLoop()
    {
        while (!isFadingOut)
        {
            AudioSource track = musicTracks[currentTrackIndex];
            track.Play();

            yield return new WaitWhile(() => track.isPlaying && !isFadingOut);

            if (isFadingOut) yield break;

            yield return new WaitForSeconds(delayBetweenTracks);
            currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
        }
    }

    public void FadeOutAndDestroy(float fadeDuration = 2f)
    {
        if (!isFadingOut)
        {
            StartCoroutine(FadeOutRoutine(fadeDuration));
        }
    }

    IEnumerator FadeOutRoutine(float fadeDuration)
    {
        isFadingOut = true;

        // Fade out all music tracks
        foreach (AudioSource track in musicTracks)
        {
            if (track.isPlaying)
            {
                float startVolume = track.volume;
                float t = 0f;

                while (t < fadeDuration)
                {
                    t += Time.deltaTime;
                    track.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                    yield return null;
                }

                track.Stop();
                track.volume = startVolume; // reset volume jika muncul lagi nanti
            }
        }

        yield return new WaitForSeconds(0.2f); // delay kecil

        Destroy(gameObject);
    }
}
