using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandomizerMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public int songCount;
    void Start()
    {
        int randomSongIndex = Random.Range(1, songCount + 1);
        string randomSongName = "Theme" + randomSongIndex.ToString();
        FindObjectOfType<AudioManager>().Play(randomSongName);
    }
}
