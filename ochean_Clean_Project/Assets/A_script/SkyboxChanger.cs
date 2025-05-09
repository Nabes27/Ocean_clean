using UnityEngine;
using System.Collections;

public class SkyboxChanger : MonoBehaviour
{
    public Material skyboxMaterial; // Assign Skybox Material di Inspector
    public float transitionDuration = 2f; // Waktu transisi dalam detik
    public string SkyBox_Siang = "#808080";
    public string SkyBox_Sore = "#95595D";


    private Coroutine transitionCoroutine;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartSkyboxTransition(SkyBox_Siang, 1f); // Warna abu-abu terang (Siang)
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            StartSkyboxTransition(SkyBox_Sore, 0.85f); // Warna kemerahan (Sore)
        }
    }

    void StartSkyboxTransition(string hexColor, float targetExposure)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(TransitionSkybox(hexColor, targetExposure));
    }

    IEnumerator TransitionSkybox(string hexColor, float targetExposure)
    {
        if (!ColorUtility.TryParseHtmlString(hexColor, out Color targetColor))
        {
            Debug.LogError("Invalid Hex Color: " + hexColor);
            yield break;
        }

        Color startColor = skyboxMaterial.GetColor("_Tint");
        float startExposure = skyboxMaterial.GetFloat("_Exposure");

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            skyboxMaterial.SetColor("_Tint", Color.Lerp(startColor, targetColor, t));
            skyboxMaterial.SetFloat("_Exposure", Mathf.Lerp(startExposure, targetExposure, t));
            RenderSettings.skybox = skyboxMaterial;
            DynamicGI.UpdateEnvironment();
            yield return null;
        }

        skyboxMaterial.SetColor("_Tint", targetColor);
        skyboxMaterial.SetFloat("_Exposure", targetExposure);
        RenderSettings.skybox = skyboxMaterial;
        DynamicGI.UpdateEnvironment();
    }
}
