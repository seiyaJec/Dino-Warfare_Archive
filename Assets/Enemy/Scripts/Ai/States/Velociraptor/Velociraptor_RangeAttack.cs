using UnityEngine;

public class Velociraptor_RangeAttack : AiState
{
    private float timer;
    private float chargeTime;

    private Tyranno_Weapon weapon;

    public AiStateId GetId()
    {
        return AiStateId.RangedAttack;
    }

    public void Init(AiAgent agent)
    {
        weapon = agent.GetComponentInChildren<Tyranno_Weapon>();
        chargeTime = weapon.chargeDuration;
    }

    public void Enter(AiAgent agent)
    {
        if (weapon.dead)
        {
            agent.stateMachine.ChangeState(AiStateId.Chase);
            return;
        }

        agent.navMeshAgent.isStopped = true;
        timer = agent.config.rangeAttackDuration + chargeTime;

        agent.animator.SetTrigger("RangeAttack");
        agent.animator.SetFloat("Speed", 0f);
    }

    public void Update(AiAgent agent)
    {
        agent.RotateToTarget();

        if (agent.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            timer -= Time.deltaTime;
        }

        if (timer < 0)
        {
            agent.stateMachine.ChangeState(AiStateId.Chase);
        }
    }

    public void Exit(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }
}


