using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcheanManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float WaveHight = 0.1f;
    public float WaveFrequency = 1f;
    public float WaveSpeed = 0.8f;
    public float NormalStrength = 1f;
    public float Somthnes = 0.04f;


    public Transform ocean;

    Material oceanMaterial;
    Texture2D WaveDisplacment;

    [Header("Ocean Colors (Hex)")]
    public Color color = new Color32(0x00, 0x55, 0xFF, 0xFF);     // #0055FF
    public Color color2 = new Color32(0x00, 0xBB, 0xD1, 0xFF);    // #00BBD1

    void Start()
    {
        SetVaiables();
    }

    void SetVaiables()
    {
        oceanMaterial = ocean.GetComponent<Renderer>().sharedMaterial;
        WaveDisplacment = (Texture2D)oceanMaterial.GetTexture("_WaveDisplacment");
    }

    public float WaterHightAtPosition(Vector3 position)
    {
        return ocean.position.y + WaveDisplacment.GetPixelBilinear(position.x * WaveFrequency/100, position.z * WaveFrequency/100 + Time.time * WaveSpeed/100).g * WaveHight/100 * ocean.localScale.x;
    }

    void OnValidate()
    {
        if(!oceanMaterial)
            SetVaiables();

        UpdateMaterial();
    }

    //
    public void ApplySomthnes()
    {
        if (oceanMaterial == null)
        {
            SetVaiables();
        }

        oceanMaterial.SetFloat("_Somthnes", Somthnes);
    }

    //

    void UpdateMaterial()
    {
        oceanMaterial.SetFloat("_WaveFrequency", WaveFrequency / 100);
        oceanMaterial.SetFloat("_WaveSpeed", WaveSpeed / 100);
        oceanMaterial.SetFloat("_WaveHight", WaveHight / 100);
        oceanMaterial.SetFloat("_NormalStrenght", NormalStrength / 100);
        oceanMaterial.SetFloat("_Somthnes", Somthnes);

        oceanMaterial.SetColor("_Color", color);
        oceanMaterial.SetColor("_Color_2", color2);

    }
    // Update Watter
    public void UpdateColors()
    {
        if (oceanMaterial == null)
            SetVaiables();

        UpdateMaterial();
    }



}