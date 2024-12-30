using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoat : MonoBehaviour
{
    public float maxSpeed = 10f;       // Kecepatan maksimum
    public float turnSpeed = 50f;      // Kecepatan belok
    public float acceleration = 2f;   // Akselerasi (laju percepatan)
    public float deceleration = 2f;   // Deselerasi (laju perlambatan)

    private Rigidbody rb;
    private float currentSpeed = 0f;  // Kecepatan saat ini

    private void Start()
    {
        // Ambil komponen Rigidbody dari kapal
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Ambil input dari pemain
        float moveInput = Input.GetAxis("Vertical"); // W (1), S (-1)
        float turnInput = Input.GetAxis("Horizontal"); // A (-1), D (1)

        // Jika ada input gerakan maju/mundur
        if (moveInput != 0)
        {
            // Tambah atau kurangi kecepatan secara bertahap menuju kecepatan maksimum
            currentSpeed = Mathf.Lerp(currentSpeed, moveInput * maxSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // Perlambatan saat tidak ada input
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
        }

        // Gerakkan kapal maju/mundur
        Vector3 moveDirection = transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);

        // Belokkan kapal
        if (turnInput != 0)
        {
            Quaternion turnRotation = Quaternion.Euler(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }
}
