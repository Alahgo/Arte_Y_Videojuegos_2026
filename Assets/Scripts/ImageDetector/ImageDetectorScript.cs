using System;
using System.IO;
using UnityEngine;

public class ImageDetectorScript : MonoBehaviour
{
    private bool lockEvent;
    private string rutaCompletaArchivo1;
    private string rutaCompletaArchivo2;
    private string rutaCarpeta;

    public GameObject piedra;
    public GameObject puente;

    [SerializeField] private Texture2D puentePNG;

    void Start()
    {
        lockEvent = false;
        rutaCarpeta = PlayerPrefs.GetString("RutaArchivoImg");
        rutaCompletaArchivo1 = Path.Combine(rutaCarpeta, "BloquearCamino.png");
    }

    void Update()
    {
        if (!lockEvent)
        {
            if (!File.Exists(rutaCompletaArchivo1))
            {
                Debug.Log("Borrado");

                piedra.GetComponent<Animator>().SetTrigger("DestruirPiedra");

                string rutaCarpeta2 = Path.Combine(rutaCarpeta, "No_necesitas_esto");
                rutaCompletaArchivo2 = Path.Combine(rutaCarpeta2, "Puente.png");

                try
                {
                    if (!Directory.Exists(rutaCarpeta2))
                    {
                        Directory.CreateDirectory(rutaCarpeta2);
                    }

                    if (!File.Exists(rutaCompletaArchivo2))
                    {
                       
                        if (puentePNG != null)
                        {
                          
                            byte[] bytesImagen = puentePNG.EncodeToPNG();

                           
                            File.WriteAllBytes(rutaCompletaArchivo2, bytesImagen);

                        }
                        else
                        {
                            Debug.LogError("No has asignado ninguna textura en el campo 'puentePNG' del Inspector.");
                        }
                    }
                    else
                    {
                        Debug.Log("El archivo ya existe, no se volvió a crear.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error al crear el archivo: {e.Message}");
                }

                lockEvent = true;
                Scene2ManagerScript.instance._moveIsPaused = true;
            }
        }
    }
}