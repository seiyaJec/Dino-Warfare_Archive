using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{

    /// <summary>
    /// �����蔻����󂯂�
    /// </summary>
    public void ReceiveHit()
    {
        GetComponent<ParticleSystem>().Play();
    }
}
