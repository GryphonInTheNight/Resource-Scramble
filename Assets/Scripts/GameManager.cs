using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject mainGameScreen;
    public GameObject mainGame;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bonusText;
    public GameObject winScreen;
    public TextMeshProUGUI winScoreText;
    public TextMeshProUGUI musicButtonText;
    public TextMeshProUGUI soundButtonText;
    [SerializeField] private int score = 0;
    [SerializeField] private int defDifficulty = 2;

    public Resource[] resources;
    [SerializeField] int bonusIndex = 0;
    [SerializeField] int bonusStrength = 1;
    private Dictionary<GameObject, bool> allGoalsMet;
    private AudioSource clipPlayer;

    private void Awake()
    {
        Instance = this;

        score = 0;
        clipPlayer = gameObject.GetComponent<AudioSource>();
        UpdateScore(false);


        if (Settings.Instance != null)
        {
            foreach (Resource res in resources)
            {
                res.InitializeGoals(Settings.Instance.Difficulty);
            }
            UpdateMusicButtonText(!Settings.Instance.playMusic);
            UpdateSoundButtonText(!Settings.Instance.playSound);
            clipPlayer.mute = !Settings.Instance.playSound;
        }
        else
            foreach (Resource res in resources)
                res.InitializeGoals(defDifficulty);

        allGoalsMet = new Dictionary<GameObject, bool>();
        foreach (Resource res in resources)
            allGoalsMet.Add(res.gameObject, false);
    }

    public void UpdateScore(bool increaseClicks)
    {
        if (increaseClicks)
            score++;
        scoreText.text = "Clicks: " + score;

        if (score % 10 == 0)
        {
            if (bonusIndex < resources.Length)
                resources[bonusIndex].RemoveMultiplier(bonusStrength);
            else
                foreach (Resource res in resources)
                    res.RemoveMultiplier(bonusStrength);
            bonusStrength++;
            bonusIndex = Random.Range(0, resources.Length + 1);
            if (bonusIndex < resources.Length)
            {
                resources[bonusIndex].AddMultiplier(bonusStrength);
                bonusText.text = "Bonus: " + resources[bonusIndex].name + " x" + bonusStrength;
            }
            else
            {
                foreach (Resource res in resources)
                    res.AddMultiplier(bonusStrength);
                bonusText.text = "Bonus: All x" + bonusStrength;
            }

        }
    }

    public void HandleUpgrade(int resTarget, bool shouldIMultiply, int factor)
    {
        if (resTarget >= resources.Length)
        {
            if (shouldIMultiply)
                foreach (Resource res in resources)
                    res.AddMultiplier(factor);
            else
                foreach (Resource res in resources)
                    res.AddBaseIncrease(factor);
        }
        else if (shouldIMultiply)
            resources[resTarget].AddMultiplier(factor);
        else
            resources[resTarget].AddBaseIncrease(factor);

    }

    public void CheckIfWon(GameObject res, bool update)
    {
        allGoalsMet[res] = update;
        bool test = true;
        foreach (GameObject resource in allGoalsMet.Keys)
        {
            test &= allGoalsMet[resource];
        }
        if (test)
        {
            winScreen.SetActive(true);
            winScoreText.text = "You won in " + score + " clicks!";
            mainGame.SetActive(false);
            Settings.Instance.CheckAndAddScore(score);
            Settings.Instance.SaveMyData();
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleMusic()
    {
        if (Settings.Instance != null)
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
        if (Settings.Instance != null)
        {
            UpdateSoundButtonText(!Settings.Instance.ToggleMute());
            clipPlayer.mute = !Settings.Instance.playSound;
        }
        else
        {
            clipPlayer.mute = !clipPlayer.mute;
            UpdateSoundButtonText(clipPlayer.mute);
        }
    }

    private void UpdateSoundButtonText(bool strikethrough)
    {
        if (strikethrough)
            soundButtonText.text = "<s>Sound</s>";
        else
            soundButtonText.text = "Sound";

    }
}
