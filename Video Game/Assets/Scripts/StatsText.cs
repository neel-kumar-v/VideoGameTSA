using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsText : MonoBehaviour
{
    public Text countries;
    public Text goldEarned;
    public Text goldSpent;
    public Text upgradesBought;


    public void Update() {
        countries.text = "Countries Beaten: " + Stats.kills.ToString();
        goldSpent.text = "Gold Spent: " + Stats.goldSpent.ToString();
        goldEarned.text = "Gold Earned: " + Stats.earnedGold.ToString();
        upgradesBought.text = "Upgrades Bought: " + Stats.upgradesBought.ToString();
    }
}
