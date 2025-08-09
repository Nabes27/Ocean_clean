using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI trashText;   // UI untuk sampah di kapal
    public TextMeshProUGUI storageText; // UI untuk storage sampah
    public Image storageBar;            // UI bar untuk storage sampah
    public Image DekKapal;


    public int maxStorageCapacity = 100; // Kapasitas maksimum storage
    private int currentStorage = 0;      // Jumlah sampah di storage

    // Tambahkan di atas
    public TextMeshProUGUI storageAText;
    public Image storageABar;
    public TextMeshProUGUI storageBText;
    public Image storageBBar;

    // Tambahkan field di ScoreManager:
    private int storageACurrent = 0;
    private int storageBCurrent = 0;

    public int maxCapacityA = 15;
    public int maxCapacityB = 15;

    [Header("Trigger GameObjects")]
    public List<GameObject> objectsWhenOneFull; // Aktif jika salah satu full
    public List<GameObject> objectsWhenBothFull; // Aktif jika dua-duanya full


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

    // Tambahkan fungsi baru:
    public void UpdateStorageUI(DualStorage.StorageID id, int current, int max)
    {
        switch (id)
        {
            case DualStorage.StorageID.A:
                if (storageAText != null)
                    storageAText.text = $"Storage A: {current}/{max}";
                if (storageABar != null)
                    storageABar.fillAmount = (float)current / max;
                break;
            case DualStorage.StorageID.B:
                if (storageBText != null)
                    storageBText.text = $"Storage B: {current}/{max}";
                if (storageBBar != null)
                    storageBBar.fillAmount = (float)current / max;
                break;
        }
    }

    public int TransferTrashToStorage(DualStorage.StorageID id, int amount)
    {
        int max = (id == DualStorage.StorageID.A) ? maxCapacityA : maxCapacityB;
        int current = (id == DualStorage.StorageID.A) ? storageACurrent : storageBCurrent;

        int available = max - current;
        int toTransfer = Mathf.Min(available, amount);
        current += toTransfer;

        if (id == DualStorage.StorageID.A)
            storageACurrent = current;
        else
            storageBCurrent = current;

        // Update UI
        UpdateStorageUI(id, current, max);

        // Cek trigger storage
        CheckStorageTriggers();

        return toTransfer;
    }


    public bool IsStorageFull(DualStorage.StorageID id)
    {
        if (id == DualStorage.StorageID.A)
            return storageACurrent >= maxCapacityA;
        else
            return storageBCurrent >= maxCapacityB;
    }



    // ======== SAVE & LOAD STORAGE DATA ========
    public void SaveStorageData()
    {
        PlayerPrefs.SetInt("StorageA_Current", storageACurrent);
        PlayerPrefs.SetInt("StorageB_Current", storageBCurrent);
        PlayerPrefs.Save();
        Debug.Log($"Storage Saved: A={storageACurrent}, B={storageBCurrent}");
    }

    public void LoadStorageData()
    {
        if (PlayerPrefs.HasKey("StorageA_Current") && PlayerPrefs.HasKey("StorageB_Current"))
        {
            storageACurrent = PlayerPrefs.GetInt("StorageA_Current");
            storageBCurrent = PlayerPrefs.GetInt("StorageB_Current");

            // Update UI setelah load
            UpdateStorageUI(DualStorage.StorageID.A, storageACurrent, maxCapacityA);
            UpdateStorageUI(DualStorage.StorageID.B, storageBCurrent, maxCapacityB);

            Debug.Log($"Storage Loaded: A={storageACurrent}, B={storageBCurrent}");
        }
    }

    private void CheckStorageTriggers()
    {
        bool isAFull = storageACurrent >= maxCapacityA;
        bool isBFull = storageBCurrent >= maxCapacityB;

        bool bothFull = isAFull && isBFull;
        bool oneFullOnly = (isAFull ^ isBFull); // XOR: hanya satu yang penuh

        // Aktifkan hanya jika salah satu penuh (bukan dua-duanya)
        foreach (var obj in objectsWhenOneFull)
        {
            if (obj != null) obj.SetActive(oneFullOnly);
        }

        // Aktifkan hanya jika dua-duanya penuh
        foreach (var obj in objectsWhenBothFull)
        {
            if (obj != null) obj.SetActive(bothFull);
        }
    }





}