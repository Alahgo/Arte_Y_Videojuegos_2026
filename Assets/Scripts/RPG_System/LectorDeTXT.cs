using System;
using UnityEngine;
using System.IO;


public class LectorDeTXT : MonoBehaviour
{
    public BattleUnit playerUnit;

    void Start()
    {
        CargarDatosDesdeTxt();
    }

    public void CargarDatosDesdeTxt()
    {
      
        string rutaArchivo = PlayerPrefs.GetString("RutaArchivoRPG");

      
        if (string.IsNullOrEmpty(rutaArchivo))
        {
            return;
        }

        if (!File.Exists(rutaArchivo))
        {
            return;
        }

      
        string[] lineas = File.ReadAllLines(rutaArchivo);

       
        foreach (string linea in lineas)
        {
            string[] partes = linea.Split(':');

            if (partes.Length == 2)
            {
                string estadistica = partes[0].Trim();
                string valorEnTexto = partes[1].Trim();

                if (int.TryParse(valorEnTexto, out int valorNumerico))
                {
                    if (estadistica == "Poder")
                    {
                        playerUnit.baseAttack = valorNumerico;
                    }
                    else if (estadistica == "Defensa")
                    {
                        playerUnit.baseDefense = valorNumerico;
                    }
                }
                else
                {
                    Debug.LogWarning($"No se pudo leer el número en la línea: {linea}");
                }
            }
        }

        Debug.Log($"¡Datos cargados con éxito desde el disco! Poder: {playerUnit.baseAttack} | Defensa: {playerUnit.baseDefense}");
    }
}
