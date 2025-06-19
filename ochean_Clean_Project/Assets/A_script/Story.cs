using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{
    public List<GameObject> storyPrefabs; // List prefab gambar cerita
    private int currentIndex = 0;
    private GameObject currentInstance;

    void Start()
    {
        if (storyPrefabs.Count > 0)
        {
            ShowStory(currentIndex);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // klik kiri mouse atau tap
        {
            NextStory();
        }
    }

    void ShowStory(int index)
    {
        if (index < storyPrefabs.Count)
        {
            currentInstance = Instantiate(storyPrefabs[index], transform);
        }
    }

    void NextStory()
    {
        if (currentInstance != null)
        {
            Destroy(currentInstance);
        }

        currentIndex++;

        if (currentIndex < storyPrefabs.Count)
        {
            ShowStory(currentIndex);
        }
        else
        {
            Debug.Log("Cerita selesai!");
            // Di sini kamu bisa tambahkan: SceneManager.LoadScene atau aktifkan kontrol player, dll
        }
    }
}
