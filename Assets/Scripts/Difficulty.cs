using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    public int difficulty;

    public void SetDifficulty()
    {
        Settings.Instance.Difficulty = difficulty;
        Settings.Instance.StartGame();
    }
}
