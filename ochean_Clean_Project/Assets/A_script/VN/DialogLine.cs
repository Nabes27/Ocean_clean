using UnityEngine;

[System.Serializable]
public class DialogLine {
    public string characterName;
    public string emotion; 
    
    [TextArea(2, 5)]
    public string line;
}
