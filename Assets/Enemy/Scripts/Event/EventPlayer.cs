using System;
using UnityEngine;

public class EventPlayer : MonoBehaviour
{
    public Action action;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        if (action != null)
        {
            action();
        }

        this.enabled = false;
    }
}
