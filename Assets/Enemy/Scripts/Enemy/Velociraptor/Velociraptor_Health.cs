public class Velociraptor_Health : LivingEntity
{
    private void Start()
    {

    }

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

        agent.Die();
        agent.stateMachine.ChangeState(AiStateId.Death);
    }

}
