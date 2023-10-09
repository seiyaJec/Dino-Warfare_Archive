using Unity.Burst.CompilerServices;
using UnityEngine;

public class Triceratops_Weapon_Health : LivingEntity
{
    private MeshRenderer model;
    private Collider hitCollider;

    private void Start()
    {
        model = GetComponentInChildren<MeshRenderer>();
        hitCollider = GetComponentInChildren<Collider>();
    }


    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        return true;
    }

    public override void Die()
    {
        base.Die();

        EffectManager.Instance.PlayEffect(transform.position, EffectManager.EffectType.Explosion);

        model.enabled = false;
        hitCollider.enabled = false;
    }

    public override void ReceiveDamage(DamageMessage damageMessage, DamageReceiverID.Part id)
    {
        ApplyDamage(damageMessage);

        EffectManager.Instance.PlayEffect(damageMessage.hitPoint, damageMessage.hitNormal,
            EffectManager.EffectType.WeaponHit);
    }
}