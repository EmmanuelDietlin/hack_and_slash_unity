using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parameters : MonoBehaviour
{
    [SerializeField] private GameObject mainVolumeSlider;
    [SerializeField] private GameObject effectVolumeSlider;
    [SerializeField] private GameObject musicVolumeSlider;
    [SerializeField] private GameObject menuMusic;

    void Start()
    {
        if (!PlayerPrefs.HasKey("MainVolume")) PlayerPrefs.SetFloat("MainVolume", 1);
        mainVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MainVolume");

        if (!PlayerPrefs.HasKey("EffectVolume")) PlayerPrefs.SetFloat("EffectVolume", 1);
        effectVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("EffectVolume");

        if (!PlayerPrefs.HasKey("MusicVolume")) PlayerPrefs.SetFloat("MusicVolume", 1);
        musicVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");

    }

    public void SelectMainVolume()
    {
        PlayerPrefs.SetFloat("MainVolume", mainVolumeSlider.GetComponent<Slider>().value);
        // Debug.Log(PlayerPrefs.GetFloat("MainVolume"));
    }

    public void SelectEffectsVolume()
    {
        PlayerPrefs.SetFloat("EffectVolume", effectVolumeSlider.GetComponent<Slider>().value);
        // Debug.Log(PlayerPrefs.GetFloat("EffectVolume"));
    }

    public void SelectMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.GetComponent<Slider>().value);
        // Debug.Log(PlayerPrefs.GetFloat("MusicVolume"));
        menuMusic.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
    }

}
