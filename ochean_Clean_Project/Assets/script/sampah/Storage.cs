using UnityEngine;

public class Storage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBoat playerBoat = other.GetComponent<PlayerBoat>();
            if (playerBoat != null)
            {
                playerBoat.TransferTrashToStorage();
            }
        }
    }
}
