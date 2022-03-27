using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject mainGameScreen;
    public GameObject mainGame;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bonusText;
    public GameObject winScreen;
    public TextMeshProUGUI winScoreText;
    [SerializeField] int score = 0;

    public Resource[] resources;
    [SerializeField] int bonusIndex = 0;
    [SerializeField] int bonusStrength = 1;
    private Dictionary<GameObject, bool> allGoalsMet;

    private void Start()
    {
        score = 0;
    }

    public void StartGame(int difficulty)
    {
        titleScreen.SetActive(false);
        mainGame.SetActive(true);
        mainGameScreen.SetActive(true);
        UpdateScore(false);
        foreach (Resource res in resources)
        {
            res.InitializeGoals(difficulty);
        }
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
            mainGameScreen.SetActive(false);
            mainGame.SetActive(false);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
