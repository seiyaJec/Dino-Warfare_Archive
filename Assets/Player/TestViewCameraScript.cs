using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestViewCameraScript : MonoBehaviour
{
    private void Awake()
    {
        //���ۂ̃Q�[���ł͂��̃J�����͋N�����Ȃ�
#if !UNITY_EDITOR
        GetComponent<GameObject>().SetActive(false);
#endif
    }
}
