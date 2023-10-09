using UnityEngine;

public class VelociraptorR_IdleState : AiState
{
    private float timeBetPatorl;

    private float minStanbyTime = 2.0f;
    private float maxStanbyTime = 4.0f;

    private float speedModifier = 0.65f;

    private float timer = 0f;

    private bool patorl;

    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }
    public void Init(AiAgent agent)
    {
        this.patorl = (agent as Velociraptor_Range_Aiagent).patrol;
    }

    public void Enter(AiAgent agent)
    {
        timer = 0f;
        timeBetPatorl = Random.Range(minStanbyTime, maxStanbyTime);
        agent.navMeshAgent.isStopped = true;
        agent.navMeshAgent.speed = agent.config.runSpeed * speedModifier;
    }

    public void Update(AiAgent agent)
    {
        if (agent.hasTarget)
        {
            if (agent.wayPoints.Length > 0)
            {
                agent.stateMachine.ChangeState(AiStateId.Detour);
            }
            else
            {
                agent.stateMachine.ChangeState(AiStateId.Chase);
            }
        }
        else if (timer > timeBetPatorl && agent.targetEntity == null && patorl)
        {
            agent.stateMachine.ChangeState(AiStateId.Patrol);
        }

        agent.CheckCollider();
        agent.Rotate();
        timer += Time.deltaTime;

        agent.animator.SetFloat("Speed", agent.navMeshAgent.velocity.magnitude);
    }

    public void Exit(AiAgent agent)
    {

    }
}
