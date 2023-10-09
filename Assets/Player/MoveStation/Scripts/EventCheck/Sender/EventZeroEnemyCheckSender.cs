using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventZeroEnemyCheckSender : EventCheckSender
{
    [SerializeField] private EventZeroEnemyCheck _eventZeroEnemyCheck;
    public override IEventCheck GetEventCheck()
    {
        _eventZeroEnemyCheck._enemyCounter = GameObject.Find("EnemyCounter").GetComponent<EnemyCounter>();
        return _eventZeroEnemyCheck;
    }
}

[System.SerializableAttribute]
public class EventZeroEnemyCheck : IEventCheck
{
    [HideInInspector] public EnemyCounter _enemyCounter;

    public bool Finished()
    {
        if(_enemyCounter.LengthAll() == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}