//ダメージを受けられるインタフェース
public interface IDamageable
{
    bool ApplyDamage(DamageMessage damageMessage);
}