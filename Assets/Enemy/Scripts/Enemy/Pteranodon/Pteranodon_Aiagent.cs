using UnityEngine;

public class Pteranodon_Aiagent : AiAgent
{
    [SerializeField]
    private Transform _standardPosition;
    

    [field: Header("死亡")]
    [SerializeField, Tooltip("死亡時の落下速度")]
    private float _death_fallSpeed;

    
    [field:Header("旋回")]
    [SerializeField, Tooltip("旋回時間(秒)")]
    private float _stanby_turningTime_sec;
    [SerializeField, Tooltip("待機時間(秒)")]
    private float _stanby_readyTime_sec;
    [SerializeField, Tooltip("旋回半径")]
    private float _stanby_turningRadius;
    [SerializeField, Tooltip("旋回時、1周するのにかかる時間(秒)")]
    private float _stanby_turningOneRapTime_sec;


    enum AttackType
    {
        Rush,
        Range
    }
    [field:Header("攻撃タイプ")]
    [SerializeField]
    private AttackType attackType;


    [HideInInspector] public GrenadeLauncher weapon;

    protected override void Awake()
    {
        base.Awake();

        weapon = GetComponentInChildren<GrenadeLauncher>();
    }

    protected override void Start()
    {
        stateMachine = new AiStateMachine(this);

        stateMachine.RegisterState(new Pteranodon_ChasePositionState(_standardPosition));
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new Pteranodon_RushState(weapon.transform));
        stateMachine.RegisterState(new Pteranodon_AttackState());
        stateMachine.RegisterState(new Pteranodon_BackOffState(_standardPosition.position, weapon.transform));

        AiStateId nextState = attackType == AttackType.Rush ?
                              AiStateId.Rush :       //近接攻撃
                              AiStateId.RangedAttack;//遠距離攻撃
        stateMachine.RegisterState(new Pteranodon_StanbyState(_stanby_readyTime_sec,
                                                              _stanby_turningTime_sec,
                                                              _stanby_turningRadius,
                                                              _stanby_turningOneRapTime_sec,
                                                              nextState));
        stateMachine.RegisterState(new Pteranodon_DeathState(_death_fallSpeed, weapon.transform));
        stateMachine.RegisterState(new Pteranodon_RangeAttack());
        stateMachine.ChangeState(initialState);

        turnSmoothTime = 0.5f;
        navMeshAgent.speed = config.runSpeed;

        animationEvents.AnimationEvent.AddListener(OnAnimationEvent);

        weapon.SetOwnerLife(GetComponent<LivingEntity>());
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
        if (weapon == null || weapon.dead) 
            return;
        weapon.Fire(targetEntity.transform);
    }
}