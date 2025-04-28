using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Fungsi untuk memuat scene berdasarkan Build Index
    public void ChangeSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Fungsi untuk keluar dari aplikasi (opsional)
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed"); // Ini hanya muncul di editor, tidak pada build game
    }
}
