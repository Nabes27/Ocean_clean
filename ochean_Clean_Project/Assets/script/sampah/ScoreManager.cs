using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;   // Instance dari ScoreManager
    public TextMeshProUGUI trashText;      // Referensi ke TextMeshPro untuk UI
    private int trashCount = 0;             // Jumlah sampah yang terkumpul

    private void Awake()
    {
        // Pastikan hanya ada satu instance dari ScoreManager
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Fungsi untuk menambahkan sampah ke skor
    public void AddTrash(int amount)
    {
        trashCount += amount;
        UpdateTrashUI();
    }

    // Update UI dengan jumlah sampah yang terkumpul
    private void UpdateTrashUI()
    {
        trashText.text = "Sampah: " + trashCount.ToString(); // Menampilkan jumlah sampah
    }
}
