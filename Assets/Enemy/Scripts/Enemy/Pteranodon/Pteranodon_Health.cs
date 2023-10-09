public class Pteranodon_Health : LivingEntity, IGetHit
{
    private bool isCalculating = false;
    private DamageReceiverID.Part part;
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
        if (deathState != null)
        {
            //
        }
        agent.stateMachine.ChangeState(AiStateId.Death);
    }

    public override void ReceiveDamage(DamageMessage damageMessage, DamageReceiverID.Part part)
    {
        base.ReceiveDamage(damageMessage, part);

        if (isCalculating && part == this.part)
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
        this.part = part;
    }

    public void OffDamageCalculate()
    {
        accumulateAmount = 0;
        isCalculating = false;
    }
}
