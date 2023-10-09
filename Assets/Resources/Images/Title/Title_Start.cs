using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class Title_Start : MonoBehaviour
{
    private float waitTime = 1.0f;
    private JoyconCheckInput _joycon;

    private void Awake()
    {
        _joycon = JoyconCheckInput.Instance;
        Screen.fullScreen = true;
    }

    void Update()
    {
        if (_joycon.m_pressedButtonR == Joycon.Button.SHOULDER_2)
        {
            StartCoroutine(StartGame());
        }
        if (Input.anyKey)
        {
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(waitTime);
        LoadingSceneController.LoadScene("Introduction");
    }
}
