using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_PlayerShooter : MonoBehaviour
{
    private AudioSource adSoruce;
    [SerializeField] private AudioClip shotSound;

    private RaycastHit hit;

    [SerializeField] private float damage;
    [SerializeField] private LayerMask targetLayer;

    private Camera mainCam;

    private float lastFireTime;
    private float timeBetFire = 0.12f;

    private void Awake()
    {
        adSoruce = GetComponent<AudioSource>();
        mainCam = Camera.main;
    }

    private void Start()
    {
        SetAllEnemiesTarget();
    }

    void Update()
    { 

        bool isFiring = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space);

        if (isFiring && 
           Time.time > lastFireTime + timeBetFire)
        {
            Shot();
        }
    }

    private void Shot()
    {
        lastFireTime = Time.time;
        Debug.Log("fire");
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 10000.0f, targetLayer))
        {
            var target = hit.collider.GetComponent<DamageReceiver>();

            if (target)
            {
                var message = new DamageMessage();
                message.Amount = damage;
                message.damager = gameObject;
                message.hitPoint = hit.point;
                message.hitNormal = hit.normal;

                target.SendDamage(message);
            }
            else
                EffectManager.Instance.PlayEffect(hit.point, hit.normal);
        }

        adSoruce.PlayOneShot(shotSound);
    }

    
    private void SetAllEnemiesTarget()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var obj in objs)
        {
            var aiAgent = obj.GetComponent<AiAgent>();
            if (aiAgent)
            {
                aiAgent.targetEntity = gameObject.GetComponent<LivingEntity>();
            }
        }
    }
    
}
