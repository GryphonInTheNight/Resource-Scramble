using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    public TextMeshProUGUI musicButtonText;
    public TextMeshProUGUI soundButtonText;
    public TextMeshProUGUI highScoresText;

    private void Awake()
    {
        UpdateMusicButtonText(!Settings.Instance.playMusic);
        UpdateSoundButtonText(!Settings.Instance.playSound);
    }

    public void QuitButton()
    {
        Settings.Instance.QuitGame();
    }

    public void ToggleMusic()
    {
        UpdateMusicButtonText(!Settings.Instance.ToggleMusic());
    }

    private void UpdateMusicButtonText(bool strikethrough)
    {
        if (strikethrough)
            musicButtonText.text = "<s>Music</s>";
        else
            musicButtonText.text = "Music";
    }

    public void ToggleMute()
    {
        UpdateSoundButtonText(!Settings.Instance.ToggleMute());
    }

    private void UpdateSoundButtonText(bool strikethrough)
    {
        if (strikethrough)
            soundButtonText.text = "<s>Sound</s>";
        else
            soundButtonText.text = "Sound";

    }

    public void ShowHighScores(int targetDifficulty)
    {
        highScoresText.gameObject.SetActive(true);
        highScoresText.text = Settings.Instance.GetScoreboard(targetDifficulty);
    }
}
