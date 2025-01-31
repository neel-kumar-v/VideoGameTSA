using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Health : MonoBehaviour
{
    [Header("Paramaters")]
    public float health;
    public float maxHealth;
    public float armor;
    public float maxArmor;
    public float regenSpeed;
    public bool canRegen;

    [Header("Unity Setup")]
    public bool runUI = true;
    public Slider healthBar;
    public Image colorBar;
    public Gradient color;
    public Slider armorBar;
    public float smoothStep;
    public Volume volume;
    public CameraShake shake;
    [Space(10)]
    public GameObject deadFx;
    [Space(10)]
    public bool isPlayer;

    public bool dead;

    PlayerController controller;
    Renderer rend;
    AIMovement enemyController;
    SphereCollider sphereCollider;
    Renderer[] cylinderRends;

    float savedArmor;
    [HideInInspector] public float startArmor;

    // Start is called before the first frame update
    void Start()
    {
        startArmor = armor;
        controller = GetComponent<PlayerController>();
        enemyController = GetComponent<AIMovement>();
        rend = GetComponent<Renderer>();
        sphereCollider = GetComponent<SphereCollider>();
        StartCoroutine(LateStart()); //TBH idk why its running 1 second later, don't question it
    }

    public IEnumerator LateStart() {
        yield return new WaitForSeconds(1f);
        cylinderRends = GetComponentsInChildren<Renderer>();
    }


    public void ResetHealth() {
        health = maxHealth; 
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        colorBar.color = color.Evaluate(healthBar.value);

        armor = startArmor;
        armorBar.maxValue = maxArmor;
        armorBar.value = armor;
    }

    // Update is called once per frame
    void Update()
    {
        // If the renderer has been enabled(this can only be done by the game manager, which does it when a "new" enemy is supposed to come in)
        // This means that we aren't dead
        if(rend.enabled) { 
            if(dead) { // So if we still think we're dead, undo that and also turn everything back on
                SetState(true);
            }
            dead = false; // We aren't dead so make the variable true
        }

        Regen();
        UpdateUI();
    }
    public void ApplyDamage(float damage) {
        if(isPlayer) {StartCoroutine(shake.Shake(0.5f, 0.01f * damage));}
        if(armor <= 0) { // If we don't have armor remaining
            DamageHealth(damage);
            return;
        }
        if(armor < damage) { // If the armor isn't enough to take on the damage
            // subtract the remaining armor from the damage so the damage applied to the health will be less
            damage -= armor;
            armor = 0f; 
            DamageHealth(damage);
        } else if(armor >= damage) { // If the armor is enough to take on the damage
            armor -= damage; // subtract the damage from the armor
            damage = 0f;
            return;
        }
    }

    public void DamageHealth(float damage) {
        health -= damage;
        CheckHealth(); // check if we're dead
        StartCoroutine(PauseRegen());
    }

    public IEnumerator PauseRegen() {
        canRegen = false;
        if(isPlayer) {
            if(!volume.sharedProfile.TryGet<Vignette>(out var vignette)) { vignette = volume.sharedProfile.Add<Vignette>(false); }
            BlendInVignette(0.2f, vignette);
        }
        
        yield return new WaitForSeconds(2f);
        if(isPlayer) {
            if(!volume.sharedProfile.TryGet<Vignette>(out var vignette)) { vignette = volume.sharedProfile.Add<Vignette>(false); }
            BlendOutVignette(0.2f, vignette);
        }
        canRegen = true;
    }

    public IEnumerator BlendInVignette(float duration, Vignette vignette) {
        float elapsed = 0f;
        while (elapsed < duration) {
            vignette.color.value = new Color(Mathf.Lerp(0f, 1f, elapsed / duration), 0f, 0f, 1f);
            vignette.intensity.value = Mathf.Lerp(0.18f, 0.3f, elapsed / duration);
            elapsed += Time.deltaTime;
        }
        yield return null;
    }
    public IEnumerator BlendOutVignette(float duration, Vignette vignette) {
        float elapsed = 0f;
        while (elapsed < duration) {
            vignette.color.value = new Color(Mathf.Lerp(1f, 0f, elapsed / duration), 0f, 0f, 1f);
            vignette.intensity.value = Mathf.Lerp(0.3f, 0.18f, elapsed / duration);
            elapsed += Time.deltaTime;
        }
        yield return null;
    }

    public void CheckHealth() {
        if(health > 0) return;
        dead = true;

        UpdateUI();

        if(!isPlayer) {
            float formula = ((maxHealth + maxHealth * (canRegen ? regenSpeed : 0f)) + maxArmor + startArmor) * 2;
            Stats.OnKill(formula);
        }

        Destroy((GameObject) Instantiate(deadFx, transform.position, transform.rotation), 3f);

        ResetHealth();
        
        SetState(false);
    }


    public void Regen(){
        if(health >= maxHealth) { // if the health is at max, then don't regen
            health = maxHealth;
            return;
        }
        if(canRegen) {
            health += regenSpeed * Time.deltaTime;
        }
    }

    public void UpdateUI() {
        if(!runUI) return;
        if(dead) { // if were dead show the value as 0 in the UI          
            healthBar.value = 0f;
            armorBar.value = 0f;
        }
        // Update the value smoothly with Lerp
        if(healthBar.value != health) healthBar.value = Mathf.Lerp(healthBar.value, health, smoothStep * Time.deltaTime);
        colorBar.color = color.Evaluate(healthBar.value); // The color changes based on the amount of health

        if(armorBar.value != armor) armorBar.value = Mathf.Lerp(armorBar.value, armor, smoothStep * Time.deltaTime);

    }


    public void SetState(bool state) {
        if(enabled) { //idk
            dead = false;
            ResetHealth();
        }
        if(controller != null) controller.enabled = state;
        if(enemyController != null) enemyController.enabled = state;
        rend.enabled = state;
        sphereCollider.enabled = state;

        foreach (Renderer cylinderRend in cylinderRends)
        {      
            cylinderRend.enabled = state;
        }
    }

    

}
