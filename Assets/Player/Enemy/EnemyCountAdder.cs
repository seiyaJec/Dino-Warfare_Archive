using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCountAdder : MonoBehaviour
{
    [SerializeField] private EnemyCounter.EnemysType _enemysType;
    private LivingEntity _livingEntity;
    private EnemyCounter _enemyCounter;

    void Start()
    {
        _enemyCounter = GameObject.Find("EnemyCounter").GetComponent<EnemyCounter>();
        _enemyCounter.AddCount(_enemysType);
        _livingEntity = GetComponent<LivingEntity>();
        _livingEntity.OnDeath += SubCount;
    }

    private void SubCount()
    {
        _enemyCounter.RemoveCount(_enemysType);
        this.enabled = false;
    }
}
