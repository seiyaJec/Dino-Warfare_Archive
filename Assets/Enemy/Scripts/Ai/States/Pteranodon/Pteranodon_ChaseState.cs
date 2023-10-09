using UnityEngine;

public class Pteranodon_ChaseState : AiState
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
    }

    public void Update(AiAgent agent)
    {
        if (!agent.navMeshAgent.enabled) return;
        if (!agent.hasTarget) agent.stateMachine.ChangeState(AiStateId.Idle);

        float targetDistance = Vector3.Distance(agent.targetEntity.transform.position, agent.transform.position);
        if (targetDistance <= agent.config.attackDistance)
        {
            BeginAttack(agent);
            return;
        }

        if (timer < 0f)
        {
            timer = agent.config.updateTargetTime;
            agent.UpdatePath();
        }

        agent.Rotate();

        agent.animator.SetFloat("Speed", agent.navMeshAgent.velocity.magnitude);

        timer -= Time.deltaTime;
    }

    public void BeginAttack(AiAgent agent)
    {
        agent.stateMachine.ChangeState(AiStateId.NormalAttack);
    }

    public void Exit(AiAgent agent)
    {

    }

}
