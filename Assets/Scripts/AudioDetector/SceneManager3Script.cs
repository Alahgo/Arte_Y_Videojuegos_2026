using UnityEngine;

public class SceneManager3Script : MonoBehaviour
{
    static public SceneManager3Script instance;
    public GameObject entrada;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Error");
        }
        instance = this;
    }


}
