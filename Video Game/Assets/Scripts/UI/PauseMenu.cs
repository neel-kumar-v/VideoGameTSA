using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject ui;
    public GameObject shopPrompt;
    public GameObject shop;
    public GameObject pauseButton;
    public GameObject blur;
    public Button play;
    public PlayerController isCont;
    



    bool wasShopOpen;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !ShopMenu.inShop && !shopPrompt.activeInHierarchy) { // If an input was pressed and we arent in the shop, pause
            Toggle();
        } else if (isCont.isController && Input.GetButtonDown("Menu") && !ShopMenu.inShop && !shopPrompt.activeInHierarchy) {
            Toggle();
        }
    }

    public void Toggle() {
        // paused is still set to old state here
        // Ex: Unpaused --> Paused | comments on lins 29-32 show result of example
        blur.SetActive(!paused); // Blur turns on
        if (!paused) {
            play.Select();
        }
        ui.SetActive(!paused); // UI is opened
        pauseButton.SetActive(paused); // Pause button goes away
        Time.timeScale = paused ? 1f : 0f;  // Time is stopped
        // now set paused to new state
        paused = !paused;
    }

    public void LoadMenu() {
        SceneManager.LoadScene("Main Menu");
    } 

    public void Quit() {
        Application.Quit();
    }
}
