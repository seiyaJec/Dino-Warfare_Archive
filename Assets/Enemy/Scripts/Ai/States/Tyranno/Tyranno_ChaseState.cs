using UnityEngine;

public class Tyranno_ChaseState : AiState
{
    private float timer = 0f;

    private float rangeAttackTimer = 0f;
    //private float tackleTImer = 0f;

    private float timeBetRangeAttack = 4.7f;
    //private float timeBetTackle = 6.5f;

    [Range(0f, 2.0f)] private float speedControl;
    private float maxDistance = 26.0f;
    private DamageReceiverID.Part[] targetPart = { DamageReceiverID.Part.Head };

    private IGetHit getHit;

    public AiStateId GetId()
    {
        return AiStateId.Chase;
    }

    public void Init(AiAgent agent)
    {
        getHit = agent.GetComponent<IGetHit>();
    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
        foreach (var part in targetPart) { getHit.OnDamageCalculate(part); }
        SetActiveLockUI(agent);
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


        SetSpeedControl(agent, targetDistance);

        if (timer < 0f)
        {
            timer = agent.config.updateTargetTime;
            agent.UpdatePath();
        }

        if (rangeAttackTimer > timeBetRangeAttack)
        {
            BgeinRangeAttack(agent);
        }
        /*
        if (tackleTImer > timeBetTackle)
        {
            agent.animator.SetTrigger("Tackle");
            tackleTImer = 0f;
        }
        */

        agent.Rotate();

        UpdateTimer(agent, targetDistance);

        if (agent.navMeshAgent.isOnOffMeshLink && !agent.animator.GetCurrentAnimatorStateInfo(0).IsName("Leap"))
        {
            agent.animator.SetTrigger("Leap");
        }
        agent.animator.SetFloat("Speed", agent.navMeshAgent.velocity.magnitude);

        agent.navMeshAgent.speed = agent.config.runSpeed * speedControl;
    }

    private void UpdateTimer(AiAgent agent, float targetDistance)
    {
        timer -= Time.deltaTime;
        if (targetDistance > agent.config.attackDistance * 1.4f && agent.IsTargetOnSight(agent.targetEntity.transform))
        {
            rangeAttackTimer += Time.deltaTime;
        }
        /*
        if (targetDistance > agent.config.attackDistance * 1.5f)
        {
            tackleTImer += Time.deltaTime;
        }
        */
    }

    private void SetSpeedControl(AiAgent agent, float targetDistance)
    {
        if (targetDistance > maxDistance && agent.IsTargetOnSight(agent.targetEntity.transform))
        {
            speedControl = ((targetDistance - maxDistance) + agent.config.runSpeed) / agent.config.runSpeed;
        }
        else
        {
            speedControl = 1.0f;
        }
    }

    public void BeginAttack(AiAgent agent)
    {
        agent.stateMachine.ChangeState(AiStateId.NormalAttack);
    }

    private void BgeinRangeAttack(AiAgent agent)
    {
        SoundManager.Instance.Play("Sounds/SFX/Enemy/roar_4", SoundManager.Sound.EFFECT);
        agent.animator.SetTrigger("RangeAttack");
        rangeAttackTimer = 0f;
    }

    public void Exit(AiAgent agent)
    {
        UIManager.Instance.DisableAllLockImage();
        agent.GetComponent<IGetHit>().OffDamageCalculate();
    }

    private void SetActiveLockUI(AiAgent agent)
    {
        if (targetPart.Length < 1) { return; }

        DamageReceiver[] dmReceivers = agent.GetComponentsInChildren<DamageReceiver>();
        foreach (var dmReceiver in dmReceivers)
        {
            if (dmReceiver.transform.tag != "Enemy_Weapon")
            {
                if (dmReceiver.id == targetPart[0])
                {
                    UIManager.Instance.EnableLockImage(dmReceiver.transform, 0.6f);
                }
            }
        }
    }

}
