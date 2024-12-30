using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Buoncy_Object : MonoBehaviour
{
    public Transform[] floaters;

    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;

    public float AirDrag = 0f;
    public float AirAngularDrag = 0.05f;

    public float floatingPower = 15f;

    OcheanManager ocheanManager;
    ///public float waterHeight = 0f;

    Rigidbody m_Rigidbody;

    int floatersUnderwater;
    bool UnderWater;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        ocheanManager = FindObjectOfType<OcheanManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        floatersUnderwater = 0;
        for(int i = 0; i < floaters.Length; i ++)
        {
            float difference = floaters[i].position.y - ocheanManager.WaterHightAtPosition(floaters[i].position);

            if (difference < 0){
                m_Rigidbody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floaters[i].position, ForceMode.Force);
                floatersUnderwater += 1;
                if(!UnderWater){
                    UnderWater = true;
                    SwitchState(true);
                }
            }
        }

        
        if(UnderWater && floatersUnderwater == 0)
        {
            UnderWater = false;
            SwitchState(false);
        }
    }

    void SwitchState(bool isUnderWater)
    {
        if(isUnderWater){
            m_Rigidbody.drag = underWaterDrag;
            m_Rigidbody.angularDrag = underWaterAngularDrag;
        }
        else
        {
            m_Rigidbody.drag = AirDrag;
            m_Rigidbody.angularDrag = AirAngularDrag;
        }
    }

    // Watter
}
