using UnityEngine;

public class Triceratops_GetHitState : AiState
{
    private float timer = 0f;
    private float duration = 0.15f;


    public AiStateId GetId()
    {
        return AiStateId.GetHit;
    }


    public void Init(AiAgent agent)
    {

    }
    public void Enter(AiAgent agent)
    {
        timer = 0f;
        agent.navMeshAgent.isStopped = true;
        agent.animator.SetTrigger("Hit");
        agent.animator.SetFloat("Speed", 0f);
    }

    public void Update(AiAgent agent)
    {
        agent.RotateToTarget();

        if (agent.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.92f)
        {
            timer += Time.deltaTime;
        }
 
        if (timer > duration)
        {
            agent.stateMachine.ChangeState(AiStateId.BackOff);
        }
    }

    public void Exit(AiAgent agent)
    {
    }
}