using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventCheckSender : MonoBehaviour
{
    public abstract IEventCheck GetEventCheck();
}
