using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CutsceneAutoTriggerV2 : MonoBehaviour
{
    [Header("Game Object yang Diaktifkan")]
    public GameObject firstObject;   // Aktif setelah delayFirstObject
    public float delayFirstObject = 2f;

    public GameObject secondObject;  // Aktif setelah delaySecondObject
    public float delaySecondObject = 4f;

    [Header("Transisi Hitam")]
    public Image transisiHitam;
    public float fadeDuration = 1f;

    [Header("Pengaturan Auto Ganti Scene")]
    public float delayBeforeSceneChange = 6f;
    public int targetSceneIndex = 1;

    private bool alreadyTriggered = false;

    void OnEnable()
    {
        if (!alreadyTriggered)
        {
            alreadyTriggered = true;
            StartCoroutine(SequenceCutscene());
        }
    }

    IEnumerator SequenceCutscene()
    {
        // Tunggu lalu aktifkan objek pertama
        if (firstObject != null)
        {
            yield return new WaitForSeconds(delayFirstObject);
            firstObject.SetActive(true);
        }

        // Tunggu lalu aktifkan objek kedua
        if (secondObject != null)
        {
            float waitTime = Mathf.Max(0, delaySecondObject - delayFirstObject);
            yield return new WaitForSeconds(waitTime);
            secondObject.SetActive(true);
        }

        // Tunggu sebelum ganti scene
        float waitBeforeChange = Mathf.Max(0, delayBeforeSceneChange - delaySecondObject);
        yield return new WaitForSeconds(waitBeforeChange);

        // Fade out
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
