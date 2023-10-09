using UnityEngine;

public class Pteranodon_ChasePositionState : AiState
{
    Vector3 _targetPos;
    public Pteranodon_ChasePositionState(Transform chasePos)
    {
        _targetPos = chasePos.position;
    }

    public AiStateId GetId()
    {
        return AiStateId.Chase;
    }

    public void Init(AiAgent agent)
    {

    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
        //Vector3 targetPos = GameObject.FindGameObjectWithTag("PteraPos").transform.position;
        agent.navMeshAgent.SetDestination(_targetPos);
    }

    public void Update(AiAgent agent)
    {
        if (!agent.navMeshAgent.enabled) return;
        if (!agent.hasTarget) agent.stateMachine.ChangeState(AiStateId.Idle);

        if (agent.navMeshAgent.velocity.sqrMagnitude < 0.2f && agent.navMeshAgent.remainingDistance < 0.5f)
        {
            agent.stateMachine.ChangeState(AiStateId.Stanby);
        }

        agent.Rotate();

        agent.animator.SetFloat("Speed", agent.navMeshAgent.velocity.magnitude);
    }


    public void Exit(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = true;
        agent.navMeshAgent.velocity = Vector3.zero;
        agent.animator.SetFloat("Speed", 0);
    }


}
