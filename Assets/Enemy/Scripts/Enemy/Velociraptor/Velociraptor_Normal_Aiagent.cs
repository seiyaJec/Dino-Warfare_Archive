using UnityEngine;

public class Velociraptor_Normal_Aiagent : AiAgent
{
    [field: SerializeField] public bool patrol;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new Velociraptor_IdleState());
        stateMachine.RegisterState(new Velociraptor_Patrol());
        stateMachine.RegisterState(new Velociraptor_Detour_State());
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new Velociraptor_AttackState());
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
}
