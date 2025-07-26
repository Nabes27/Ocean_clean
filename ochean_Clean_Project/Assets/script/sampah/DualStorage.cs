using UnityEngine;
using System.Collections.Generic;

public class DualStorage : MonoBehaviour
{
    public enum StorageID { A, B }
    public StorageID storageID;

    [Header("Aksi Saat Storage Penuh")]
    public List<GameObject> objectsToDisableAtEnd;
    public List<GameObject> objectsToEnableAtEnd;
    public List<GameObject> objectsToMoveDownAfterEnd;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBoat playerBoat = other.GetComponent<PlayerBoat>();
            if (playerBoat != null)
            {
                int jumlahSampah = playerBoat.GetCurrentTrash();

                // Kirim ke ScoreManager, yang akan mengatur kapasitas & UI
                int diterima = ScoreManager.instance.TransferTrashToStorage(storageID, jumlahSampah);
                playerBoat.RemoveTrash(diterima);

                // Kalau sudah penuh, jalankan aksi
                if (ScoreManager.instance.IsStorageFull(storageID))
                {
                    foreach (var go in objectsToDisableAtEnd)
                        if (go != null) go.SetActive(false);

                    foreach (var go in objectsToEnableAtEnd)
                        if (go != null) go.SetActive(true);

                    foreach (var go in objectsToMoveDownAfterEnd)
                        if (go != null) go.transform.position += new Vector3(0, -30f, 0);
                }
            }
        }
    }
}
