using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public static int kills;
    public int startKills = 0;
    public int startingGold;
    public static int savedGold;
    public static int earnedGold;
    public static int gold;
    public static int upgradesBought;
    public static int goldSpent;

    public GameObject goldText;
    Text gText;
    bool counting;

    void Start()
    {
        kills = startKills;
        gText = goldText.GetComponent<Text>();
        gold = startingGold;
        gText.text = gold.ToString();
        savedGold = startingGold;
        counting = false;
    }
    void Update() {
        int tGold = int.Parse(gText.text);
        if(tGold != gold && !counting) {
            if(Mathf.Abs(tGold - gold) < 50) {
                gText.text = gold.ToString();
                savedGold = gold;
                return;
            }
            StartCoroutine(Anim(1f));
        }
    }
    public static void OnKill(float num) {
        num = Mathf.Ceil(num);
        kills++;
        savedGold = gold;
        gold += (int) num;
        earnedGold += (int) num;
        // animText.GetComponent<Text>().text = "+" + num.ToString();
        // StartCoroutine(Anim(0.5f));
    }
    public IEnumerator Anim(float time) {
        counting = true;
        while(savedGold < gold) {
            savedGold += gold/100;
            gText.text = savedGold.ToString();
            yield return new WaitForSeconds(time * 0.01f);
        }
        while(savedGold > gold) {
            savedGold -= gold/80;
            gText.text = savedGold.ToString();
            yield return new WaitForSeconds(time * 0.01f);
        }
        counting = false;
        savedGold = gold;
    }

    public static void Spend(float num) {
        num = Mathf.Ceil(num);
        upgradesBought++;
        goldSpent += (int) num;
        savedGold = gold;
        gold -= (int) num;
    }

}
