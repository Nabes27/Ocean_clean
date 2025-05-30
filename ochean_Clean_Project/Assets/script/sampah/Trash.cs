using UnityEngine;

public class Trash : MonoBehaviour
{
    public int points = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBoat playerBoat = other.GetComponent<PlayerBoat>();
            if (playerBoat != null && playerBoat.CanCollectTrash())
            {
                playerBoat.CollectTrash(points);
                Destroy(gameObject);
            }
        }
    }
}
