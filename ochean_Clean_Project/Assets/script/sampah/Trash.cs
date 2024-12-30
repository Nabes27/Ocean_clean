using UnityEngine;

public class Trash : MonoBehaviour
{
    public int points = 1; // Nilai sampah yang didapat player (dalam hal ini, 1)

    // Fungsi ini dipanggil ketika objek memasuki trigger
    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang masuk adalah Player
        if (other.CompareTag("Player"))
        {
            // Tambahkan skor ke UI dan destroy objek sampah
            ScoreManager.instance.AddTrash(points);
            Destroy(gameObject);
        }
    }
}
