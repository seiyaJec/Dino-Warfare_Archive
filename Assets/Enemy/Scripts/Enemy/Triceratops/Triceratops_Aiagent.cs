using UnityEngine;

public class Triceratops_Aiagent : AiAgent
{
    private Triceratops_Weapon[] weapons;

    public bool disableRangeAttack
    {
        get
        {
            foreach (Triceratops_Weapon weapon in weapons)
            {
                if (weapon.dead) return true;
            }
            return false;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        weapons  = GetComponentsInChildren<Triceratops_Weapon>();
    }

    protected override void Start()
    {
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new Triceratops_ChasePosition());
        stateMachine.RegisterState(new TriceratopsAttackState());
        stateMachine.RegisterState(new TriceratopsRangeAttack());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new Triceratops_BackOffState());
        stateMachine.RegisterState(new Triceratops_MoveRightState());
        stateMachine.RegisterState(new Triceratops_MoveLeftState());
        stateMachine.RegisterState(new Triceratops_GetHitState());
        stateMachine.RegisterState(new Triceratops_StanbyState());
        stateMachine.RegisterState(new Triceratops_RushState());
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
            default:
                break;
        }
    }

    private void RangeAttack()
    {
        Transform targetTransform = targetEntity.transform;
        foreach (var weapon in weapons)
        {
            if (weapon == null || weapon.dead) { continue; }
            weapon.Fire(targetTransform);
        }
    }

    public override void Die()
    {
        foreach (var weapon in weapons)
        {
            if (weapon == null || weapon.dead) { continue; }
            weapon.gameObject.SetActive(false);
        }
    }
}
