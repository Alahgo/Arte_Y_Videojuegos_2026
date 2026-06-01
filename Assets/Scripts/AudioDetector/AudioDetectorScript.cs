using UnityEngine;


public class AudioDetectorScript : MonoBehaviour
{
    private AudioClip clipEscucha;
    private const int SAMPLE_RATE = 8000;  
    private const int BUFFER_SEGS = 1;     
    private const float UMBRAL = 0.02f;     

    [SerializeField] private string dispositivoObjetivo = "CABLE Output";


    public bool HaySonidoExterno { get; private set; }
    private string dispositivoActivo;

    public static AudioDetectorScript instance;

    private void Awake()
    {
        if (instance != null) {
            Debug.LogError("Hay más de una instancia de detectores de audio");
        }

        instance = this;
    }


    void Start()
    {
        dispositivoActivo = EncontrarDispositivoLoopback();


        if (dispositivoActivo == null)
        {
            Debug.LogWarning("No se encontró un dispositivo loopback compatible.");
            enabled = false;
            return;
        }


        clipEscucha = Microphone.Start(dispositivoActivo, true, BUFFER_SEGS, SAMPLE_RATE);
        Debug.Log($"Escuchando audio del sistema via: '{dispositivoActivo}'");
    }


    void Update()
    {
        if (clipEscucha == null) return;

        float[] muestras = new float[2048];

        int posicion = Microphone.GetPosition(dispositivoActivo);
        if (posicion < muestras.Length) return;

 
        clipEscucha.GetData(muestras, posicion - muestras.Length);


        float nivelMax = 0f;
        foreach (float muestra in muestras)
        {
            float abs = Mathf.Abs(muestra);
            if (abs > nivelMax) nivelMax = abs;
        }


        bool habiaSonido = HaySonidoExterno;
        HaySonidoExterno = nivelMax > UMBRAL;


        if (HaySonidoExterno != habiaSonido)
        {
            Debug.Log(HaySonidoExterno ? "▶ Sonido externo detectado" : "⏹ Silencio");
        }

    }


    private string EncontrarDispositivoLoopback()
    {
        foreach (string dispositivo in Microphone.devices)
        {
            Debug.Log($"Dispositivo de grabación encontrado: {dispositivo}");

            if (dispositivo.Contains(dispositivoObjetivo) ||
                dispositivo.Contains("Stereo Mix") ||
                dispositivo.Contains("What U Hear") ||
                dispositivo.Contains("Mezcla estéreo"))
            {
                return dispositivo;
            }
        }
        return null;
    }


    void OnDestroy()
    {
        if (!string.IsNullOrEmpty(dispositivoActivo) && Microphone.IsRecording(dispositivoActivo))
            Microphone.End(dispositivoActivo);
    }


    public bool EstaDetectandoAudio()
    {
        if (!enabled || clipEscucha == null) return false;

        return HaySonidoExterno;
    }
}
