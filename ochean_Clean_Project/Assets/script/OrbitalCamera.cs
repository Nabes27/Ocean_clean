using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public Transform target; // Objek yang menjadi pusat rotasi
    public float distance = 10.0f; // Jarak kamera dari target
    public float rotationSpeed = 5.0f; // Kecepatan rotasi
    public float scrollSpeed = 2.0f; // Kecepatan zoom dengan scroll
    public float minDistance = 2.0f; // Jarak minimum kamera
    public float maxDistance = 20.0f; // Jarak maksimum kamera

    private float currentX = 0.0f; // Sudut rotasi horizontal
    private float currentY = 0.0f; // Sudut rotasi vertikal
    public float yMinLimit = -30f; // Batas rotasi vertikal minimum
    public float yMaxLimit = 60f; // Batas rotasi vertikal maksimum

    void Update()
    {
        // Ambil input dari mouse
        if (Input.GetMouseButton(1)) // Klik kanan mouse untuk rotasi
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
        }

        // Ambil input dari scroll wheel untuk zoom
        distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
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
