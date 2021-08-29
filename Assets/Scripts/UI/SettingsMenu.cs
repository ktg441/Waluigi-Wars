using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    #region Global Variables

    // Dropdown UI Elements
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityPresetDropdown;
    public TMP_Dropdown textureDropdown;
    public TMP_Dropdown antiAliasingDropdown;
    public TMP_Dropdown windowModeDropdown;

    // Audio UI Elements
    public AudioMixer audioMixer;
    public Slider volumeRocker;
    public TMP_Text volumeText;

    // Resolution UI Elements
    private Resolution[] resolutions;
    private int matchedResolution;

    #endregion

    #region Enums

    /// <summary>
    /// For saving/restoring user preferences for various game settings
    /// Using Enums to minimize spelling errors :)
    /// </summary>
    private enum PlayerPrefKeys
    {
        QUALITY_SETTING_PREFERENCE,
        RESOLUTION_PREFERENCE,
        TEXTURE_PREFERENCE,
        AA_PREFERENCE,
        SCREEN_MODE_PREFERENCE,
        VOLUME_PREFERENCE,
    }

    /// <summary>
    /// The different Quality options I'm allowing
    /// </summary>
    private enum QualityOptions
    {
        VERY_LOW,
        LOW,
        MEDIUM,
        HIGH,
        VERY_HIGH,
        ULTRA,
        CUSTOM,
    }

    #endregion

    #region Unity Defaults

    /// <summary>
    /// Runs before first frame
    /// </summary>
    void Start()
    {
        matchedResolution = -1; // If we can't match a screen resolution to one of the ones pulled from Unity, we will use the smallest option that Unity found

        FillResolutions();
        LoadSettings();
    }

    #endregion

    #region Player Prefs

    /// <summary>
    /// Saves options to a file so that they can be reloaded the next time the game launches (like anyone is going to replay this piece of garbage)
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetInt(PlayerPrefKeys.QUALITY_SETTING_PREFERENCE.ToString(), qualityPresetDropdown.value);
        PlayerPrefs.SetInt(PlayerPrefKeys.RESOLUTION_PREFERENCE.ToString(), resolutionDropdown.value);
        PlayerPrefs.SetInt(PlayerPrefKeys.TEXTURE_PREFERENCE.ToString(), textureDropdown.value);
        PlayerPrefs.SetInt(PlayerPrefKeys.AA_PREFERENCE.ToString(), antiAliasingDropdown.value);
        PlayerPrefs.SetInt(PlayerPrefKeys.SCREEN_MODE_PREFERENCE.ToString(), windowModeDropdown.value);
        PlayerPrefs.SetFloat(PlayerPrefKeys.VOLUME_PREFERENCE.ToString(), volumeRocker.value);
    }

    /// <summary>
    /// Loads settings from Player Preferences if any were saved
    /// </summary>
    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey(PlayerPrefKeys.QUALITY_SETTING_PREFERENCE.ToString()))
        {
            qualityPresetDropdown.value = PlayerPrefs.GetInt(PlayerPrefKeys.QUALITY_SETTING_PREFERENCE.ToString());
            qualityPresetDropdown.RefreshShownValue();
        }
        else
        {
            qualityPresetDropdown.value = Convert.ToInt32(QualityOptions.VERY_HIGH);
            qualityPresetDropdown.RefreshShownValue();
        }

        if (PlayerPrefs.HasKey(PlayerPrefKeys.RESOLUTION_PREFERENCE.ToString()))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt(PlayerPrefKeys.RESOLUTION_PREFERENCE.ToString());
            resolutionDropdown.RefreshShownValue();
        }
        else if (matchedResolution != -1)
        {
            resolutionDropdown.value = matchedResolution;
        }
        else
        {
            resolutionDropdown.value = 0;
        }

        if (PlayerPrefs.HasKey(PlayerPrefKeys.TEXTURE_PREFERENCE.ToString()))
        {
            textureDropdown.value = PlayerPrefs.GetInt(PlayerPrefKeys.TEXTURE_PREFERENCE.ToString());
            textureDropdown.RefreshShownValue();
        }

        if (PlayerPrefs.HasKey(PlayerPrefKeys.AA_PREFERENCE.ToString()))
        {
            antiAliasingDropdown.value = PlayerPrefs.GetInt(PlayerPrefKeys.AA_PREFERENCE.ToString());
            antiAliasingDropdown.RefreshShownValue();
        }

        if (PlayerPrefs.HasKey(PlayerPrefKeys.SCREEN_MODE_PREFERENCE.ToString()))
        {
            windowModeDropdown.value = PlayerPrefs.GetInt(PlayerPrefKeys.SCREEN_MODE_PREFERENCE.ToString());
            windowModeDropdown.RefreshShownValue();
        }
        else
        {
            windowModeDropdown.value = Convert.ToInt32(FullScreenMode.ExclusiveFullScreen);
        }

        if (PlayerPrefs.HasKey(PlayerPrefKeys.VOLUME_PREFERENCE.ToString()))
        {
            volumeRocker.value = PlayerPrefs.GetFloat(PlayerPrefKeys.VOLUME_PREFERENCE.ToString());
        }
    }

    #endregion

    #region Overall Quality

    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex != (int)QualityOptions.CUSTOM)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        switch (qualityIndex)
        {
            case (int)QualityOptions.VERY_LOW:
                textureDropdown.value = 3;
                antiAliasingDropdown.value = 0;
                break;
            case (int)QualityOptions.LOW:
                textureDropdown.value = 2;
                antiAliasingDropdown.value = 0;
                break;
            case (int)QualityOptions.MEDIUM:
                textureDropdown.value = 1;
                antiAliasingDropdown.value = 0;
                break;
            case (int)QualityOptions.HIGH:
                textureDropdown.value = 0;
                antiAliasingDropdown.value = 0;
                break;
            case (int)QualityOptions.VERY_HIGH:
                textureDropdown.value = 0;
                antiAliasingDropdown.value = 1;
                break;
            case (int)QualityOptions.ULTRA:
                textureDropdown.value = 0;
                antiAliasingDropdown.value = 2;
                break;
        }

        qualityPresetDropdown.value = qualityIndex;
    }

    #endregion

    #region Screen Resolution

    /// <summary>
    /// Fills out the dropdown list for screen resolutions via all available resolutions for that monitor
    /// </summary>
    private void FillResolutions()
    {
        resolutionDropdown.ClearOptions();
        resolutions = Screen.resolutions;
        int resIndex = 0;

        foreach (Resolution res in resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(string.Format("{0} x {1} @ {2} hz", res.width, res.height, res.refreshRate)));

            // Set dropdown value for the one that matches the current Screen res
            if (Screen.currentResolution.width == res.width && Screen.currentResolution.height == res.height)
            {
                matchedResolution = resIndex;
            }

            resIndex++;
        }
    }

    /// <summary>
    /// Sets game resolution based on a selection from the resolution dropdown
    /// </summary>
    /// <param name="resIndex">Selection from the res dropdown</param>
    public void SetResolution(int resIndex)
    {
        Screen.SetResolution(resolutions[resIndex].width, resolutions[resIndex].height, Screen.fullScreen);
    }

    #endregion

    #region Textures

    /// <summary>
    /// Sets texture quality based on dropdown selection
    /// </summary>
    /// <param name="texIndex">index of selection from dropdown</param>
    public void SetTextureQuality(int texIndex)
    {
        QualitySettings.masterTextureLimit = texIndex;
        qualityPresetDropdown.value = Convert.ToInt32(QualityOptions.CUSTOM); // Update their overall quality to custom as we are no longer following the presets
        qualityPresetDropdown.RefreshShownValue();
    }

    #endregion

    #region Anti Aliasing

    /// <summary>
    /// Sets anti aliasing based on dropdown selection
    /// </summary>
    /// <param name="aaIndex">index of selection from dropdown</param>
    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        qualityPresetDropdown.value = Convert.ToInt32(QualityOptions.CUSTOM); // Update their overall quality to custom as we are no longer following the presets
        qualityPresetDropdown.RefreshShownValue();
    }

    #endregion

    #region Window Size

    /// <summary>
    /// Sets window size based on dropdown selection
    /// </summary>
    /// <param name="windowIndex">index of selection from dropdown</param>
    public void SetScreenMode(int windowIndex)
    {
        switch (windowIndex)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
        }
    }

    #endregion

    #region Volume

    /// <summary>
    /// Sets volume based on slider
    /// </summary>
    /// <param name="volume">current value of slider</param>
    public void SetVolume(float volume)
    {
        //audioMixer.SetFloat("Volume", volume);
        volumeText.text = volume.ToString();

    }

    #endregion

}
