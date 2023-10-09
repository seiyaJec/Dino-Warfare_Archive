using System.Collections.Generic;
using UnityEngine;

public class Pteranodon_DeathState : AiState
{
    private float timer;
    Transform weaponTransform;

    struct FallTransformInfo
    {
        public Transform transform;
        public Vector3 basePosOffset;
    }
    List<FallTransformInfo> fallTransformInfos;
    void RegisterFallTransformInfo(Transform transform, Vector3 basePos)
    {
        FallTransformInfo info;
        info.transform = transform;
        info.basePosOffset = transform.position - basePos;

        fallTransformInfos.Add(info);
    }
    private float fallSpeed;

    public Pteranodon_DeathState(in float fallSpeed, Transform weaponTransform)
    {
        this.fallSpeed = fallSpeed;
        this.weaponTransform = weaponTransform;
    }

    public AiStateId GetId()
    {
        return AiStateId.Death;
    }

    public void Init(AiAgent agent)
    {
        fallTransformInfos = new List<FallTransformInfo>();
        //降下するオブジェクトのTransformをキャッシュ
        Vector3 basePos = agent.model.transform.position;
        RegisterFallTransformInfo(agent.attackRoot, basePos);
        RegisterFallTransformInfo(agent.model.transform, basePos);
        RegisterFallTransformInfo(agent.eyeTransform, basePos);
        RegisterFallTransformInfo(weaponTransform, basePos);
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

        FallToGround(agent);

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

    private void FallToGround(AiAgent agent)
    {
        float groundHeight = agent.navMeshAgent.transform.position.y;
        float fallSpeed_ThisFrame = fallSpeed * Time.deltaTime;
        
        foreach(var fallInfo in fallTransformInfos)
        {
            Vector3 targetPos = fallInfo.transform.position;

            float distanceToGround = targetPos.y - groundHeight - fallInfo.basePosOffset.y;

            if (distanceToGround > 0.0f)
                targetPos.y -= fallSpeed_ThisFrame;

            fallInfo.transform.position = targetPos;
        }
    }

}
