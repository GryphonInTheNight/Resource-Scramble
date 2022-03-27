using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Resource : MonoBehaviour
{
    [SerializeField] private int number = 0;
    [SerializeField] private int baseIncrease = 1;
    [SerializeField] private int multiplier = 1;
    [SerializeField] private int metGoal = 0;


    public TextMeshProUGUI numberText;
    public TextMeshProUGUI baseText;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI goalText;
    public GameObject goals;
    private SortedList<int, Goal> myGoals;
    private IList<int> goalsToMeet;
    private GameManager gm;

    private void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        numberText.text = gameObject.name + ": " + number;
        baseText.text = "Base: " + baseIncrease;
        multiplierText.text = "Multiplier: " + multiplier;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeGoals(int numOfGoals)
    {
        Goal[] unsortedGoals = goals.GetComponentsInChildren<Goal>();
        myGoals = new SortedList<int, Goal>(unsortedGoals.Length);
        foreach (Goal goal in unsortedGoals)
            myGoals.Add(goal.myGoal, goal);
        goalsToMeet = myGoals.Keys;
        UpdateNumberText();
        for (int i = numOfGoals; i < myGoals.Count; i++)
        {
            myGoals[goalsToMeet[i]].gameObject.SetActive(false);
        }
    }

    public void IncrementNumber()
    {
        number += baseIncrease * multiplier;
        if (metGoal < myGoals.Count && number >= goalsToMeet[metGoal])
        {
            AddMultiplier(myGoals[goalsToMeet[metGoal]].bonusMultiplier);
            metGoal++;
        }
        UpdateNumberText();
        gm.UpdateScore(true);
    }

    //Attempts to lower the number, i.e. buys with the currency.  returns true if successful.
    public bool LowerNumber(int delta)
    {
        if (number < delta)
            return false;
        number -= delta;
        UpdateNumberText();
        return true;
    }

    private void UpdateNumberText()
    {
        numberText.text = gameObject.name + ": " + number;
        if (metGoal < myGoals.Count && myGoals[goalsToMeet[metGoal]].gameObject.activeInHierarchy)
            goalText.text = "Next Goal: " + number + "/" + goalsToMeet[metGoal] + "\nx" + myGoals[goalsToMeet[metGoal]].bonusMultiplier;
        else
        {
            goalText.text = "Next Goal: " + number + "/" + goalsToMeet[metGoal - 1];
                gm.CheckIfWon(gameObject, (goalsToMeet[metGoal - 1] <= number));
        }
        for (int i = 0; i < metGoal; i++)
            if (number >= goalsToMeet[i])
                myGoals[goalsToMeet[i]].gameObject.GetComponent<TextMeshProUGUI>().text = "O";
            else myGoals[goalsToMeet[i]].gameObject.GetComponent<TextMeshProUGUI>().text = "X";
    }

    public void AddBaseIncrease(int increase)
    {
        baseIncrease += increase;
        baseText.text = "Base: " + baseIncrease;
    }

    public void AddMultiplier(int factorIncrease)
    {
        multiplier *= factorIncrease;
        multiplierText.text = "Multiplier: " + multiplier;
    }

    public void RemoveMultiplier(int factorDecrease)
    {
        multiplier /= factorDecrease;
        multiplierText.text = "Multiplier: " + multiplier;
    }
}
