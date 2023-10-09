using UnityEngine;

//敵の部位についてダメージを受けて伝えるクラス
public class DamageReceiver : MonoBehaviour
{
    [field: SerializeField] public DamageReceiverID.Part id { get; private set; }

    private LivingEntity enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<LivingEntity>();
    }

    public void SendDamage(DamageMessage damageMessage)
    {
        if (!enemy)
        {
            return;
        }

        enemy.ReceiveDamage(damageMessage, id);
        //Debug.Log(id.ToString());
    }

    //------------------------------------------------------
    //ci0329
    //敵がすでに死亡しているか
    public bool IsDead()
    {
        return enemy.dead;
    }

    //------------------------------------------------------
}
