using System.Collections;
using UnityEngine;

public class AI_Spawner : MonoBehaviour
{
    [Header("AI Spawn Settings")]
    [SerializeField] private GameObject[] aiCarPrefabs; // List AI Car Prefabs
    [SerializeField] private float spawnRadius = 10f; // Radius spawn
    [SerializeField] private float spawnRotationY = 0f; // Satu nilai rotasi tetap (derajat)

    [Header("Spawn Timing")]
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 5f;

    [Header("Spawn Distance")]
    public float startSpawnDistance = 50f;
    public float stopSpawnDistance = 40f;

    [Header("Player Reference")]
    public Transform player;

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

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator SpawnAI()
    {
        while (canSpawn)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Quaternion spawnRotation = Quaternion.Euler(0, spawnRotationY, 0);

            if (aiCarPrefabs.Length > 0) 
            {
                int randomIndex = Random.Range(0, aiCarPrefabs.Length);
                Instantiate(aiCarPrefabs[randomIndex], spawnPosition, spawnRotation);
            }

            float spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        return new Vector3(randomCircle.x, 0, randomCircle.y) + transform.position;
    }

    // Menampilkan garis hijau hanya di Scene View
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) // Garis hijau hanya muncul di Scene View
        {
            Gizmos.color = Color.green;
            Vector3 direction = Quaternion.Euler(0, spawnRotationY, 0) * Vector3.forward;
            Gizmos.DrawLine(transform.position, transform.position + direction * 5f);
        }
    }
}
