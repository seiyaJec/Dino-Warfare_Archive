using UnityEngine;

public class Velociraptor_Range_Aiagent : AiAgent
{
    [HideInInspector] public Tyranno_Weapon weapon;

    [field: SerializeField] public bool patrol;

    protected override void Awake()
    {
        base.Awake();

        weapon = GetComponentInChildren<Tyranno_Weapon>();
    }

    protected override void Start()
    {
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new VelociraptorR_IdleState());
        stateMachine.RegisterState(new Velociraptor_Patrol());
        stateMachine.RegisterState(new Velociraptor_Detour_State());
        stateMachine.RegisterState(new Velociraptor_ChaseState());
        stateMachine.RegisterState(new Velociraptor_RangeAttack());
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

    public override void Die()
    {
        if (weapon == null || weapon.dead) return;

        weapon.StopCharge();

        weapon.gameObject.SetActive(false);
    }

    private void RangeAttack()
    {
        if (weapon == null || weapon.dead || !weapon.gameObject.activeSelf) { return; }
        weapon.StartCharge(targetEntity);
    }
}
