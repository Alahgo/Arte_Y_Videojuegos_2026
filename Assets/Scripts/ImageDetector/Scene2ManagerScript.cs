using TMPro;
using UnityEngine;

public class Scene2ManagerScript : MonoBehaviour
{
    static public Scene2ManagerScript instance;

    [SerializeField] private GameObject _dPanel;

    [SerializeField] private TextMeshProUGUI _dText;

    [SerializeField] private GameObject[] choices;

    public GameObject _canvas;

    public AudioSource audioSource;

    public bool _moveIsPaused = false;
    private void Awake()
    {
        if( instance != null)
        {
            Debug.LogError("Error");
        }
        instance = this;
    }

    private void Start()
    {
        DialogManager.instance.Setdata(_dPanel,_dText,choices,audioSource);
        _canvas.SetActive(false);
    }
}
