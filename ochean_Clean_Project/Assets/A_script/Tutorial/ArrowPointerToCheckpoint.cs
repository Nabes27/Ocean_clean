using UnityEngine;

public class ArrowPointerToCheckpoint : MonoBehaviour
{
    public LayerMask checkpointLayer;
    public float maxDetectDistance = 100f;
    public int maxCheckpoints = 3;

    [Header("GameObject Setelah Selesai")]
    public GameObject objectToDisableWhenDone;
    public GameObject objectToEnableWhenDone;

    private int checkpointsReached = 0;
    private Transform targetCheckpoint;

    private void Update()
    {
        if (checkpointsReached >= maxCheckpoints)
        {
            if (objectToDisableWhenDone != null)
                objectToDisableWhenDone.SetActive(false);

            if (objectToEnableWhenDone != null)
                objectToEnableWhenDone.SetActive(true);

            Destroy(gameObject);
            return;
        }

        targetCheckpoint = FindNearestCheckpoint();

        if (targetCheckpoint != null)
        {
            Vector3 dir = targetCheckpoint.position - transform.position;
            dir.y = 0f;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void NotifyCheckpointReached()
    {
        checkpointsReached++;
    }

    Transform FindNearestCheckpoint()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, maxDetectDistance, checkpointLayer);
        float closestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (Collider col in colliders)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                nearest = col.transform;
            }
        }

        return nearest;
    }
}
