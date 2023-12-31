public class Grenade_Health : LivingEntity
{
    private bool isReflected;

    private void Awake()
    {
        base.Awake();
        isReflected = false;
    }
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        return true;
    }

    public override void Die()
    {
        //死にはしないけど、反射する(反射した後はいったん無敵みたいになる)
        if (isReflected)
            return;

        GetComponent<Grenade_Move>().Reflection();
        isReflected = true;
    }

    public override void ReceiveDamage(DamageMessage damageMessage, DamageReceiverID.Part id)
    {
        ApplyDamage(damageMessage);
    }
}
