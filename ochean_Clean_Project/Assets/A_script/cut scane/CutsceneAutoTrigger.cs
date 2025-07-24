using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CutsceneAutoTrigger : MonoBehaviour
{
    [Header("Transisi Hitam")]
    public Image transisiHitam;
    public float fadeDuration = 1f;

    [Header("Pengaturan Auto Ganti Scene")]
    public float delayBeforeSceneChange = 3f;
    public int targetSceneIndex = 1;

    private bool alreadyTriggered = false;

    void OnEnable()
    {
        if (!alreadyTriggered)
        {
            alreadyTriggered = true;
            StartCoroutine(AutoChangeScene());
        }
    }

    IEnumerator AutoChangeScene()
    {
        // Tunggu beberapa detik sebelum ganti scene
        yield return new WaitForSeconds(delayBeforeSceneChange);

        // Fade out ke hitam
        yield return StartCoroutine(FadeScreen(0f, 1f, fadeDuration));

        // Ganti scene
        SceneManager.LoadScene(targetSceneIndex);
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
            clr.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
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
