using UnityEngine;

public class AnimationControllerForAudio : MonoBehaviour
{
   private bool lockB = false;
    void Update()
    {
        if (!lockB && AudioDetectorScript.instance.HaySonidoExterno)
        {
            lockB = true;
            transform.GetComponent<Animator>().SetTrigger("LevantarPuerta");
        }
    }
}
