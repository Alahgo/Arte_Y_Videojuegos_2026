using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DialogManager : MonoBehaviour
{
    [Header("Dialog UI")]
    [SerializeField] private GameObject _dPanel;

    [SerializeField] private TextMeshProUGUI _dText;

    [SerializeField] private Image _dImage;

    public bool _isConsoleText;

    [Tooltip("Posibles opciones dentro de la UI del canvas")]
    [SerializeField] private GameObject[] choices;

    private TextMeshProUGUI[] _choicesText;

    [Header("Dialog Sound")]

    [SerializeField] private AudioSource _audioSource;

    private AudioClip[] _audioClips;

    private Story _story;

    public bool _dIsPlaying;

    private bool _isTyping;

    private string rutaCompletaArchivo;

    private bool lockEvent = false;

    private string contenidoActual;

    private string contenidoEsperado = "Chico\nChica\nChique";

    public bool _isChossing;

    private List<Choice> currentChoices;

    [Tooltip("Tiempo entre una letra y otra, a menos typingSpeed m?s r?pido se escribe el texto")]
    public float typingSpeed;

    [Tooltip("A menos pitch sonidos m?s lentos y graves. A mayor pitch sonidos m?s r?pidos y agudos")]
    public float minPitch, maxPitch;



    public static DialogManager instance;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Hay m?s de una instancia de DialogManager");
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        rutaCompletaArchivo = PlayerPrefs.GetString("RutaArchivoGeneros");
        _dIsPlaying = false;
        _isTyping = false;
        _isChossing = false;

        _dPanel.SetActive(false);
        _choicesText = new TextMeshProUGUI[choices.Length];
        int i = 0;
        foreach (GameObject c in choices)
        {
            _choicesText[i] = c.GetComponentInChildren<TextMeshProUGUI>();
            c.SetActive(false);
            i++;
        }
    }

    private void Update()
    {
        
        if (File.Exists(rutaCompletaArchivo))
        {
            Debug.Log("Existe");

            if (!lockEvent)
            {
                string contenidoActual = File.ReadAllText(rutaCompletaArchivo);

                if (contenidoActual == contenidoEsperado)
                {
                    Debug.Log("Se modifico");
                    lockEvent = !lockEvent;
                    Narrador.instance.SetIndex(1);

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && !_isTyping && !_isChossing)
        {
            ContinueStory();
        }
    }



    public void EnterInDialogMode(TextAsset _InkJson, AudioClip[] typingSounds, Sprite portrait)
    {
        if (File.Exists(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ArchivoPruebas.txt")))
        {
            _audioClips = typingSounds;
            _story = new Story(_InkJson.text);
            _dIsPlaying = true;
            _dPanel.SetActive(true);
            if(_isConsoleText) _dImage.sprite = portrait;

            ContinueStory();
        }
    }

    public void EnterInDialogMode(TextAsset _InkJson, AudioClip[] typingSounds)
    {
        _audioClips = typingSounds;
        _story = new Story(_InkJson.text);
        _dIsPlaying = true;
       
        if(_isChossing) _dPanel.SetActive(true);

        ContinueStory();
       
    }

    public void ExitDialogMode()
    {
        _dIsPlaying = false;
        _dPanel.SetActive(false);
        _dText.text = "";
        UnDisplaceChoices();
    }

    public void ContinueStory()
    {
        if (_story.canContinue)
        {
            StopAllCoroutines();
            string lineaTexto = _story.Continue();


            if (_story.currentChoices.Count > 0)
            {
               
                _dPanel.SetActive(true);
            }
            else
            {
               
                _dPanel.SetActive(false);
            }

            List<string> etiquetasActuales = _story.currentTags;
            if (etiquetasActuales.Count > 0)
            {
                // Comprobamos si viene la etiqueta de encender el panel
                if (etiquetasActuales.Contains("mostrarPanel"))
                {
                    _dPanel.SetActive(true);
                }

               
                if (etiquetasActuales.Contains("siguientePantalla"))
                {
                    Debug.Log("¡Etiqueta detectada! Cambiando de escena...");
                    SceneManager.LoadScene(2);
                }

                if (etiquetasActuales.Contains("activarMov"))
                {
                    Scene2ManagerScript.instance._moveIsPaused = false;
                    Scene2ManagerScript.instance._canvas.SetActive(false);
                }
            }

            // Empezamos a escribir la línea
            StartCoroutine(TypeLine(lineaTexto));
        }
        else if (!_story.canContinue && Narrador.instance.index < Narrador.instance.numeroD)
        {
            Narrador.instance.index++;
            Narrador.instance.MandarCorutinaDialogo();
        }
        else
        {
            ExitDialogMode();
        }
    }



    public void MakeChoice(int slectedindex)
    {
        _isChossing = false;
        Debug.Log("Se pulso con indice" + slectedindex);
        _story.ChooseChoiceIndex(slectedindex);
        ContinueStory();
    }

    private void DisplayChoices()
    {
        _dPanel.SetActive(true);
        _isChossing = true;
        currentChoices = _story.currentChoices;

        if (currentChoices.Count > choices.Length)
            Debug.LogError("Más opciones de las que soporta la UI. Max soportado " + choices.Length);

        int i = 0;
        foreach (Choice choice in currentChoices)
        {

            choices[i].SetActive(true);
            _choicesText[i].text = choice.text;
            i++;
        }
    }

    private void UnDisplaceChoices()
    {

        // Desactivamos los botones de la UI
        foreach (GameObject choiceBtn in choices)
        {
            if (choiceBtn != null)
            {
                choiceBtn.SetActive(false);
            }
        }

        // Limpiamos los textos de los botones
        foreach (TextMeshProUGUI txt in _choicesText)
        {
            if (txt != null) txt.text = "";
        }
    }

    IEnumerator TypeLine(string line)
    {
        _isTyping = true;
        _dText.text = "";

        // Recorremos y escribimos el texto letra a letra
        foreach (char letter in line.ToCharArray())
        {
            _dText.text += letter;

            if (letter != ' ' && _audioClips != null && _audioClips.Length > 0)
            {
                PlayTypingSound();
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        _isTyping = false;

        // Al terminar de escribir, si había opciones detectadas, las mostramos en pantalla
        if (_story.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        else
        {
            UnDisplaceChoices();
        }
    }

    private void PlayTypingSound()
    {
        _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        _audioSource.PlayOneShot(_audioClips[UnityEngine.Random.Range(0, _audioClips.Length - 1)]);
    }

    public void Setdata(GameObject p, TextMeshProUGUI t, GameObject[] o, AudioSource a)
    {
        _dPanel = p;
        _dText = t;
        choices = o;
        _audioSource = a;

    }
}
