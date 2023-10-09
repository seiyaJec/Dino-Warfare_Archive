using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{

    /// <summary>
    /// 当たり判定を受ける
    /// </summary>
    public void ReceiveHit()
    {
        GetComponent<ParticleSystem>().Play();
    }
}
