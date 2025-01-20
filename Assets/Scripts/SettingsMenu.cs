using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; // Needed for Slider and Toggle

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer sfxMixer;
    public AudioMixer bgmMixer;
    public Slider sfxSlider;
    public Slider bgmSlider;
    public Toggle fullScreenToggle;
    public bool shouldPauseGame = false;
    public GameObject settingsPanel;

    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string FULLSCREEN_KEY = "FullScreen";

    void Start()
    {
        
        // Load settings
        if (PlayerPrefs.HasKey(SFX_VOLUME_KEY))
        {
            float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY);
            sfxSlider.value = sfxVolume;
            sfxMixer.SetFloat("Volume", sfxVolume);
        }

        if (PlayerPrefs.HasKey(BGM_VOLUME_KEY))
        {
            float bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY);
            bgmSlider.value = bgmVolume;
            bgmMixer.SetFloat("Volume", bgmVolume);
        }

        if (PlayerPrefs.HasKey(FULLSCREEN_KEY))
        {
            bool isFullScreen = PlayerPrefs.GetInt(FULLSCREEN_KEY) == 1;
            fullScreenToggle.isOn = isFullScreen;
            Screen.fullScreen = isFullScreen;
        }
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullScreen ? 1 : 0);
    }

    public void SetVolumeSfx(float volume)
    {
        sfxMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
    }

    public void SetVolumeBGM(float volume)
    {
        bgmMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volume);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        if (shouldPauseGame)
        {
            Time.timeScale = 0;
            // Pause timer
            TimeChangeScript.Instance.PauseTime();
        }
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        if (shouldPauseGame)
        {
            Time.timeScale = 1;
            // Unpause timer
            TimeChangeScript.Instance.ResumeTime();
        }
    }
}
