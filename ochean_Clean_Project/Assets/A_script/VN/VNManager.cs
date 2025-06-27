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

    void Start() {
        ShowLine();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
            if (isTyping) {
                StopAllCoroutines();
                dialogText.text = currentText;
                isTyping = false;
            } else {
                currentLineIndex++;
                if (currentLineIndex < storyLines.Count) {
                    ShowLine();
                } else {
                    Debug.Log("End of story.");
                }
            }
        }
    }

    void ShowLine() {
        DialogLine line = storyLines[currentLineIndex];
        CharacterData character = characters.Find(c => c.characterName == line.characterName);

        if (character != null) {
            characterImage.sprite = character.characterSprite;
            characterImage.rectTransform.anchoredPosition = character.uiPosition;
            characterNameText.text = character.characterName;
        }

        StartCoroutine(TypeLine(line.line));
    }

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
