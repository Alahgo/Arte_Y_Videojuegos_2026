using UnityEngine;
using System.IO;
using System;

public class CrearFicheroScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CrearArchivoEnEscritorio();
    }

    private void CrearArchivoEnEscritorio()
    {
        string escritorioPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        
        string nombreArchivo = "ArchivoPruebas.txt";
        string rutaCompleta = Path.Combine(escritorioPath, nombreArchivo);

        
        string contenido = "¡Hola! Este archivo fue creado desde Unity el " + DateTime.Now;

        try
        {
            
            File.WriteAllText(rutaCompleta, contenido);
            Debug.Log("Archivo guardado con éxito en: " + rutaCompleta);
        }
        catch (Exception e)
        {
            Debug.LogError("Error al guardar el archivo: " + e.Message);
        }
    }
}

