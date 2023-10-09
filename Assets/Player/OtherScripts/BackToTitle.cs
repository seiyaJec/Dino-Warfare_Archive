using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToTitle : MonoBehaviour
{
    [SerializeField] private Joycon.Button _buttonToBack;
    [SerializeField] private KeyCode _keyToBack;
    [HideInInspector] private JoyconCheckInput _joycon;

    private void Awake()
    {
        _joycon = GetComponent<JoyconCheckInput>();
    }

    private void Update()
    {
        if(_joycon.m_pressedButtonL == _buttonToBack
            || Input.GetKeyDown(_keyToBack))
        {
            LoadingSceneController.LoadScene("Scene_Title");
        }
    }

}
