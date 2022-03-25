using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletsStatsParser : MonoBehaviour
{
    public BulletStats bulletStats;

    [Header("Weapon")]
    public Text gunName;
    public Text price;

    [Header("Stats")]
    public Text dmg;
    public Text reload;
    public Text bSpeed;
    public Text pierce;
    public Text explode;
    public Text eDmg;
    public Text eRad;
    public Text weight;

    [Header("Info")]
    public Text info;

    // This just sets all the UI to displaying the correct gun information
    void Start() {
        Bullet b = bulletStats.bullet;
        // Accessible Attributes
            // float reload;
            // float speed;
            // float damage;
            // bool explode;
            // float explosionDamage;
            // float explosionRadius;
            // float weight;

        gunName.text = bulletStats.gunName;
        price.text = "$" + bulletStats.price.ToString();

        dmg.text = "Damage: " + b.damage.ToString();
        reload.text = "Reload Time: " + b.reload.ToString();
        bSpeed.text = "Bullet Speed: " + b.speed.ToString();
        pierce.text = "Pierce: " + b.pierce.ToString();
        
        explode.text = "Explosive: " + (b.explosive ? "Yes" : "No");

        // Only show the explosive stats if the bullet is explosive
        if(b.explosive) {
            eDmg.text = "Explosive Damage: " + b.explosionDamage.ToString();
            eRad.text = "Explosion Radius: " + b.explosionRadius.ToString();
        } else {
            eDmg.gameObject.SetActive(false);
            eRad.gameObject.SetActive(false);
        }

        weight.text = "Weight: " + b.weight.ToString();

        info.text = bulletStats.gunInfo;
    }

    
}
