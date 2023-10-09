using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTestView : MonoBehaviour
{
    [SerializeField] public bool CallViewCamera;
    [SerializeField] public int SetViewInfo;
    [SerializeField] public PlayerRotateInfo prot;

    private void Awake()
    {
        prot = new PlayerRotateInfo();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
