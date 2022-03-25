using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Dropdown country;
    public static string countryString;
    public static bool controller;
    public Toggle toggle;

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
