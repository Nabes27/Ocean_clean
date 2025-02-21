using System.Collections;
using UnityEngine;

public class AI_Spawner : MonoBehaviour
{
    [Header("AI Spawn Settings")]
    [SerializeField] private GameObject aiCarPrefab; // Prefab AI Car
    [SerializeField] private float spawnRadius = 10f; // Radius random spawn
    [SerializeField] private float minSpawnRotation = 0f; // Minimum rotasi spawn (derajat)
    [SerializeField] private float maxSpawnRotation = 360f; // Maksimum rotasi spawn (derajat)

    [Header("Spawn Timing")]
    public float minSpawnTime = 3f; // Minimum waktu antar spawn (bisa diubah di Inspector)
    public float maxSpawnTime = 5f; // Maksimum waktu antar spawn (bisa diubah di Inspector)

    [Header("Spawn Distance")]
    public float startSpawnDistance = 50f; // Jarak mulai spawn (bisa diubah di Inspector)
    public float stopSpawnDistance = 40f; // Jarak berhenti spawn (bisa diubah di Inspector)

    [Header("Player Reference")]
    public Transform player; // Objek player (bisa di-set di Inspector)

    private bool canSpawn = false;

    void Start()
    {
        StartCoroutine(CheckDistance());
    }

    IEnumerator CheckDistance()
    {
        while (true)
        {
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.position);

                if (distance <= startSpawnDistance && distance > stopSpawnDistance)
                {
                    if (!canSpawn)
                    {
                        canSpawn = true;
                        StartCoroutine(SpawnAI());
                    }
                }
                else
                {
                    canSpawn = false;
                }
            }

            yield return new WaitForSeconds(1f); // Cek jarak setiap 1 detik
        }
    }

    IEnumerator SpawnAI()
    {
        while (canSpawn)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Quaternion spawnRotation = GetRandomSpawnRotation();
            Instantiate(aiCarPrefab, spawnPosition, spawnRotation);

            float spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomCircle.x, 0, randomCircle.y) + transform.position;
        return spawnPosition;
    }

    private Quaternion GetRandomSpawnRotation()
    {
        float randomRotationY = Random.Range(minSpawnRotation, maxSpawnRotation);
        return Quaternion.Euler(0, randomRotationY, 0);
    }
}
