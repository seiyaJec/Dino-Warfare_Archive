using System.Collections;
using UnityEngine;

public class HomingMissile_Move : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField, Min(0)] private float time = 1;
    [SerializeField] private float lifeTime = 2;
    [SerializeField] private bool limitAcceleration = false;
    [SerializeField, Min(0)] private float maxAcceleration = 100;
    [SerializeField] private Vector3 minInitVelocity;
    [SerializeField] private Vector3 maxInitVelocity;

    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private float damage;
    [SerializeField] private float attackRaidus;

    private Vector3 position;
    private Vector3 velocity;
    private Vector3 acceleration;
    private Transform thisTransform;

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

        UIManager.Instance.EnableLockImage(thisTransform, 0.5f);

        StartCoroutine(Timer());
    }

    public void Update()
    {
        if (target == null)
        {
            return;
        }
        
        acceleration = 2f / (time * time) * (target.position - position - time * velocity);

        if (limitAcceleration && acceleration.sqrMagnitude > maxAcceleration * maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }

        time -= Time.deltaTime;

        if (time < 0f)
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
        var colliders = Physics.OverlapSphere(transform.position, attackRaidus, targetLayer);
        foreach (var collider in colliders)
        {
            var attackTargetEntity = collider.GetComponent<LivingEntity>();

            if (attackTargetEntity != null && !attackTargetEntity.dead)
            {
                var message = new DamageMessage();
                message.Amount = damage;
                message.damager = gameObject;

                attackTargetEntity.ApplyDamage(message);
                Destroy(gameObject);
                break;
            }
        }
    }

}