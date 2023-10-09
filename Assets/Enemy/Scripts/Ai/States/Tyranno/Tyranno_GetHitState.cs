using UnityEngine;

public class Tyranno_GetHitState : AiState
{
    private float timer = 0f;
    private float duration = 1.25f;


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

        SoundManager.Instance.Play("Sounds/SFX/Enemy/roar_0", SoundManager.Sound.EFFECT);
    }

    public void Update(AiAgent agent)
    {
        agent.RotateToTarget();
        timer += Time.deltaTime;
        if (timer > duration && agent.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.92f)
        {
            agent.stateMachine.ChangeState(AiStateId.Chase);
        }

    }

    public void Exit(AiAgent agent)
    {
    }
}