using UnityEngine;
using System.Collections;

public class SkyboxChanger : MonoBehaviour
{
    public Material skyboxMaterial; // Assign Skybox Material di Inspector
    public float transitionDuration = 2f;
    public float timePerCycle = 10f; // Lama tiap waktu sebelum ganti (detik)

    public string SkyBox_Siang = "#808080";
    public string SkyBox_Sore = "#95595D";
    public string SkyBox_Malam = "#262626";
    public string SkyBox_Pagi = "#826F5D";

    public OcheanManager oceanManager; // Drag object dengan OcheanManager ke sini

    private Coroutine transitionCoroutine;
    private Coroutine autoCycleCoroutine;

    private int currentSkyboxIndex = 0;

    void Start()
    {
        // Mulai dari siang
        StartSkyboxTransition(SkyBox_Siang, 1f, 0.5f);
        autoCycleCoroutine = StartCoroutine(CycleTimeOfDay());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ChangeSkyboxSequence();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartSkyboxTransition(SkyBox_Siang, 1f, 0.5f); // Contoh manual
        }
    }

    IEnumerator CycleTimeOfDay()
    {
        while (true)
        {
            yield return new WaitForSeconds(timePerCycle);
            ChangeSkyboxSequence();
        }
    }

    void ChangeSkyboxSequence()
    {
        switch (currentSkyboxIndex)
        {
            case 0:
                StartSkyboxTransition(SkyBox_Siang, 1f, 0.5f); // Siang
                break;
            case 1:
                StartSkyboxTransition(SkyBox_Sore, 0.85f, 0.2f); // Sore
                break;
            case 2:
                StartSkyboxTransition(SkyBox_Malam, 0.5f, 0.04f); // Malam
                break;
            case 3:
                StartSkyboxTransition(SkyBox_Pagi, 0.5f, 0.1f); // Pagi
                break;
        }

        currentSkyboxIndex = (currentSkyboxIndex + 1) % 4;
    }

    void StartSkyboxTransition(string hexColor, float targetExposure, float targetSomthnes)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(TransitionSkybox(hexColor, targetExposure, targetSomthnes));
    }

    IEnumerator TransitionSkybox(string hexColor, float targetExposure, float targetSomthnes)
    {
        if (!ColorUtility.TryParseHtmlString(hexColor, out Color targetColor))
        {
            Debug.LogError("Invalid Hex Color: " + hexColor);
            yield break;
        }

        Color startColor = skyboxMaterial.GetColor("_Tint");
        float startExposure = skyboxMaterial.GetFloat("_Exposure");
        float startSomthnes = oceanManager.Somthnes;

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            // Transition Skybox
            skyboxMaterial.SetColor("_Tint", Color.Lerp(startColor, targetColor, t));
            skyboxMaterial.SetFloat("_Exposure", Mathf.Lerp(startExposure, targetExposure, t));
            RenderSettings.skybox = skyboxMaterial;
            DynamicGI.UpdateEnvironment();

            // Transition Somthnes
            oceanManager.Somthnes = Mathf.Lerp(startSomthnes, targetSomthnes, t);
            oceanManager.ApplySomthnes();

            yield return null;
        }

        // Final apply
        skyboxMaterial.SetColor("_Tint", targetColor);
        skyboxMaterial.SetFloat("_Exposure", targetExposure);
        RenderSettings.skybox = skyboxMaterial;
        DynamicGI.UpdateEnvironment();

        oceanManager.Somthnes = targetSomthnes;
        oceanManager.ApplySomthnes();
    }
}
