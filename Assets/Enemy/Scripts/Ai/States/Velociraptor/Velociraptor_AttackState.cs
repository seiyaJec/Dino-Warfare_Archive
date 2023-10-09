using UnityEngine;
using System.Collections.Generic;

public class Velociraptor_AttackState : AiState
{
    private List<LivingEntity> lastAttackedTargets = new List<LivingEntity>();

    private float timer;

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
        agent.navMeshAgent.isStopped = true;
        agent.navMeshAgent.velocity = Vector3.zero;
        agent.animator.SetFloat("Speed", 0f);
        agent.animator.SetTrigger("Attack");

        SoundManager.Instance.Play("Sounds/SFX/Enemy/roar_0", SoundManager.Sound.EFFECT);
    }

    public void Update(AiAgent agent)
    {
        if (!agent.hasTarget) agent.stateMachine.ChangeState(AiStateId.Idle);

        if (agent.isAttacking)
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

        agent.RotateToTarget();

        if (agent.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            timer += Time.deltaTime;
        }

        if (timer > agent.config.attackDuration)
        {
            agent.stateMachine.ChangeState(AiStateId.Chase);
        }
    }
    public void Exit(AiAgent agent)
    {

    }
}
