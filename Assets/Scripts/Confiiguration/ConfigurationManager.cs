using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class DatosImagen
{
    public Texture2D textura;
    public string nombreArchivo;
}

public class ConfigurationManager : MonoBehaviour
{
    private static ConfigurationManager instance;
    private bool _isWindowed;

    public AudioMixer mainMixer;

    public TMP_Dropdown resolutionDropdown;
    [SerializeField] private DatosImagen[] imagenes;
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
        CrearEstructuraDeCarpetas();
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

    private void CrearEstructuraDeCarpetas()
    {
        string nombreRoot = "Carpeta_Datos";
        string nombreCarpeta = "Generos disponibles";
        string nombreCarpeta2 = "Prohibido";
        string nombreCarpetaImg = "IMG";
        string nombreArchivo = "Generos.txt";
        string nombreCarpetaRPG = "StatCombats";
        string nombreArchivoRPG = "TusEstadisticas.txt";

        string rutaCarpetaRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), nombreRoot);

        string rutaImg = Path.Combine(rutaCarpetaRoot, nombreCarpetaImg);

        string rutaCarpeta = Path.Combine(rutaCarpetaRoot, nombreCarpeta);
        string rutaCompletaArchivo = Path.Combine(rutaCarpeta, nombreArchivo);

        string rutaCarpeta2 = Path.Combine(rutaCarpeta, nombreCarpeta2);
        string rutaCompletaArchivo2 = Path.Combine(rutaCarpeta2, nombreArchivo);

        string rutaRPGarchivo = Path.Combine(rutaCarpetaRoot, nombreCarpetaRPG);
        string rutaRPGtxt = Path.Combine(rutaRPGarchivo, nombreArchivoRPG);



        try
        {

            if (!Directory.Exists(rutaCarpetaRoot))
            {
                Directory.CreateDirectory(rutaCarpeta);
            }

            if (!Directory.Exists(rutaCarpeta))
            {
                Directory.CreateDirectory(rutaCarpeta);
            }

            if (!File.Exists(rutaCompletaArchivo))
            {
                File.WriteAllText(rutaCompletaArchivo, "Chico\nChica");
            }
            else
            {
                Debug.Log("El archivo ya existe, no se volvió a crear.");
            }

            if (!Directory.Exists(rutaCarpeta2))
            {
                Directory.CreateDirectory(rutaCarpeta2);
            }

            if (!File.Exists(rutaCompletaArchivo2))
            {
                File.WriteAllText(rutaCompletaArchivo2, "Chico\nChica\nChique");
            }
            else
            {
                Debug.Log("El archivo ya existe, no se volvió a crear.");
            }

            if (!Directory.Exists(rutaRPGarchivo))
            {
                Directory.CreateDirectory(rutaRPGarchivo);
            }

            if (!File.Exists(rutaRPGtxt))
            {
                File.WriteAllText(rutaRPGtxt, "Poder: 1\nDefensa: 1");
                PlayerPrefs.SetString("RutaArchivoRPG", rutaRPGtxt);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.Log("El archivo ya existe, no se volvió a crear.");
            }

            PlayerPrefs.SetString("RutaArchivoGeneros", rutaCompletaArchivo);
            PlayerPrefs.Save();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al intentar crear los archivos: {e.Message}");
        }
        try
        {
            if (!Directory.Exists(rutaImg))
            {
                Directory.CreateDirectory(rutaImg);
            }

            PlayerPrefs.SetString("RutaArchivoImg", rutaImg);
            PlayerPrefs.Save();

            
            foreach (DatosImagen img in imagenes)
            {
                if (img.textura == null) continue;

                byte[] bytesImagen = img.textura.EncodeToPNG();
                string ruta = Path.Combine(rutaImg, img.nombreArchivo);

                File.WriteAllBytes(ruta, bytesImagen);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
