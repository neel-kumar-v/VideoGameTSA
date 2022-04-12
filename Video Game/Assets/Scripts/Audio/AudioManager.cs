using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    void Awake() {
        foreach (Sound sound in sounds)
        {
            // For all the objects we want to put sounds on, put the correct component and clip on it
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;


            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void Play(string name) {
        foreach (Sound sound in sounds)
        {
            if(name == sound.name) {
                sound.source.Play();
                return;
            }
        }
        Debug.LogWarning("Sound: " + name + " not found!");
    }

    public void PlaySong(string name) {
        bool songFound = false;
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
            if(name == sound.name) {
                songFound = true;
                sound.source.Play();
            }
        }
        if(!songFound) Debug.LogWarning("Sound: " + name + " not found!");
    }
}
