using UnityEngine;

public class Pteranodon_Aiagent : AiAgent
{
    [SerializeField]
    private Transform _standardPosition;
    

    [field: Header("���S")]
    [SerializeField, Tooltip("���S���̗������x")]
    private float _death_fallSpeed;

    
    [field:Header("����")]
    [SerializeField, Tooltip("���񎞊�(�b)")]
    private float _stanby_turningTime_sec;
    [SerializeField, Tooltip("�ҋ@����(�b)")]
    private float _stanby_readyTime_sec;
    [SerializeField, Tooltip("���񔼌a")]
    private float _stanby_turningRadius;
    [SerializeField, Tooltip("���񎞁A1������̂ɂ����鎞��(�b)")]
    private float _stanby_turningOneRapTime_sec;


    enum AttackType
    {
        Rush,
        Range
    }
    [field:Header("�U���^�C�v")]
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
                              AiStateId.Rush :       //�ߐڍU��
                              AiStateId.RangedAttack;//�������U��
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