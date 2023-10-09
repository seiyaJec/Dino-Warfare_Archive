public class Triceratops_BackOffState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.BackOff;
    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
        agent.navMeshAgent.SetDestination(agent.config.attackPosition);
        agent.navMeshAgent.speed = agent.config.runSpeed;
    }

    public void Init(AiAgent agent)
    {

    }

    public void Update(AiAgent agent)
    {
        if (agent.navMeshAgent.velocity.sqrMagnitude < 0.2f && agent.navMeshAgent.remainingDistance < 0.5f)
        {
            agent.stateMachine.ChangeState(AiStateId.Stanby);
        }

        agent.RotateToTarget();

        agent.animator.SetFloat("Speed", -1 * agent.navMeshAgent.velocity.magnitude);
    }

    public void Exit(AiAgent agent)
    {

    }
}