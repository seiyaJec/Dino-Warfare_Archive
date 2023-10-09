using UnityEngine;

public class Triceratops_RushState : AiState
{
    private float timer;

    private DamageReceiverID.Part targetPart;
    private IGetHit getHit;

    public AiStateId GetId()
    {
        return AiStateId.Rush;
    }

    public void Init(AiAgent agent)
    {
        targetPart = DamageReceiverID.Part.Head;
        getHit = agent.GetComponent<IGetHit>();
    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.speed = agent.config.rushSpeed;
        agent.navMeshAgent.velocity = Vector3.zero;
        agent.navMeshAgent.SetDestination(agent.targetEntity.transform.position);
        agent.navMeshAgent.isStopped = false;
        getHit.OnDamageCalculate(targetPart);
        SetActiveLockUI(agent);
    }

    private void SetActiveLockUI(AiAgent agent)
    {
        DamageReceiver[] dmReceivers = agent.GetComponentsInChildren<DamageReceiver>();
        foreach (var dmReceiver in dmReceivers)
        {
            if (dmReceiver.id == targetPart && dmReceiver.transform.tag != "Enemy_Weapon")
            {
                UIManager.Instance.EnableLockImage(dmReceiver.transform, 1.25f);
            }
        }
    }

    public void Update(AiAgent agent)
    {
        float targetDistance = Vector3.Distance(agent.targetEntity.transform.position, agent.transform.position);
        if (targetDistance <= agent.config.attackDistance)
        {
            BeginAttack(agent);
            return;
        }
        if (timer < 0f)
        {
            UpdatePath(agent);
        }

        agent.RotateToTarget();

        agent.animator.SetFloat("Speed", agent.navMeshAgent.velocity.magnitude);

        timer -= Time.deltaTime;
    }

    private void UpdatePath(AiAgent agent)
    {
        timer = agent.config.updateTargetTime;
        agent.navMeshAgent.SetDestination(agent.targetEntity.transform.position);
    }

    private void BeginAttack(AiAgent agent)
    {
        agent.stateMachine.ChangeState(AiStateId.NormalAttack);
    }

    public void Exit(AiAgent agent)
    {
        UIManager.Instance.DisableAllLockImage();
        getHit.OffDamageCalculate();
    }
}

