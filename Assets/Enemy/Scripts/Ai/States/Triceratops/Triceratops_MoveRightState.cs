using UnityEngine;

public class Triceratops_MoveRightState : AiState
{
    private float timer;

    public AiStateId GetId()
    {
        return AiStateId.MoveRight;
    }

    public void Init(AiAgent agent)
    {

    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = true;
        agent.animator.SetBool("MoveRight", true);

        timer = Random.Range(1.0f, 3.0f);
    }

    public void Update(AiAgent agent)
    {
        agent.RotateToTarget();

        agent.transform.Translate(Vector3.right * agent.config.runSpeed * Time.deltaTime * 0.5f);

        if (timer < 0)
        {
            agent.stateMachine.ChangeState(AiStateId.Stanby);
        }

        timer -= Time.deltaTime;
    }

    public void Exit(AiAgent agent)
    {
        agent.animator.SetBool("MoveRight", false);
    }
}


