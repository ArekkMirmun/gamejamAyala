using System;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer sfxMixer;
    public AudioMixer bgmMixer;
    public bool shouldPauseGame = false;
    public GameObject settingsPanel;
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    
    public void SetVolumeSfx(float volume)
    {
        sfxMixer.SetFloat("Volume", volume);
    }
    
    public void SetVolumeBGM(float volume)
    {
        bgmMixer.SetFloat("Volume", volume);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        if (shouldPauseGame)
        {
            Time.timeScale = 0;
        }
    }
    
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        if (shouldPauseGame)
        {
            Time.timeScale = 1;
        }
    }
    
}
