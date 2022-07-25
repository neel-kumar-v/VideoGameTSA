using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateVolumeText : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider master;
    public Slider music;
    public Slider sfx;
    public Text masterText;
    public Text musicText;
    public Text sfxText;
    // Update is called once per frame
    void Update()
    {
        masterText.text = ((master.value + 80f) * 0.0125f).ToString("#%");
        musicText.text = ((music.value + 80f) * 0.0125f).ToString("#%");
        sfxText.text = ((sfx.value + 80f) * 0.0125f).ToString("#%");
    }
}
