using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventZeroTypeCheckSender : EventCheckSender
{
    [SerializeField] private EventZeroTypeCheck _eventZeroTypeCheck;
    public override IEventCheck GetEventCheck()
    {
        _eventZeroTypeCheck._enemyCounter = GameObject.Find("EnemyCounter").GetComponent<EnemyCounter>();
        return _eventZeroTypeCheck;
    }
}

[System.SerializableAttribute]
public class EventZeroTypeCheck : IEventCheck
{
    [HideInInspector] public EnemyCounter _enemyCounter;
    [SerializeField] private EnemyCounter.EnemysType _checkEnemyType;

    public bool Finished()
    {
        if (_enemyCounter._enemysNum[(int)_checkEnemyType] == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}