using UnityEngine;

[System.Serializable]
public class DialogLine {
    public string characterName;
    [TextArea(2, 5)]
    public string line;
}
