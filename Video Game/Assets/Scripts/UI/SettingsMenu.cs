using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;

    public Slider mainSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private float velocity = 0f;
    private float velocity1 = 0f;
    private float velocity2 = 0f;
    public float smoothSpeed;

    [Space(10)]
    public Dropdown resolutionsDropdown;

    private Resolution[] resolutions;

    public void Start() {
        SetMainVolume(mainSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();

        List<string> resolutionNames = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionName = resolutions[i].width + " x " + resolutions[i].height;
            resolutionNames.Add(resolutionName);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        resolutionsDropdown.AddOptions(resolutionNames);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }

    public void SetMainVolume(float volume) {
        mainMixer.SetFloat("MainVolume", volume);
    }
    public void SetMusicVolume(float volume) {
        mainMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume) {
        mainMixer.SetFloat("SFXVolume", volume);
    }

    public void SmoothReset(float volume) {
        StartCoroutine(SmoothResetCoroutine(volume));
    }

    public IEnumerator SmoothResetCoroutine(float volume) {
        float timeElapsed = 0f;
        float duration = 2f;
        while(timeElapsed <= duration) {  
            mainSlider.value = Mathf.SmoothDamp(mainSlider.value, volume, ref velocity, smoothSpeed);
            musicSlider.value = Mathf.SmoothDamp(musicSlider.value, volume, ref velocity1, smoothSpeed);
            sfxSlider.value = Mathf.SmoothDamp(sfxSlider.value, volume, ref velocity2, smoothSpeed);
            timeElapsed += Time.deltaTime;
        }
        yield return null;
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}
