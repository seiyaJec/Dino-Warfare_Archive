using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class Pteranodon_BackOffState : AiState
{
    Vector3 backOffPos;
    Transform weaponTransform;
    public Pteranodon_BackOffState(in Vector3 backOffPos, Transform weaponTransform)
    {
        this.backOffPos = backOffPos;
        this.weaponTransform = weaponTransform;
    }

    //上昇する対象の情報
    struct RiseTargetInfo
    {
        public Transform transform;  //Transform
        public float     initHeight; //初期高度
    }
    List<RiseTargetInfo> riseTargetInfos;

    public AiStateId GetId()
    {
        return AiStateId.BackOff;
    }

    public void Init(AiAgent agent)
    {
        CacheRiseTransforms(agent);
    }

    //上昇するオブジェクトの情報をキャッシュ
    void CacheRiseTransforms(in AiAgent agent)
    {
        if (riseTargetInfos == null)
            riseTargetInfos = new List<RiseTargetInfo>();

        RegisterRiseTargetInfo(agent.model.transform);
        RegisterRiseTargetInfo(agent.eyeTransform);
        RegisterRiseTargetInfo(agent.attackRoot);
        RegisterRiseTargetInfo(weaponTransform);
    }
    //上昇するオブジェクトの情報を登録
    void RegisterRiseTargetInfo(in Transform transform)
    {
        RiseTargetInfo riseTargetInfo;
        riseTargetInfo.transform = transform;
        riseTargetInfo.initHeight = transform.position.y;
        riseTargetInfos.Add(riseTargetInfo);
    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
        agent.navMeshAgent.SetDestination(backOffPos);
        agent.navMeshAgent.speed = agent.config.runSpeed;

        StartRise();
    }

    void StartRise()
    {
        float riseTime = 1.0f;
        foreach (RiseTargetInfo targetInfo in riseTargetInfos)
        {
            targetInfo.transform.DOMoveY(targetInfo.initHeight, riseTime);
        }
    }

    public void Update(AiAgent agent)
    {
        if (agent.navMeshAgent.velocity.sqrMagnitude < 0.2f && agent.navMeshAgent.remainingDistance < 0.5f)
        {
            agent.stateMachine.ChangeState(AiStateId.Stanby);
        }

        agent.Rotate();

        agent.animator.SetFloat("Speed", -1 * agent.navMeshAgent.velocity.magnitude);
    }

    public void Exit(AiAgent agent)
    {
        CanselRise();
    }

    //上昇中止
    void CanselRise()
    {
        foreach(RiseTargetInfo targetInfo in riseTargetInfos)
        {
            targetInfo.transform.DOKill();
        }
    }
}