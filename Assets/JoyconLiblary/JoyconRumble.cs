using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializableAttribute]
public class JoyconRumble
{
    public bool _enable = false;
    public float _lowFreq;
    public float _highFreq;
    public float _amp;
    public int _time;

    public void SetToJoycon(Joycon jcon)
    {
        if (jcon == null || _enable == false)
        {
            return;
        }

        jcon.SetRumble(_lowFreq, _highFreq, _amp, _time);
    }
}
