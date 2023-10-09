using UnityEngine;

public class AiDeathState : AiState
{
    private float timer;

    public AiStateId GetId()
    {
        return AiStateId.Death;
    }

    public void Init(AiAgent agent)
    {

    }

    public void Enter(AiAgent agent)
    {
        timer = 0.0f;
        agent.navMeshAgent.enabled = false;
        agent.animator.SetTrigger("Die");

        SoundManager.Instance.Play("Sounds/SFX/Enemy/roar_3", SoundManager.Sound.EFFECT);
    }

    public void Update(AiAgent agent)
    {
        if (agent.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            timer += Time.deltaTime;
        }

        if (timer > agent.config.deathDuration)
        {
            agent.animator.enabled = false;
            agent.model.SetActive(false);
            foreach (var c in agent.hitColliders) c.enabled = false;
            agent.enabled = false;
        }
    }

    public void Exit(AiAgent agent)
    {

    }
}
