using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestViewCameraScript : MonoBehaviour
{
    private void Awake()
    {
        //ÀÛ‚ÌƒQ[ƒ€‚Å‚Í‚±‚ÌƒJƒƒ‰‚Í‹N“®‚µ‚È‚¢
#if !UNITY_EDITOR
        GetComponent<GameObject>().SetActive(false);
#endif
    }
}
