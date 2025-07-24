using System.Collections;
using UnityEngine;

public class CorruptedZoneDetector : MonoBehaviour
{
    public LayerMask corruptedZoneLayer;
    public OcheanManager oceanManager;

    [Header("Normal Ocean Colors (Hex)")]
    public Color normalColor = new Color32(0x00, 0xDE, 0xFF, 0xFF);   // #00DEFF
    public Color normalColor2 = new Color32(0x00, 0x94, 0xA1, 0xFF);  // #0094A1

    [Header("Corrupted Ocean Colors (Hex)")]
    public Color corruptedColor = new Color32(0x00, 0x55, 0xFF, 0xFF);   // #0055FF
    public Color corruptedColor2 = new Color32(0x00, 0xBB, 0xD1, 0xFF);  // #00BBD1

    private bool isInCorruptedZone = false;

    private Coroutine colorTransitionCoroutine;
    public float transitionDuration = 2.0f;

    private void Start()
    {
        // Inisialisasi warna default di awal
        oceanManager.color = normalColor;
        oceanManager.color2 = normalColor2;
        oceanManager.UpdateColors();
    }

    public void ForceResetOcean()
    {
        if (colorTransitionCoroutine != null)
            StopCoroutine(colorTransitionCoroutine);

        colorTransitionCoroutine = StartCoroutine(TransitionOceanColor(
            oceanManager.color, oceanManager.color2,
            normalColor, normalColor2));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & corruptedZoneLayer) != 0)
        {
            isInCorruptedZone = true;
            if (colorTransitionCoroutine != null)
                StopCoroutine(colorTransitionCoroutine);

            colorTransitionCoroutine = StartCoroutine(TransitionOceanColor(
                oceanManager.color, oceanManager.color2,
                corruptedColor, corruptedColor2));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & corruptedZoneLayer) != 0)
        {
            isInCorruptedZone = false;
            if (colorTransitionCoroutine != null)
                StopCoroutine(colorTransitionCoroutine);

            colorTransitionCoroutine = StartCoroutine(TransitionOceanColor(
                oceanManager.color, oceanManager.color2,
                normalColor, normalColor2));
        }
    }




    private IEnumerator TransitionOceanColor(Color fromColor, Color fromColor2, Color toColor, Color toColor2)
    {
        float timer = 0f;

        while (timer < transitionDuration)
        {
            float t = timer / transitionDuration;
            oceanManager.color = Color.Lerp(fromColor, toColor, t);
            oceanManager.color2 = Color.Lerp(fromColor2, toColor2, t);
            oceanManager.UpdateColors();
            timer += Time.deltaTime;
            yield return null;
        }

        oceanManager.color = toColor;
        oceanManager.color2 = toColor2;
        oceanManager.UpdateColors();
    }


}
