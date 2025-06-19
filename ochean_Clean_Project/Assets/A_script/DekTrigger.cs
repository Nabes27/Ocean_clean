using UnityEngine;

public class DekTrigger : MonoBehaviour
{
    public GameObject dekUI; // Canvas Dek
    public GameObject promptUI; // Teks "Tekan Spasi untuk masuk dek"
    private bool isPlayerNearby = false;
    private bool isInDek = false;

    private PlayerBoat playerBoat;
    private Rigidbody playerRb;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            promptUI.SetActive(true);

            playerBoat = other.GetComponent<PlayerBoat>();
            playerRb = other.GetComponent<Rigidbody>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            promptUI.SetActive(false);

            if (isInDek)
                ToggleDek(); // Auto keluar dari dek kalau player pergi
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            ToggleDek();
        }
    }

    void ToggleDek()
    {
        isInDek = !isInDek;

        dekUI.SetActive(isInDek);
        promptUI.SetActive(!isInDek);

        if (isInDek)
        {
            // Stop player movement
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;

            // Disable acceleration
            playerBoat.enabled = false;
        }
        else
        {
            // Enable player movement
            playerBoat.enabled = true;
        }
    }
}
