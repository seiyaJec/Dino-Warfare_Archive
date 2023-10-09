using UnityEngine;

public class Velociraptor_Detour_State : AiState
{
    private Transform[] wayPoints;

    private int index;
    private int lastIndex;
    
    public AiStateId GetId()
    {
        return AiStateId.Detour;
    }

    public void Init(AiAgent agent)
    {
        if (agent.wayPoints.Length > 0)
        {
            wayPoints = agent.wayPoints;
            index = 0;
            lastIndex = wayPoints.Length - 1;
        }
    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
        agent.navMeshAgent.speed = agent.config.runSpeed;
        agent.navMeshAgent.SetDestination(wayPoints[index].position);
    }

    public void Update(AiAgent agent)
    {
        if (!agent.navMeshAgent.enabled) return;

        float targetDistance = Vector3.Distance(wayPoints[index].position, agent.transform.position);

        if (targetDistance < 1.5f)
        {
            if (index < lastIndex)
            {
                index++;
                agent.navMeshAgent.SetDestination(wayPoints[index].position);
            }
            else
            {
                agent.stateMachine.ChangeState(AiStateId.Chase);
            }
        }

        agent.Rotate();

        SetAnimation(agent);
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
