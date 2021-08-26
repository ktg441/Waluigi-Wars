using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    public Dropdown resolutionDropdown;
    public Dropdown qualityPresetDropdown;
    public Dropdown textureDropdown;
    public Dropdown antiAliasingDropdown;

    Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ToggleFullscreen(FullScreenMode screenMode)
    {
        Screen.fullScreenMode = screenMode;
    }

    public void SetResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }

    public void SetTextureQuality(int index)
    {
        QualitySettings.masterTextureLimit = index;
        qualityPresetDropdown.value = 6; // whatever custom option is
    }

    public void SetAntiAliasing(int index)
    {
        QualitySettings.antiAliasing = index;
        qualityPresetDropdown.value = 6; // whatever custom option is
    }

    public void SetQuality(int index)
    {
        switch (index)
        {
            // do options here
            case 0:
                textureDropdown.value = 3;
                antiAliasingDropdown.value = 0;
                break;
            default:
                break;
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", qualityPresetDropdown.value);
    }

    public void LoadSettings(int index)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
        {
            qualityPresetDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        }
    }

}
