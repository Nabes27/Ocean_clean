using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoat : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float turnSpeed = 50f;
    public float acceleration = 2f;
    public float deceleration = 2f;

    public int maxTrashCapacity = 15;
    private int currentTrash = 0;

    private Rigidbody rb;
    private float currentSpeed = 0f;


    [Header("Stuck Detection")]
    public string groundTag = "Ground"; // ganti jadi cek tag

    public Transform groundCheck; // posisi di bawah kapal
    public float checkRadius = 0.5f;
    public GameObject stuckNotificationUI; // UI "Hold R" canvas

    private bool isStuck = false;
    private float originalAcceleration;
    private float originalDeceleration;
    private float originalTurnSpeed;

    [Header("Animasi Mesin Kapal")]
    public Transform engineVisual;
    public float engineTurnAngle = 40f;
    public float engineTurnSpeed = 5f;

    [Header("Baling-Baling Kapal")]
    public Transform propeller;             // Objek baling-baling
    public float propellerSpeed = 360f;     // Derajat per detik (putaran)


    [Header("Suara Mesin")]
    public AudioSource engineAudio;
    public AudioClip engineRunningClip;
    public float engineVolume = 0.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        originalAcceleration = acceleration;
        originalDeceleration = deceleration;
        originalTurnSpeed = turnSpeed;


        if (stuckNotificationUI != null)
            stuckNotificationUI.SetActive(false);


    }

    private void Update()
    {
        CheckIfStuck();
    }

    private void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        if (moveInput != 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, moveInput * maxSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
        }

        Vector3 moveDirection = transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);

        if (turnInput != 0)
        {
            Quaternion turnRotation = Quaternion.Euler(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }

        // Animasi rotasi mesin saat belok
        if (engineVisual != null)
        {
            float targetY = 0f;

            if (turnInput > 0)
                targetY = -engineTurnAngle;
            else if (turnInput < 0)
                targetY = engineTurnAngle;

            Quaternion targetRot = Quaternion.Euler(0f, targetY, 0f);
            engineVisual.localRotation = Quaternion.Lerp(engineVisual.localRotation, targetRot, Time.fixedDeltaTime * engineTurnSpeed);
        }

        // Rotasi baling-baling berdasarkan arah gerak
        if (propeller != null)
        {
            float rotationDir = 0f;

            if (moveInput > 0)
                rotationDir = -1f; // Maju = rotasi negatif terhadap X
            else if (moveInput < 0)
                rotationDir = 1f;  // Mundur = rotasi positif terhadap X

            propeller.Rotate(Vector3.right * rotationDir * propellerSpeed * Time.fixedDeltaTime, Space.Self);
        }
        
        // Suara mesin kapal
        if (engineAudio != null && engineRunningClip != null)
        {
            if (!engineAudio.isPlaying)
            {
                engineAudio.clip = engineRunningClip;
                engineAudio.volume = engineVolume;
                engineAudio.loop = true;
                engineAudio.Play();
            }

            // Atur pitch berdasarkan gerakan
            float targetPitch = Mathf.Abs(moveInput) > 0.1f ? 1.5f : 1.0f; // 1.5 = suara meraung, 1.0 = idle
            engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, targetPitch, Time.fixedDeltaTime * 5f);
        }

    }

    void CheckIfStuck()
    //
    {
        bool stuckByGround = false;
        bool stuckByFlip = false;

        // Cek jika menyentuh GameObject bertag "Ground"
        if (groundCheck != null)
        {
            Collider[] hits = Physics.OverlapSphere(groundCheck.position, checkRadius);
            foreach (Collider col in hits)
            {
                if (col.CompareTag(groundTag))
                {
                    stuckByGround = true;
                    break;
                }
            }
        }

        // Cek jika kapal terbalik terlalu miring (Z rotasi mendekati ±180)
        float zRot = NormalizeAngle(transform.eulerAngles.z);
        if (Mathf.Abs(zRot - 180f) <= 30f) // toleransi 30 derajat
        {
            stuckByFlip = true;
        }

        bool currentlyStuck = stuckByGround || stuckByFlip;

        // Jika mulai stuck
        if (currentlyStuck && !isStuck)
        {
            isStuck = true;

            acceleration = 0f;
            deceleration = 0f;
            turnSpeed = 0f;

            if (stuckNotificationUI != null)
                stuckNotificationUI.SetActive(true);
        }
        // Jika bebas dari stuck
        else if (!currentlyStuck && isStuck)
        {
            isStuck = false;

            acceleration = originalAcceleration;
            deceleration = originalDeceleration;
            turnSpeed = originalTurnSpeed;

            if (stuckNotificationUI != null)
                stuckNotificationUI.SetActive(false);
        }
    }
    //

    float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }


    public bool CanCollectTrash()
    {
        return currentTrash < maxTrashCapacity;
    }

    public void CollectTrash(int amount)
    {
        currentTrash = Mathf.Clamp(currentTrash + amount, 0, maxTrashCapacity);
        ScoreManager.instance.UpdateTrashUI(currentTrash, maxTrashCapacity);
    }

    public void TransferTrashToStorage()
    {
        if (currentTrash > 0)
        {
            ScoreManager.instance.AddToStorage(currentTrash);
            currentTrash = 0;
            ScoreManager.instance.UpdateTrashUI(currentTrash, maxTrashCapacity);
        }
    }



    public void SetTemporaryDeceleration(float newValue, float duration)
    {
        StopAllCoroutines(); // hentikan coroutine sebelumnya jika ada
        StartCoroutine(ApplyTemporaryDeceleration(newValue, duration));
    }

    private IEnumerator ApplyTemporaryDeceleration(float newValue, float duration)
    {
        originalDeceleration = deceleration;
        deceleration = newValue;

        yield return new WaitForSeconds(duration);

        deceleration = originalDeceleration;
    }


}