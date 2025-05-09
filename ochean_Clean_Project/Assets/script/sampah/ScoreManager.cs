using UnityEngine;
using TMPro;
using UnityEngine.UI; // Tambahkan namespace untuk UI Image

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI trashText;   // UI untuk sampah di kapal
    public TextMeshProUGUI storageText; // UI untuk storage sampah
    public Image storageBar;            // UI bar untuk storage sampah
    public Image DekKapal;


    public int maxStorageCapacity = 100; // Kapasitas maksimum storage
    private int currentStorage = 0;      // Jumlah sampah di storage

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateTrashUI(int currentTrash, int maxTrashCapacity)
    {
        trashText.text = "Sampah: " + currentTrash + "/" + maxTrashCapacity;
        if (DekKapal != null)
        {
            DekKapal.fillAmount = (float)currentTrash / maxTrashCapacity;
        }

    }

    public void AddToStorage(int amount)
    {
        currentStorage = Mathf.Clamp(currentStorage + amount, 0, maxStorageCapacity);
        UpdateStorageUI();
    }

    private void UpdateStorageUI()
    {
        // Perbarui teks storage
        storageText.text = "Storage: " + currentStorage + "/" + maxStorageCapacity;

        // Perbarui fill amount pada bar
        if (storageBar != null)
        {
            storageBar.fillAmount = (float)currentStorage / maxStorageCapacity;
        }
    }
}