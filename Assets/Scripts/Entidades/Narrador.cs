using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Narrador : MonoBehaviour
{

    [SerializeField] private TextAsset[] inkJSON;
    public int numeroD;
    public int index = 0;
    [SerializeField] private AudioClip[] typingSounds;
    public static Narrador instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Hay más de in narrador");
        }
        instance = this;
    }
    private void Start()
    {
        numeroD = inkJSON.Length-1;
        MandarCorutinaDialogo();
    }


    IEnumerator empezarDialogo()
    {
        yield return new WaitForSeconds(1.0f);
        DialogManager.instance.EnterInDialogMode(inkJSON[index], typingSounds);
    }

    public void MandarCorutinaDialogo()
    {
        StartCoroutine("empezarDialogo");
    }
}
