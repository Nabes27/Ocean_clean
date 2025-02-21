using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_CarTraffic3 : MonoBehaviour
{
    public float speed = 10f;  // Kecepatan AI
    public float turnSpeed = 5f;  // Kecepatan belok AI
    public float stopDistance = 2f; // Jarak untuk mengganti checkpoint
    public float scanRange = 50f; // Jangkauan pencarian checkpoint
    public LayerMask checkpointLayer; // Layer untuk checkpoint biasa
    public LayerMask finalCheckpointLayer; // Layer untuk checkpoint terakhir yang bisa menghancurkan AI

    [Header("Player Detection Settings")]
    public LayerMask playerLayer; // LayerMask untuk mendeteksi player
    public float destroyDistance = 90f; // Jarak di mana AI akan dihancurkan

    private Transform targetCheckpoint; // Checkpoint yang dituju
    private bool isCheckpointReached = false; // Status apakah checkpoint sudah dilewati

    void Start()
    {
        FindNewCheckpoint();
    }

    void Update()
    {
        if (!IsPlayerInRange())
        {
            Destroy(gameObject); // Hancurkan AI jika player terlalu jauh
        }

        if (targetCheckpoint == null)
        {
            FindNewCheckpoint();
            return;
        }

        MoveTowardsCheckpoint();
    }

    bool IsPlayerInRange()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, destroyDistance, playerLayer);
        return players.Length > 0; // True jika ada player dalam jarak destroyDistance
    }

    void MoveTowardsCheckpoint()
    {
        Vector3 targetPosition = new Vector3(targetCheckpoint.position.x, transform.position.y, targetCheckpoint.position.z);
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Rotasi hanya pada sumbu Y agar tidak miring
        if (!isCheckpointReached)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
        }

        // Bergerak maju ke checkpoint
        transform.position += transform.forward * speed * Time.deltaTime;

        // Cek apakah sudah mencapai checkpoint dengan jarak minimal
        if (Vector3.Distance(transform.position, targetPosition) < stopDistance)
        {
            isCheckpointReached = true;
            Invoke("FindNewCheckpoint", 0.5f); // Beri jeda sebelum mencari checkpoint baru
        }
    }

    void FindNewCheckpoint()
    {
        isCheckpointReached = false; // Reset status checkpoint
        Collider[] checkpoints = Physics.OverlapSphere(transform.position, scanRange, checkpointLayer | finalCheckpointLayer);

        if (checkpoints.Length == 0)
        {
            targetCheckpoint = null;
            return;
        }

        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider checkpoint in checkpoints)
        {
            Vector3 directionToCheckpoint = (checkpoint.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, directionToCheckpoint) > 0) // Hanya pilih yang ada di depan
            {
                float distance = Vector3.Distance(transform.position, checkpoint.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = checkpoint.transform;
                }
            }
        }

        targetCheckpoint = closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & finalCheckpointLayer.value) != 0)
        {
            Destroy(gameObject); // Hancurkan AI Car jika menyentuh checkpoint terakhir
        }
    }
}
