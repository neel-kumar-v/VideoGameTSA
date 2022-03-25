using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCurrentlyUsedText : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;
    public Health playerHealth;
    public Text text;

    public GameObject weapons;
    public GameObject health;
    public GameObject armor;

    // Update is called once per frame
    void Update()
    {
        string t = "";
        if(weapons.activeInHierarchy) {
            t = "Current Weapon: " + gameManager.weapon.gunName;
        } else if (health.activeInHierarchy) {
            t = "Current Health Regeneration Speed: " + playerHealth.regenSpeed.ToString();
        } else if (armor.activeInHierarchy) {
            t = "Current Armor: " + playerHealth.startArmor.ToString();
        }
        text.text = t;
    }

}
