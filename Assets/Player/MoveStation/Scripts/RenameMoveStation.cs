using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[ExecuteAlways]
public class RenameMoveStation : MonoBehaviour
{
#if UNITY_EDITOR
    private void Update()
    {
        if(!Application.isPlaying)
        {
            gameObject.name = "MoveStation_" + transform.GetSiblingIndex();
        }
    }
#endif
}
