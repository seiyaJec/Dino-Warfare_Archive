using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{

    /// <summary>
    /// “–‚½‚è”»’è‚ðŽó‚¯‚é
    /// </summary>
    public void ReceiveHit()
    {
        GetComponent<ParticleSystem>().Play();
    }
}
