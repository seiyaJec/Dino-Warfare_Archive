using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pteranodon_RushState : AiState
{
    private float timer;

    private DamageReceiverID.Part targetPart;
    private IGetHit getHit;

    //降下情報
    struct DescentTransformInfo
    {
        public Transform transform;     //対象のtransform
        public Vector3 basePosOffset;   //基準点からのオフセット
    }
    List<DescentTransformInfo> descentTransformInfos;
    Transform weaponTransform;

    public AiStateId GetId()
    {
        return AiStateId.Rush;
    }

    public Pteranodon_RushState(Transform weaponTransform)
    {
        this.weaponTransform = weaponTransform;
    }

    public void Init(AiAgent agent)
    {
        targetPart = DamageReceiverID.Part.Head;
        getHit = agent.GetComponent<IGetHit>();

        CacheDescentTransforms(agent);
    }

    //降下するオブジェクトのTransformをキャッシュ
    void CacheDescentTransforms(in AiAgent agent)
    {
        Vector3 basePos = agent.model.transform.position;

        descentTransformInfos = new List<DescentTransformInfo>();
        RegisterDescentTransformInfo(agent.model.transform, basePos);
        RegisterDescentTransformInfo(agent.eyeTransform, basePos);
        RegisterDescentTransformInfo(agent.attackRoot, basePos);
        RegisterDescentTransformInfo(weaponTransform, basePos);
    }
    //降下情報を登録
    void RegisterDescentTransformInfo(Transform transform, in Vector3 basePos)
    {
        DescentTransformInfo info;
        info.transform = transform;
        info.basePosOffset = transform.position - basePos;
        descentTransformInfos.Add(info);
    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.speed = agent.config.rushSpeed;
        agent.navMeshAgent.velocity = Vector3.zero;

        SetDestination(agent);

        agent.navMeshAgent.isStopped = false;

        getHit.OnDamageCalculate(targetPart);

        //SetActiveLockUI(agent);

        //降下開始
        StartDescent(agent);

        //agent.animator.SetTrigger("Attack");
    }

    //降下開始
    public void StartDescent(in AiAgent agent)
    {
        float descentOffset = -1.5f;
        float descentTime = 1.0f;

        foreach (DescentTransformInfo info in descentTransformInfos)
        {
            info.transform.DOMoveY(agent.targetEntity.transform.position.y + descentOffset + info.basePosOffset.y, descentTime);
        }
    }

    //目的地設定
    private void SetDestination(AiAgent agent)
    {
        Vector3 targetDir = (agent.targetEntity.transform.position - agent.navMeshAgent.transform.position).normalized;
        float distance = agent.config.attackDistance;
        agent.navMeshAgent.SetDestination(agent.targetEntity.transform.position - (targetDir * distance));//ちょっと手前で攻撃してくるように
    }

    //private void SetActiveLockUI(AiAgent agent)
    //{
    //    DamageReceiver[] dmReceivers = agent.GetComponentsInChildren<DamageReceiver>();
    //    Debug.Log("dmReceivers" + dmReceivers.ToString());
    //    foreach (var dmReceiver in dmReceivers)
    //    {
    //        if (dmReceiver.id == targetPart && dmReceiver.transform.tag != "Enemy_Weapon")
    //        {
    //            UIManager.Instance.EnableLockImage(dmReceiver.transform, 1.25f);
    //        }
    //    }
    //}

    public void Update(AiAgent agent)
    {
        float targetDistance = Vector3.Distance(agent.navMeshAgent.destination, agent.transform.position);

        if (targetDistance <= agent.config.attackDistance)
        {
            BeginAttack(agent);
            agent.animator.SetTrigger("Attacked");
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
        SetDestination(agent);
    }

    private void BeginAttack(AiAgent agent)
    {
        agent.stateMachine.ChangeState(AiStateId.NormalAttack);
    }

    public void Exit(AiAgent agent)
    {
        UIManager.Instance.DisableAllLockImage();
        getHit.OffDamageCalculate();

        CanselDescent(agent);
    }

    //降下中止
    private void CanselDescent(AiAgent agent)
    {
        foreach (DescentTransformInfo info in descentTransformInfos)
        {
            info.transform.DOKill();
        }
    }
}

