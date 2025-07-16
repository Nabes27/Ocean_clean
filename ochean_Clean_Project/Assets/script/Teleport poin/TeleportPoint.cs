using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    [Header("Custom Rotasi (kosongkan untuk pakai default transform)")]
    public Vector3 customEulerRotation;
    public bool useCustomRotation = false;



    public Quaternion GetRotation()
    {
        return useCustomRotation ? Quaternion.Euler(customEulerRotation) : transform.rotation;
    }
}
