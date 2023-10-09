using System;
using UnityEngine;

//体力を持つすべてのオブジェクトのベースクラス
public class LivingEntity : MonoBehaviour, IDamageable
{
    [SerializeField] protected float maxHealth = 100f;
    public float currentHealth { get; protected set; }
    public bool dead { get; protected set; }

    [HideInInspector] public event Action OnDeath;
    protected AiAgent agent;

    private const float minTimeBetDamaged = 0.1f; //攻撃を受けて次の攻撃を受けるまでの時間
    private float lastDamagedTime; //最後に攻撃を受けた時間

    protected void Awake()
    {
        agent = GetComponent<AiAgent>();
    }

    //攻撃を受けてから十分な時間がたっているかチェック
    protected bool IsInvulnerabe
    {
        get
        {
            if (Time.time >= lastDamagedTime + minTimeBetDamaged) return false;

            return true;
        }
    }

    protected virtual void OnEnable()
    {
        dead = false;
        currentHealth = maxHealth;
    }

    public virtual void ReceiveDamage(DamageMessage damageMessage, DamageReceiverID.Part id)
    {
        foreach (var myId in agent.config.damageReceiverIds)
        {
            if (myId.id == id)
            {
                damageMessage.Amount *= myId.damageRate;
                break;
            }
        }
        ApplyDamage(damageMessage);
    }

    //ダメージを受ける
    public virtual bool ApplyDamage(DamageMessage damageMessage) //IDamageable
    {
        if (IsInvulnerabe || damageMessage.damager == gameObject || dead) return false; 

        lastDamagedTime = Time.time;
        currentHealth -= damageMessage.Amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        return true;
    }
    //体力回復
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead) return;

        currentHealth += newHealth;
    }
    //Die
    public virtual void Die()
    {
        if (OnDeath != null) OnDeath();

        dead = true;
    }
}