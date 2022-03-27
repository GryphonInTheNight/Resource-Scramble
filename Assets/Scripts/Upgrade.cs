using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Upgrade : MonoBehaviour
{
    /*public enum UpgradeChoice
    {
        BlueBase,
        BlueMult,
        GreenBase,
        GreenMult,
        BrownBase,
        BrownMult,
        RedBase,
        RedMult,
        AllBase,
        AllMult
    }*/
    public Resource myCurrency;
    public TextMeshProUGUI upgradeText;
    public List<Vector3Int> upgradeList;
    private int currentUpgrade = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUIText();
    }

    public void TryToBuy()
    {
        if (currentUpgrade < upgradeList.Count && myCurrency.LowerNumber(upgradeList[currentUpgrade].z))
        {
            int myType = (int)(upgradeList[currentUpgrade].x / 2);
            int multiply = upgradeList[currentUpgrade].x % 2;
            int factor = upgradeList[currentUpgrade].y;
            if (multiply == 0)
                GameManager.Instance.HandleUpgrade(myType, false, factor);
            else
                GameManager.Instance.HandleUpgrade(myType, true, factor);
            currentUpgrade++;
            UpdateUIText();
            GameManager.Instance.UpdateScore(true);
        }
    }

    private void UpdateUIText()
    {
        if (currentUpgrade < upgradeList.Count)
        {
            int myType = (int)(upgradeList[currentUpgrade].x / 2);
            int multiply = upgradeList[currentUpgrade].x % 2;
            int factor = upgradeList[currentUpgrade].y;
            int cost = upgradeList[currentUpgrade].z;
            if (myType < GameManager.Instance.resources.Length)
                if (multiply == 0)
                    upgradeText.text = GameManager.Instance.resources[myType].gameObject.name +
                        "\nBase +" + factor + "\nCost:\n" + cost;
                else
                    upgradeText.text = GameManager.Instance.resources[myType].gameObject.name +
                        " x" + factor + "\n\nCost:\n" + cost;
            else if (multiply == 0)
                upgradeText.text = "All\nBase +" + factor + "\nCost:\n" + cost;
            else
                upgradeText.text = "All x" + factor + "\n\nCost:\n" + cost;

        }
        else
            upgradeText.text = "Sold Out!";
    }
}
