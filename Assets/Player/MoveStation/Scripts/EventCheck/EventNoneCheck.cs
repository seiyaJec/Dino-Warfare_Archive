using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNoneCheck : IEventCheck
{
    public bool Finished()
    {
        return true;
    }
}