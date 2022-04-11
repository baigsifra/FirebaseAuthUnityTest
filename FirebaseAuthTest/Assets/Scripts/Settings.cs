using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;
    float currentVolume;
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
    }

    public void SetVolume(float volume) {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
        currentVolume = volume;
    }

    public void SaveSettings() {
        PlayerPrefs.SetFloat("VolumePreference", currentVolume);
    }
}
