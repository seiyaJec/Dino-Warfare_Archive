using System.Collections;
using UnityEngine;

public class Grenade_Move : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField, Min(0)] private float time = 1;
    [SerializeField] private float lifeTime = 2;
    [SerializeField] private bool limitAcceleration = false;
    [SerializeField, Min(0)] private float maxAcceleration = 100;
    [SerializeField] private Vector3 minInitVelocity;
    [SerializeField] private Vector3 maxInitVelocity;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float damage;
    [SerializeField, Tooltip("反射時のダメージ倍率")]
    private float reflectionDamageMagnification;
    [SerializeField] private float attackRaidus;

    private Vector3 position;
    private Vector3 velocity;
    private Vector3 acceleration;
    private Transform thisTransform;

    private GrenadeLauncher ownerLauncher;
    private Transform ownerTransform;
    private float currentTime;

    public Transform Target
    {
        set
        {
            target = value;
        }
        get
        {
            return target;
        }
    }

    public void SetOwnerLauncher(Transform owner)
    {
        this.ownerLauncher = owner.transform.GetComponent<GrenadeLauncher>();
        ownerTransform = owner;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, attackRaidus);
    }

    void Start()
    {
        thisTransform = transform;
        position = thisTransform.position;
        velocity = new Vector3(Random.Range(minInitVelocity.x, maxInitVelocity.x), Random.Range(minInitVelocity.y, maxInitVelocity.y), Random.Range(minInitVelocity.z, maxInitVelocity.z));
        currentTime = time;

        //UIManager.Instance.EnableLockImage(thisTransform, 0.5f);

        StartCoroutine(Timer());
    }

    public void Update()
    {
        //持ち主が死んだら弾も死ぬ
        if (ownerLauncher.OwnerDead())
        {
            foreach(Transform child in this.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            GameObject.Destroy(this.gameObject);
        }

        if (target == null)
        {
            return;
        }

        acceleration = 2f / (currentTime * currentTime) * (target.position - position - currentTime * velocity);

        if (limitAcceleration && acceleration.sqrMagnitude > maxAcceleration * maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }

        currentTime -= Time.deltaTime;

        if (currentTime < 0f)
        {
            return;
        }

        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;
        thisTransform.position = position;
        thisTransform.rotation = Quaternion.LookRotation(velocity);

        CheckCollider();
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }

    public void CheckCollider()
    {
        var colliders = Physics.OverlapSphere(transform.position, attackRaidus, targetLayer.value);//自分と無視したいものを除外するためにLayerを設定

        foreach (var collider in colliders)
        {
            var message = new DamageMessage();
            message.Amount = damage;
            message.damager = gameObject;

            bool successAttack;
            var entity = collider.GetComponent<LivingEntity>();
            if (entity != null)
            {
                successAttack = Attack_Entity(entity, message);
            }
            else
            {
                successAttack = Attack_Part(collider, message);
            }

            if (successAttack)
                Destroy(gameObject);

            break;
        }
    }

    private bool Attack_Part(Collider targetCollider, in DamageMessage damageMessage)
    {
        var reciever = targetCollider.GetComponent<DamageReceiver>();

        if (reciever == null)
            return false;

        reciever.SendDamage(damageMessage);
        return true;
    }
    private bool Attack_Entity(LivingEntity entity, in DamageMessage damageMessage)
    {
        if (entity == null || entity.dead)
            return false;

        return entity.ApplyDamage(damageMessage);
    }

    public void Reflection()
    {
        //ターゲット変更
        Target = ownerTransform.transform;
        //レイヤー変更 192は敵のレイヤーマスク(変数とか使うとうまくいかないからいったん直で入れてる)
        targetLayer = enemyLayer;

        //速度リセット
        velocity = new Vector3(Random.Range(minInitVelocity.x, maxInitVelocity.x), Random.Range(minInitVelocity.y, maxInitVelocity.y), Random.Range(minInitVelocity.z, maxInitVelocity.z));
        currentTime = time;

        //タイマー最初から
        StopCoroutine(Timer());
        StartCoroutine(Timer());

        //ダメージ変更
        damage *= reflectionDamageMagnification;
    }
}