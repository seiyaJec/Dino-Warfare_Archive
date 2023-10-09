using UnityEngine;
using UnityEngine.AI;

public class Velociraptor_Patrol : AiState
{
    private float timer = 0.0f;

    private Vector3 startPos;

    public AiStateId GetId()
    {
        return AiStateId.Patrol;
    }

    public void Init(AiAgent agent)
    {
        startPos = agent.transform.position;
    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
        agent.navMeshAgent.speed = agent.config.runSpeed;

        Vector3 targetPos = NavMeshUtility.GetRandomPointOnNavMesh(startPos, agent.config.patrolDistance, NavMesh.AllAreas);

        agent.navMeshAgent.SetDestination(targetPos);
    }

    public void Update(AiAgent agent)
    {
        if (!agent.navMeshAgent.enabled) return;
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

        float targetDistance = Vector3.Distance(agent.navMeshAgent.destination, agent.transform.position);

        if (targetDistance <= agent.config.attackDistance)
        {
            agent.stateMachine.ChangeState(AiStateId.Idle);
            return;
        }

        agent.CheckCollider();
        agent.Rotate();

        SetAnimation(agent);

        timer -= Time.deltaTime;
    }

    private static void SetAnimation(AiAgent agent)
    {
        if (agent.navMeshAgent.isOnOffMeshLink && !agent.animator.GetCurrentAnimatorStateInfo(0).IsName("Leap"))
        {
            agent.animator.SetTrigger("Leap");
        }


        agent.animator.SetFloat("Speed", agent.navMeshAgent.velocity.magnitude);
    }

    public void Exit(AiAgent agent)
    {

    }

}
