using UnityEngine;

public class OceanFollow : MonoBehaviour
{
    // Referensi ke objek player (kapal)
    public Transform player;

    void Update()
    {
        if (player != null)
        {
            // Salin posisi x dan z dari player ke ocean
            transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
        }
    }
}
