
using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public Transform target;             // Objek yang menjadi pusat rotasi
    public float distance = 10.0f;       // Jarak kamera dari target
    public float rotationSpeed = 5.0f;  // Kecepatan rotasi
    public float scrollSpeed = 2.0f;    // Kecepatan zoom dengan scroll
    public float minDistance = 2.0f;    // Jarak minimum kamera
    public float maxDistance = 20.0f;   // Jarak maksimum kamera

    private float currentX = 0.0f;       // Sudut rotasi horizontal
    private float currentY = 0.0f;       // Sudut rotasi vertikal
    public float yMinLimit = -30f;      // Batas rotasi vertikal minimum
    public float yMaxLimit = 60f;       // Batas rotasi vertikal maksimum

    private bool isOrbitalActive = false; // Apakah mode orbital aktif



    //
    public void SetOrbitalActive(bool value)
    {
        isOrbitalActive = value;
    }

    private bool IsMapOpen()
    {
        FullMapController mapController = FindObjectOfType<FullMapController>();
        if (mapController != null)
        {
            return mapController.IsMapActive(); // Panggil fungsi baru dari FullMapController
        }
        return false;
    }


    void Update()
    {
        // Klik kanan mouse untuk toggle mode orbital
        if (Input.GetMouseButtonDown(1) && !IsMapOpen())
        {
            isOrbitalActive = !isOrbitalActive; // Toggle mode orbital
        }


        // Jika mode orbital aktif, aktifkan rotasi kamera
        if (isOrbitalActive)
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
            distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

       
    }

    void LateUpdate()
    {
        if (target)
        {
            // Hitung posisi kamera berdasarkan rotasi dan jarak
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            Vector3 position = rotation * new Vector3(0, 0, -distance) + target.position;

            // Terapkan rotasi dan posisi ke kamera
            transform.rotation = rotation;
            transform.position = position;
        }
    }
}