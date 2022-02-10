using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    void Awake() {
        foreach (Sound s in sounds)
        {
            // For all the objects we want to put sounds on, put the correct component and clip on it
            s.src = gameObject.AddComponent<AudioSource>();
            s.src.clip = s.clip;
        }
    }
}
