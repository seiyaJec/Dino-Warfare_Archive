using UnityEngine;
using UnityEngine.InputSystem;

public class InputReciever : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    public static InputReciever Instance { get; private set; }
    public InputAction OneShot { get; private set; }

    private void Awake()
    {
        //ƒVƒ“ƒOƒ‹ƒgƒ“
        if (Instance == null)
        {
           OneShot = _playerInput.actions["OneShot"];
           Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
