using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("Scene Tujuan")]
    public int sceneIndex = 1;

    [Header("Transisi Hitam")]
    public Image transisiHitam;
    public float fadeDuration = 1f;

    private bool triggered = false;

    void Start()
    {
        if (transisiHitam != null)
        {
            transisiHitam.gameObject.SetActive(false); // pastikan transparan dulu
            Color clr = transisiHitam.color;
            clr.a = 0f;
            transisiHitam.color = clr;
        }
    }

    void Update()
    {
        if (triggered) return;

        // Jika ada input dari keyboard atau mouse kanan/kiri
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            triggered = true;
            StartCoroutine(LoadSceneWithFade());
        }
    }

    IEnumerator LoadSceneWithFade()
    {
        yield return StartCoroutine(FadeScreen(0f, 1f, fadeDuration));
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator FadeScreen(float startAlpha, float endAlpha, float duration)
    {
        if (transisiHitam == null) yield break;

        transisiHitam.gameObject.SetActive(true);

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

        if (endAlpha == 0f)
            transisiHitam.gameObject.SetActive(false);
    }
}
