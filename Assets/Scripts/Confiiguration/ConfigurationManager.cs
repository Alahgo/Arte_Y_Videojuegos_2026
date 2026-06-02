using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfigurationManager : MonoBehaviour
{
    private static ConfigurationManager instance;
    private bool _isWindowed;

    public AudioMixer mainMixer;

    public TMP_Dropdown resolutionDropdown;
    Resolution[] allResolutions;
    private List<Resolution> filteredResolutions; 

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Hay más de una instancia de DialogManager");
        }
        instance = this;
    }

    void Start()
    {
        RefreshRate currentRefreshRate = Screen.currentResolution.refreshRateRatio;

        // 1. Detectar resoluciones disponibles
        allResolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        HashSet<string> seenResolutions = new HashSet<string>();

        for (int i = 0; i < allResolutions.Length; i++)
        {
            
            if (allResolutions[i].refreshRateRatio.numerator == currentRefreshRate.numerator &&
                allResolutions[i].refreshRateRatio.denominator == currentRefreshRate.denominator)
            {
                string optionText = allResolutions[i].width + " x " + allResolutions[i].height;

                if (!seenResolutions.Contains(optionText))
                {
                    seenResolutions.Add(optionText);
                    filteredResolutions.Add(allResolutions[i]);
                    options.Add(optionText);
                }
            }
        }

        resolutionDropdown.AddOptions(options);

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            if (filteredResolutions[i].width == Screen.currentResolution.width &&
                filteredResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
                break;
            }
        }

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height,FullScreenMode.FullScreenWindow, currentRefreshRate);

    }

    public void Continue()
    {
        SceneManager.LoadScene(1);
    }

    public void SetMasterVolume(float sliderValue)
    {
       
        float volumeInDb = Mathf.Log10(sliderValue) * 20;
        Debug.Log("skhdghf");
        mainMixer.SetFloat("Master", volumeInDb);
        
    }

    public void SetMusicVolume(float sliderValue)
    {

        float volumeInDb = Mathf.Log10(sliderValue) * 20;
        Debug.Log("skhdghf");
        mainMixer.SetFloat("Music", volumeInDb);

    }

    public void SetSfxVolume(float sliderValue)
    {

        float volumeInDb = Mathf.Log10(sliderValue) * 20;
        Debug.Log("skhdghf");
        mainMixer.SetFloat("Sfx", volumeInDb);

    }

    public void SetFullscreen(bool isWindowed)
    {
        _isWindowed = isWindowed;
        if (isWindowed)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
           
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
           

        }
       
    }

    public void SetResolution(int resolutionIndex)
    {
        int screenMode = (_isWindowed) ? 3 : 1;
        Resolution resolution = filteredResolutions[resolutionIndex];

        Screen.fullScreenMode = (FullScreenMode)screenMode;
        Screen.SetResolution(resolution.width, resolution.height,(FullScreenMode)screenMode);
    }
}
