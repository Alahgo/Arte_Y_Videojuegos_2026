using UnityEngine;

public class AnimationControllerForAudio : MonoBehaviour
{
   
    void Update()
    {
        if (AudioDetectorScript.instance.HaySonidoExterno)
        {
            transform.GetComponent<Animator>().SetTrigger("Sonido");
        }
    }
}
