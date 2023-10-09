public class HomingMissile_Health : LivingEntity
{
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        return true;
    }

    public override void Die()
    {
        base.Die();

        EffectManager.Instance.PlayEffect(transform.position, EffectManager.EffectType.Explosion);
        Destroy(gameObject);
    }

    public override void ReceiveDamage(DamageMessage damageMessage, DamageReceiverID.Part id)
    {
        ApplyDamage(damageMessage);
    }
}
