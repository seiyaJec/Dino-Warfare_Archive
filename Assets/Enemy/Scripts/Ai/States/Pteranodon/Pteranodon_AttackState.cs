using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pteranodon_AttackState : AiState
{
    //çUåÇÇÃèdï°Çñhé~Ç∑ÇÈÇΩÇﬂÇÃÉäÉXÉg
    private List<LivingEntity> lastAttackedTargets = new List<LivingEntity>();

    private float timer;
    private float speedLerp = 0.02f;
    private RaycastHit[] hits = new RaycastHit[2];

    public AiStateId GetId()
    {
        return AiStateId.NormalAttack;
    }

    public void Init(AiAgent agent)
    {

    }

    public void Enter(AiAgent agent)
    {
        timer = 0f;
        lastAttackedTargets.Clear();
        agent.navMeshAgent.isStopped = false;
        agent.navMeshAgent.velocity = Vector3.zero;
    }

    public void Update(AiAgent agent)
    {
        Attack(agent);

        timer += Time.deltaTime;

        if (timer > agent.config.attackDuration)
        {
            agent.stateMachine.ChangeState(AiStateId.BackOff);
        }

        float speed = Mathf.Lerp(agent.animator.GetFloat("Speed"), 0, speedLerp);
        agent.animator.SetFloat("Speed", speed);
    }

    private void Attack(AiAgent agent)
    {
        var direction = agent.transform.forward;
        var deltaDistance = agent.navMeshAgent.velocity.magnitude * Time.deltaTime;

        var size = Physics.SphereCastNonAlloc(agent.attackRoot.position, agent.config.attackRadius, direction,
            hits, deltaDistance, agent.whatIsTarget);

        for (var i = 0; i < size; i++)
        {
            var attackTargetEntity = hits[i].collider.GetComponent<LivingEntity>();

            if (attackTargetEntity != null && !lastAttackedTargets.Contains(attackTargetEntity))
            {
                var message = new DamageMessage();
                message.Amount = agent.config.damage;
                message.damager = agent.gameObject;

                if (hits[i].distance <= 0f)
                {
                    message.hitPoint = agent.attackRoot.position;
                }
                else
                {
                    message.hitPoint = hits[i].point;
                }

                message.hitNormal = hits[i].normal;
                attackTargetEntity.ApplyDamage(message);
                lastAttackedTargets.Add(attackTargetEntity);
                break;
            }
        }
    }

    public void Exit(AiAgent agent)
    {

    }

}
