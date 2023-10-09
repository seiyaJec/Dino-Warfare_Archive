using UnityEngine;

public class Velociraptor_ChaseState : AiState
{
    private float timer = 0.0f;

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
        agent.navMeshAgent.velocity = Vector3.zero; 
        agent.navMeshAgent.speed = agent.config.runSpeed;
        agent.UpdatePath();
    }

    public void Update(AiAgent agent)
    {
        if (!agent.navMeshAgent.enabled) return;
        if (!agent.hasTarget)
        {
            agent.stateMachine.ChangeState(AiStateId.Idle);
            return;
        }

        float targetDistance = Vector3.Distance(agent.targetEntity.transform.position, agent.transform.position);

        if (agent.IsTargetOnSight(agent.targetEntity.transform))
        {
            BeginRangeAttack(agent);
            return;
        }

        if (timer < 0f)
        {
            timer = agent.config.updateTargetTime;
            agent.UpdatePath();
        }

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

    private void BeginRangeAttack(AiAgent agent)
    {
        agent.stateMachine.ChangeState(AiStateId.RangedAttack);
    }

    public void Exit(AiAgent agent)
    {

    }

}
