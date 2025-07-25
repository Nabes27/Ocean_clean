using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [Header("Transisi Hitam")]
    public Image transisiHitam;
    public float fadeDuration = 1f;

    [Header("Skybox Controller")]
    public SkyboxChanger skyboxChanger;


    //
    public void Sleep()
    {
        StartCoroutine(SleepAndAdvanceDay());
    }

    IEnumerator SleepAndAdvanceDay()
    {
        yield return StartCoroutine(FadeScreen(0f, 1f, fadeDuration));

        // Langsung ke pagi dan tambah hari
        if (skyboxChanger != null)
        {
            skyboxChanger.StartSkyboxTransition(skyboxChanger.SkyBox_Pagi, 0.5f, 0.1f);
            skyboxChanger.SkipToMorningAndAdvanceDay(); // Fungsi baru
        }

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeScreen(1f, 0f, fadeDuration));
    }

    //

    //
    void Start()
    {
        if (transisiHitam != null)
        {
            transisiHitam.gameObject.SetActive(false); // Matikan saat awal
            Color clr = transisiHitam.color;
            clr.a = 0f;
            transisiHitam.color = clr;
        }
    }

    //

    public void LoadSceneByIndex(int sceneIndex)
    {
        StartCoroutine(LoadSceneRoutine(sceneIndex));
    }

    public void ToggleGameObject(GameObject target)
    {
        if (target != null)
        {
            target.SetActive(!target.activeSelf); // Toggle status
        }
    }


    IEnumerator LoadSceneRoutine(int sceneIndex)
    {
        yield return StartCoroutine(FadeScreen(0f, 1f, fadeDuration)); // Fade Out

        SceneManager.LoadScene(sceneIndex); // Pindah scene
    }

    // Fungsi tombol: Ubah ke pagi dengan transisi
    public void ChangeToMorning()
    {
        StartCoroutine(ChangeToMorningRoutine());
    }

    IEnumerator ChangeToMorningRoutine()
    {
        yield return StartCoroutine(FadeScreen(0f, 1f, fadeDuration));

        if (skyboxChanger != null)
        {
            skyboxChanger.StartSkyboxTransition(skyboxChanger.SkyBox_Pagi, 0.5f, 0.1f);
            skyboxChanger.SkipToMorningAndAdvanceDay(); // ini memperbarui teks dan hari
        }

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(FadeScreen(1f, 0f, fadeDuration));
    }


    // Fungsi tombol: Bisa dipakai tombol lain
    public void ChangeToNight()
    {
        StartCoroutine(ChangeSkyboxWithFade(skyboxChanger.SkyBox_Malam, 0.5f, 0.04f));
    }

    // Fungsi umum: transisi fade + ganti skybox
    IEnumerator ChangeSkyboxWithFade(string hexColor, float exposure, float somthnes)
    {
        // Fade Out (ke hitam)
        yield return StartCoroutine(FadeScreen(0f, 1f, fadeDuration));

        // Ganti skybox pagi
        if (skyboxChanger != null)
        {
            skyboxChanger.StartSkyboxTransition(hexColor, exposure, somthnes);
        }

        // Tunggu sebentar agar efek terasa
        yield return new WaitForSeconds(0.5f);

        // Fade In (dari hitam)
        yield return StartCoroutine(FadeScreen(1f, 0f, fadeDuration));
    }

    // Fungsi umum: Transisi hitam alpha dari start ke end
    IEnumerator FadeScreen(float startAlpha, float endAlpha, float duration)
    {
        if (transisiHitam == null) yield break;

        transisiHitam.gameObject.SetActive(true); // Aktifkan saat mulai fade

        Color clr = transisiHitam.color;
        clr.a = startAlpha;
        transisiHitam.color = clr;

        float elapsed = 0f;
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

        // Nonaktifkan kembali setelah selesai fade
        if (endAlpha == 0f)
            transisiHitam.gameObject.SetActive(false);
    }


    // Contoh fungsi untuk tombol keluar game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
    
    public void StopAndDestroyGameMusic()
    {
        StartCoroutine(FadeOutAndDestroyMusic());
    }

    IEnumerator FadeOutAndDestroyMusic()
    {
        GameObject musicObj = GameObject.FindWithTag("GameMusic");

        if (musicObj != null)
        {
            GameMusicManager manager = musicObj.GetComponent<GameMusicManager>();
            if (manager != null)
            {
                manager.FadeOutAndDestroy(2f); // panggil fungsi di script manager
            }
        }

        yield return null;
    }




}
