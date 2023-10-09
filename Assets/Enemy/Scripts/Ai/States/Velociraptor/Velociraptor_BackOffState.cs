using UnityEngine;

public class Velociraptor_BackOffState : AiState
{
    private float speedModifier = 0.35f;

    private float timer;
    private float maxStanbyTime = 1.0f;

    public AiStateId GetId()
    {
        return AiStateId.BackOff;
    }

    public void Enter(AiAgent agent)
    {
        timer = 0f;

        agent.navMeshAgent.isStopped = false;
        agent.navMeshAgent.SetDestination(agent.RangeAttackPosition);
        agent.navMeshAgent.speed = agent.config.runSpeed * speedModifier;
    }

    public void Init(AiAgent agent)
    {

    }

    public void Update(AiAgent agent)
    {
        if ((agent.navMeshAgent.velocity.sqrMagnitude < 0.2f && agent.navMeshAgent.remainingDistance < 0.5f) 
            || timer > maxStanbyTime)
        {
            agent.stateMachine.ChangeState(AiStateId.Chase);
        }

        if (agent.navMeshAgent.velocity == Vector3.zero)
        {
            timer += Time.deltaTime;
        }

        agent.RotateToTarget();

        agent.animator.SetFloat("Speed", -1 * agent.navMeshAgent.velocity.magnitude);
    }

    public void Exit(AiAgent agent)
    {

    }
}