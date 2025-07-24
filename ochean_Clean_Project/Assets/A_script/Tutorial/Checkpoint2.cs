using UnityEngine;

public class Checkpoint2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ArrowPointerToCheckpoint arrow = FindObjectOfType<ArrowPointerToCheckpoint>();
            if (arrow != null)
            {
                arrow.NotifyCheckpointReached();
            }

            Destroy(gameObject);
        }
    }
}
