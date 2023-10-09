using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventZeroTargetCheckSender : EventCheckSender
{
    [SerializeField] private Event_SetEnemyTarget _targets;

    public override IEventCheck GetEventCheck()
    {
        return new EventZeroTargetCheck(_targets.GetEnemy);
    }

    [System.SerializableAttribute]
    public class EventZeroTargetCheck : IEventCheck
    {
        [HideInInspector] private LivingEntity[] _enemies;

        public EventZeroTargetCheck(AiAgent[] enemies)
        {
            _enemies = new LivingEntity[enemies.Length];
            for(int i = 0; i < enemies.Length; i++)
            {
                _enemies[i] = enemies[i].GetComponent<LivingEntity>();
            }
        }

        public bool Finished()
        {
            foreach(var enemy in _enemies)
            {
                if(enemy.dead == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

