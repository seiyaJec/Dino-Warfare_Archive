/*this code is written in SHIFT-JIS*/
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    //Life Debug
    [SerializeField] private bool isVulnerable;

    private CameraShake cameraShake;

    //------------------
    //ci0329
    [SerializeField]  public float revNonHitTime;
    [HideInInspector] private bool nonHit;
    [HideInInspector] private float revNonHitCount;
    [HideInInspector] public int deadCount;
    //------------------

    private void Start()
    {
        cameraShake= GetComponentInChildren<CameraShake>();
        OnDeath += Stanby;
        OnDeath += SetAtctiveContinue;
        revNonHitCount = 0;
        deadCount = 0;
        nonHit = false;
    }

    private void SetAtctiveContinue()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CheckContinue();
        }
    }

    public void Stanby()
    {
        GetComponentInChildren<CursorMove>().enabled = false;
        GetComponentInChildren<CameraRotation>().enabled = false;
        GetComponent<PlayerMove>().enabled = false;
        GetComponent<PlayerScript>().enabled = false;


        ++deadCount;
    }

    //ダメージを享受する
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (isVulnerable) return false;
        if (nonHit) return false;

        if (!base.ApplyDamage(damageMessage))
            return false;


        cameraShake.Shake(0.25f, 0.07f);

        SoundManager.Instance.Play("Sounds/SFX/Player/damage_1", SoundManager.Sound.EFFECT);

        UIManager.Instance.UpdateHealth(currentHealth);
        UIManager.Instance.PlayHitUIAnimation(transform,damageMessage.damager.transform,dead);
        return true;
    }


    //------------------------------------------------------------
    //ci0329
    //復活
    public void Revival()
    {
        GetComponentInChildren<CursorMove>().enabled = true;
        GetComponentInChildren<CameraRotation>().enabled = true;
        GetComponent<PlayerMove>().enabled = true;

        var playersc = GetComponent<PlayerScript>();
        playersc.enabled = true;
        playersc.SetRevival();

        revNonHitCount = 0;
        nonHit = true;
        dead = false;
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealth(currentHealth);
    }

    private void Update()
    {
        if (nonHit == true)
        {
            revNonHitCount += Time.deltaTime;
            Debug.Log("無敵時間残り" + (revNonHitTime - revNonHitCount) + "秒");
            if(revNonHitCount >= revNonHitTime)
            {
                nonHit = false;
            }
        }
    }
    //------------------------------------------------------------
}
