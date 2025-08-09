using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    public Transform player; // drag player kapal
    [Header("Custom Spawn Height")]
    public bool useCustomSpawnY = false;
    public float customY = 2f; // ketinggian manual

    private void Awake()
    {
        if (PlayerPrefs.HasKey("ContinueFromSave") && PlayerPrefs.GetInt("ContinueFromSave") == 1)
        {
            LoadPlayerData();
            PlayerPrefs.SetInt("ContinueFromSave", 0);
        }
    }

    public void SaveGame()
    {
        // Simpan scene
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex);

        // Simpan posisi player
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.position.z);

        // Simpan rotasi
        PlayerPrefs.SetFloat("PlayerRotX", player.eulerAngles.x);
        PlayerPrefs.SetFloat("PlayerRotY", player.eulerAngles.y);
        PlayerPrefs.SetFloat("PlayerRotZ", player.eulerAngles.z);

        // Simpan data storage
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.SaveStorageData();
        }

        PlayerPrefs.Save();
        Debug.Log("Game Saved");
    }

    public void LoadPlayerData()
    {
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        float z = PlayerPrefs.GetFloat("PlayerZ");

        // Kalau pakai custom Y, override nilai dari save
        if (useCustomSpawnY)
        {
            y = customY;
        }

        float rx = PlayerPrefs.GetFloat("PlayerRotX");
        float ry = PlayerPrefs.GetFloat("PlayerRotY");
        float rz = PlayerPrefs.GetFloat("PlayerRotZ");

        player.position = new Vector3(x, y, z);
        player.eulerAngles = new Vector3(rx, ry, rz);

        // Load storage
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.LoadStorageData();
        }

        Debug.Log($"Game Loaded at {player.position}");
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("SavedScene"))
        {
            int sceneToLoad = PlayerPrefs.GetInt("SavedScene");
            PlayerPrefs.SetInt("ContinueFromSave", 1);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.Log("No save found!");
        }
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteKey("SavedScene");
        SceneManager.LoadScene(2);
    }
}
