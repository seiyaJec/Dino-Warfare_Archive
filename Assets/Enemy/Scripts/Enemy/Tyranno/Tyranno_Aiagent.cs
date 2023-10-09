using UnityEngine;

public class Tyranno_Aiagent : AiAgent
{
    private Tyranno_Weapon[] weapons;

    protected override void Awake()
    {
        base.Awake();

        weapons = GetComponentsInChildren<Tyranno_Weapon>();
    }

    protected override void Start()
    {
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new Tyranno_ChaseState());
        stateMachine.RegisterState(new Tyranno_GetHitState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiNormalAttackState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.ChangeState(initialState);

        turnSmoothTime = 0.2f;

        navMeshAgent.speed = config.runSpeed;

        animationEvents.AnimationEvent.AddListener(OnAnimationEvent);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void GetHit()
    {
        stateMachine.ChangeState(AiStateId.GetHit);
    }

    protected override void OnAnimationEvent(string evenName)
    {
        base.OnAnimationEvent(evenName);
        switch (evenName)
        {
            case "RangeAttack":
                RangeAttack();
                break;
            case "FootStep":
                SoundManager.Instance.Play("Sounds/SFX/Enemy/footStep_1", SoundManager.Sound.EFFECT);
                break;
            default:
                break;
        }
    }

    private void RangeAttack()
    {
        Debug.Log("range");
        foreach (var weapon in weapons)
        {
            Debug.Log(weapon.dead);
            if (weapon == null || weapon.dead) { continue; }
            weapon.StartCharge(targetEntity);
        }
    }

    public override void Die()
    {
        foreach (var weapon in weapons)
        {
            if (weapon == null || weapon.dead) { continue; }
            weapon.StopCharge();
            weapon.gameObject.SetActive(false);
        }
    }
}
