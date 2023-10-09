using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testSkip : MonoBehaviour
{
    [SerializeField] private JoyconCheckInput _joycon; //ƒWƒ‡ƒCƒRƒ“

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("CameraStageTest1");
        }

        if (_joycon.m_pressedButtonR == Joycon.Button.PLUS)
        {
            LoadingSceneController.LoadScene("CameraStageTest1");
        }
    }
}
