using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestViewCameraScript : MonoBehaviour
{
    private void Awake()
    {
        //実際のゲームではこのカメラは起動しない
#if !UNITY_EDITOR
        GetComponent<GameObject>().SetActive(false);
#endif
    }
}
