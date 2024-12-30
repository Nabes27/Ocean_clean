using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcheanManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float WaveHight = 0.1f;
    public float WaveFrequency = 1f;
    public float WaveSpeed = 0.8f;

    public Transform ocean;

    Material oceanMaterial;
    Texture2D WaveDisplacment;

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

    void UpdateMaterial()
    {
        oceanMaterial.SetFloat("_WaveFrequency", WaveFrequency/100);
        oceanMaterial.SetFloat("_WaveSpeed", WaveSpeed/100);
        oceanMaterial.SetFloat("_WaveHight", WaveHight/100);
    }
    // Update Watter
    


}
