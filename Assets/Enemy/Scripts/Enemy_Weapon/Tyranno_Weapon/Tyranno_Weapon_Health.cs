using Unity.Burst.CompilerServices;
using UnityEngine;

public class Tyranno_Weapon_Health : LivingEntity, IGetHit
{
    private MeshRenderer model;
    private Collider hitCollider;

    private bool isCalculating = false;
    private float accumulateAmount;

    private Tyranno_Weapon weapon;

    private void Start()
    {
        model = GetComponentInChildren<MeshRenderer>();
        hitCollider = GetComponentInChildren<Collider>();
        weapon = GetComponent<Tyranno_Weapon>();
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
        //weapon.enabled = false;
    }


    public override void ReceiveDamage(DamageMessage damageMessage, DamageReceiverID.Part part)
    {
        ApplyDamage(damageMessage);

        EffectManager.Instance.PlayEffect(damageMessage.hitPoint, damageMessage.hitNormal,
            EffectManager.EffectType.WeaponHit);

        if (isCalculating)
        {
            accumulateAmount += damageMessage.Amount;

            if (accumulateAmount > weapon.hitAmount)
            {
                EffectManager.Instance.PlayEffect(transform.position, EffectManager.EffectType.Explosion);
                weapon.StopCharge();
            }
        }
    }

    public void OnDamageCalculate(DamageReceiverID.Part part)
    {
        isCalculating = true;
    }

    public void OffDamageCalculate()
    {
        accumulateAmount = 0;
        isCalculating = false;
    }
}