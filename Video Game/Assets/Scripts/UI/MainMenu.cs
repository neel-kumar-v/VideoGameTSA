using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Dropdown country;
    public static string countryString;
    public static bool controller;
<<<<<<< Updated upstream
    public Toggle toggle;
    public Button play;

    public void Start() {
        play.Select();
    }
=======
>>>>>>> Stashed changes

    public void Play() {
        if(country.captionText.text == "Choose Country") return;
        SceneManager.LoadScene("Game");

    }
    public void UpdateText() {
        countryString = country.captionText.text;
        Debug.Log(countryString);
    }
    
    public void Quit() {
        Application.Quit();
    }

    public void ToggleControls() {
        controller = toggle.isOn;
        Debug.Log(controller);
    }

}
