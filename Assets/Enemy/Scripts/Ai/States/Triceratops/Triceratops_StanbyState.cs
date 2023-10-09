using UnityEngine;

public class Triceratops_StanbyState : AiState
{
    private enum ActionWeight
    {
        MoveLeft = 40,
        MoveRIght = 40,
        RangeAttack = 35,
        RushAttack = 15,
    }

    private float timer;

    private int actionCount = 0;

    private bool isAttackPosSet = false;

    public AiStateId GetId()
    {
        return AiStateId.Stanby;

    }

    public void Init(AiAgent agent)
    {

    }

    public void Enter(AiAgent agent)
    {
        if (actionCount > 0)
        {
            timer = agent.config.stanbyTime - 0.15f;
        }
        else
        {
            timer = Random.Range(0.5f, agent.config.stanbyTime);
        }

        agent.navMeshAgent.isStopped = true;
        agent.navMeshAgent.velocity = Vector3.zero;

        if (!isAttackPosSet)
        {
            isAttackPosSet = true;
            agent.config.attackPosition = agent.transform.position;
        }
    }

    public void Update(AiAgent agent)
    {
        timer += Time.deltaTime;
        if (timer > agent.config.stanbyTime)
        {
            SetNextAction(agent);
            return;
        }

        agent.RotateToTarget();

        agent.animator.SetFloat("Speed", 0f);
    }

    private void SetNextAction(AiAgent agent)
    {
        if (actionCount >= 4)
        {
            actionCount = 0;
            agent.stateMachine.ChangeState(AiStateId.Rush);
            return;
        }

        int randomActionNum = Random.Range(0, 4);

        switch (randomActionNum)
        {
            case 0:
                ++actionCount;
                agent.stateMachine.ChangeState(AiStateId.MoveLeft);
                break;
            case 1:
                ++actionCount;
                agent.stateMachine.ChangeState(AiStateId.MoveRight);
                break;
            case 2:
                ++actionCount;
                agent.stateMachine.ChangeState(AiStateId.RangedAttack);
                break;
            case 3:
                actionCount = 0;
                agent.stateMachine.ChangeState(AiStateId.Rush);
                break;
            default:
                break;
        }
        return;
    }

    public void Exit(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }
}
