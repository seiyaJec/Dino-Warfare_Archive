using UnityEngine;

public class Pteranodon_RangeAttack : AiState
{
    private float timer;

    public AiStateId GetId()
    {
        return AiStateId.RangedAttack;
    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = true;
        timer = agent.config.rangeAttackDuration;
        agent.animator.SetTrigger("RangeAttack");
    }

    public void Init(AiAgent agent)
    {
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
            agent.stateMachine.ChangeState(AiStateId.Stanby);
        }
    }

    public void Exit(AiAgent agent)
    {

    }
}


