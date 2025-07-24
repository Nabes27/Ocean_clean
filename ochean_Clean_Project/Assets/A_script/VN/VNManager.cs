
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class VNManager : MonoBehaviour {
    [Header("UI References")]
    public Image characterImage;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogText;

    [Header("Characters")]
    public List<CharacterData> characters;

    [Header("Story")]
    public List<DialogLine> storyLines;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private string currentText = "";


    [Tooltip("GameObject yang ingin dinonaktifkan setelah cerita selesai (boleh kosong)")]
    public List<GameObject> objectsToDisableAtEnd;


    [Header("GameObjects yang Diaktifkan Setelah Cerita Selesai")]
    public List<GameObject> objectsToActivateAtEnd;

    [Tooltip("Berapa banyak objek yang ingin diaktifkan saat cerita selesai (0 = tidak aktifkan apapun)")]
    public int numberOfObjectsToActivate = 0;



    [Header("Pindahkan GameObject ke bawah (Y = -30) Setelah Cerita Selesai")]
    public List<GameObject> objectsToMoveDownAfterEnd;


    void Start() {
        ShowLine();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogText.text = currentText;
                isTyping = false;
            }
            else
            {
                currentLineIndex++;
                if (currentLineIndex < storyLines.Count)
                {
                    ShowLine();
                }
                //
                else
                {
                    Debug.Log("End of story.");
                    //
                    foreach (GameObject obj in objectsToDisableAtEnd)
                    {
                        if (obj != null)
                            obj.SetActive(false);
                    }

                    //

                    for (int i = 0; i < numberOfObjectsToActivate && i < objectsToActivateAtEnd.Count; i++)
                    {
                        if (objectsToActivateAtEnd[i] != null)
                        {
                            objectsToActivateAtEnd[i].SetActive(true);
                        }
                    }

                    //
                    // Pindahkan GameObject corrupt zone ke bawah agar trigger tidak aktif
                    foreach (GameObject go in objectsToMoveDownAfterEnd)
                    {
                        if (go != null)
                        {
                            Vector3 newPos = go.transform.position;
                            newPos.y = -30f;
                            go.transform.position = newPos;
                        }
                    }

                    //
                }

            }
        }
    }

    //
    void ShowLine() {
        DialogLine line = storyLines[currentLineIndex];
        CharacterData character = characters.Find(c => c.characterName == line.characterName);

        if (character != null) {
            // Ambil sprite berdasarkan emosi
            Sprite emotionSprite = character.GetEmotionSprite(line.emotion);
            if (emotionSprite != null) {
                characterImage.sprite = emotionSprite;
            } else {
                Debug.LogWarning($"Emotion '{line.emotion}' not found for character '{line.characterName}'");
            }

            characterImage.rectTransform.anchoredPosition = character.uiPosition;
            characterNameText.text = character.characterName;
        }

        StartCoroutine(TypeLine(line.line));
    }

    // 


    IEnumerator TypeLine(string line) {
        dialogText.text = "";
        isTyping = true;
        currentText = line;

        foreach (char c in line) {
            dialogText.text += c;
            yield return new WaitForSeconds(0.02f); // Efek mengetik
        }

        isTyping = false;
    }
}