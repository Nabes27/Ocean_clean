using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMusicController : MonoBehaviour
{
    [Header("Referensi Skybox Changer")]
    public SkyboxChanger skyboxChanger;

    [Header("Audio")]
    public AudioSource musicSource;
    public float delayBetweenTracks = 3f;

    [Header("Musik Pagi + Siang")]
    public List<AudioClip> dayClips;

    [Header("Musik Sore + Malam")]
    public List<AudioClip> nightClips;

    private List<AudioClip> currentPlaylist;
    private Coroutine musicCoroutine;

    private int lastSkyboxIndex = -1;

    void Start()
    {
        UpdatePlaylist();
        musicCoroutine = StartCoroutine(MusicLoop());
    }

    void Update()
    {
        // Deteksi perubahan waktu dari SkyboxChanger
        if (skyboxChanger != null && skyboxChanger.CurrentSkyboxIndex() != lastSkyboxIndex)
        {
            lastSkyboxIndex = skyboxChanger.CurrentSkyboxIndex();
            UpdatePlaylist();
        }
    }

    void UpdatePlaylist()
    {
        if (skyboxChanger == null) return;

        int index = skyboxChanger.CurrentSkyboxIndex();

        // Waktu 0 dan 1 = Pagi + Siang → Day Music
        if (index == 0 || index == 1)
            currentPlaylist = dayClips;
        // Waktu 2 dan 3 = Sore + Malam → Night Music
        else
            currentPlaylist = nightClips;
    }

    IEnumerator MusicLoop()
    {
        while (true)
        {
            if (currentPlaylist != null && currentPlaylist.Count > 0)
            {
                AudioClip nextClip = currentPlaylist[Random.Range(0, currentPlaylist.Count)];

                if (musicSource != null && nextClip != null)
                {
                    musicSource.clip = nextClip;
                    musicSource.Play();

                    yield return new WaitForSeconds(nextClip.length + delayBetweenTracks);
                }
                else
                {
                    yield return new WaitForSeconds(1f); // Jeda fallback
                }
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
