using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    public int difficulty;
    public MainMenuUI menu;

    public void SetDifficulty()
    {
        Settings.Instance.Difficulty = difficulty;
        Settings.Instance.StartGame();
    }

    public void ShowHighscores()
    {
        menu.ShowHighScores(difficulty);
    }
}
