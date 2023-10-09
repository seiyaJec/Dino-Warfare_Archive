using UnityEngine;

public class AiIdleState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }
    public void Init(AiAgent agent)
    {

    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = true;
        agent.navMeshAgent.velocity = Vector3.zero;
    }

    public void Update(AiAgent agent)
    {
        if (agent.targetEntity && !agent.targetEntity.dead)
        {
            agent.stateMachine.ChangeState(AiStateId.Chase);
        }

        agent.animator.SetFloat("Speed", 0f);

        agent.CheckCollider();
    }

    public void Exit(AiAgent agent)
    {

    }
}
