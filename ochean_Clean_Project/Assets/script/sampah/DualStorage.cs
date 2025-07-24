
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DualStorage : MonoBehaviour
{
    public enum StorageID { A, B }
    public StorageID storageID;

    public int maxCapacity = 8;
    private int currentTrash = 0;

    [Header("UI")]
    public TMPro.TextMeshProUGUI storageUIText;
    public UnityEngine.UI.Image storageUIBar;

    [Tooltip("GameObject yang ingin dinonaktifkan setelah Storage Yang di pilih penuh (boleh kosong)")]
    public List<GameObject> objectsToDisableAtEnd;

    [Tooltip("GameObject yang ingin diaktifkan setelah Storage Yang di pilih penuh (boleh kosong)")]
    public List<GameObject> objectsToEnableAtEnd;

    [Header("Pindahkan GameObject ke bawah (Y = -30) Setelah Cerita Selesai")]
    public List<GameObject> objectsToMoveDownAfterEnd;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBoat playerBoat = other.GetComponent<PlayerBoat>();
            if (playerBoat != null)
            {
                int transferred = TransferFromPlayer(playerBoat.GetCurrentTrash(), playerBoat);
                playerBoat.RemoveTrash(transferred); // Hapus dari kapal
            }
        }
    }

    public int TransferFromPlayer(int amountFromPlayer, PlayerBoat playerBoat)
    {
        int available = maxCapacity - currentTrash;
        int toTransfer = Mathf.Min(available, amountFromPlayer);
        currentTrash += toTransfer;
        UpdateStorageUI();

        // Panggil ScoreManager untuk UI jika perlu
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.UpdateStorageUI(storageID, currentTrash, maxCapacity);
        }

        // Jika storage sudah penuh
        if (currentTrash >= maxCapacity)
        {
            foreach (GameObject go in objectsToDisableAtEnd)
            {
                if (go != null) go.SetActive(false);
            }

            foreach (GameObject go in objectsToEnableAtEnd)
            {
                if (go != null) go.SetActive(true);
            }

            foreach (GameObject go in objectsToMoveDownAfterEnd)
            {
                if (go != null)
                {
                    Vector3 pos = go.transform.position;
                    pos.y -= 30f;
                    go.transform.position = pos;
                }
            }
        }


        return toTransfer;
    }

    public void UpdateStorageUI()
    {
        if (storageUIText != null)
            storageUIText.text = $"Storage {storageID}: {currentTrash}/{maxCapacity}";

        if (storageUIBar != null)
            storageUIBar.fillAmount = (float)currentTrash / maxCapacity;
    }
}