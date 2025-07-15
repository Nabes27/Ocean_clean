using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EmotionSprite {
    public string emotionName;
    public Sprite sprite;
}

[System.Serializable]
public class CharacterData {
    public string characterName;
    public List<EmotionSprite> emotions;
    public Vector2 uiPosition;

    public Sprite GetEmotionSprite(string emotion) {
        var found = emotions.Find(e => e.emotionName == emotion);
        return found != null ? found.sprite : null;
    }
}
