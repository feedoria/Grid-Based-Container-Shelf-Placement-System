using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    [field: SerializeField] public GameObject OneHandSlot { get; set; }
    public GameObject HeldObject => _heldObject;
    [SerializeField] GameObject _heldObject;
    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}