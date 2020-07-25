using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuSaveFileButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lastPlayedText = default;
    [SerializeField] TextMeshProUGUI currentSceneText = default;
    [SerializeField] List<Image> spellImages = default;

    string saveFileName;

    public void Initialize(SaveFileData data)
    {
        saveFileName = data.FileName;
        foreach (var img in spellImages)
            img.sprite = null;
        lastPlayedText.text = data.LastPlayed;
        currentSceneText.text = data.CurrentSceneName;
        for (int i = 0; i < data.Spells.Count; i++)
            if (data.Spells[i] != null)
                spellImages[i].sprite = data.Spells[i].Icon;
    }

    public string GetSaveFileName()
    {
        return saveFileName;
    }
}
