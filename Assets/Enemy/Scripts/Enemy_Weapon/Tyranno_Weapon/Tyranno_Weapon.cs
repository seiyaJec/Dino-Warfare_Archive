using UnityEngine;
using System.Collections;

public class Tyranno_Weapon : MonoBehaviour
{
    private Transform fireTransform;
    private LivingEntity target;

    private Tyranno_Weapon_Health health;
    private IGetHit getHit;

    private IEnumerator chargeProcess;

    [SerializeField] private ParticleSystem gathering;

    [field: SerializeField] public float hitAmount { get; private set; }
    [SerializeField] private float damage = 10.0f;
    [field: SerializeField] public float chargeDuration { get; private set; }

    public bool dead
    {
        get { return health.dead; }
    }

    private void Awake()
    {
        health = GetComponent<Tyranno_Weapon_Health>();
        getHit = GetComponent<IGetHit>();

        health.OnDeath += SetOff;

        fireTransform = transform.Find("FireTransform");
    }


    public void StartCharge(LivingEntity target)
    {
        this.target = target;
        chargeProcess = ChargeProcess();
        StartCoroutine(chargeProcess);
    }

    public void StopCharge()
    {
        if (chargeProcess != null)
        {
            StopCoroutine(chargeProcess);
        }
        SetOff();
    }

    private void SetOff()
    {
        gathering.gameObject.SetActive(false);
        health.OffDamageCalculate();

        UIManager.Instance.DisableSignalImage(fireTransform);
    }

    private IEnumerator ChargeProcess()
    {
        float timer = 0f;

        gathering.gameObject.SetActive(true);
        getHit.OnDamageCalculate();

        UIManager.Instance.EnableSignalImage(fireTransform, chargeDuration);

        while (timer < chargeDuration)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Fire();
    }

    private void Fire()
    {
        var message = new DamageMessage();
        message.Amount = damage;
        message.damager = gameObject;

        target.GetComponent<LivingEntity>().ApplyDamage(message);

        SetOff();

        SoundManager.Instance.Play("Sounds/SFX/Player/Attack/beam_2", SoundManager.Sound.EFFECT);
    }
}
