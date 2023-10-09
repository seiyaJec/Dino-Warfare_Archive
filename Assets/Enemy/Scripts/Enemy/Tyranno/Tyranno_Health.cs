using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Tyranno_Aiagent))]
public class Tyranno_Health : LivingEntity, IGetHit
{
    private bool isCalculating = false;

    private List<DamageReceiverID.Part> weakPoints = new List<DamageReceiverID.Part>();

    private float accumulateAmount;

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        if (agent.targetEntity == null && damageMessage.damager != null)
        {
            agent.targetEntity = damageMessage.damager.GetComponent<LivingEntity>();
        }

        EffectManager.Instance.PlayEffect(damageMessage.hitPoint, damageMessage.hitNormal,
            EffectManager.EffectType.Hit);

        return true;
    }

    public override void Die()
    {
        base.Die();

        AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeathState;

        if (deathState == null || !agent) { return; }
        agent.Die();
        agent.stateMachine.ChangeState(AiStateId.Death);

        //
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameClear();
        }
    }

    public override void ReceiveDamage(DamageMessage damageMessage, DamageReceiverID.Part part)
    {
        base.ReceiveDamage(damageMessage, part);

        if (isCalculating && weakPoints.Contains(part))
        {
            accumulateAmount += damageMessage.Amount;

            if (accumulateAmount >= agent.config.p1)
            {
                agent.GetHit();
                OffDamageCalculate();
            }
        }
    }

    public void OnDamageCalculate(DamageReceiverID.Part part)
    {
        isCalculating = true;
        weakPoints.Add(part);
    }

    public void OffDamageCalculate()
    {
        accumulateAmount = 0;
        weakPoints.Clear();
        isCalculating = false;
    }
}