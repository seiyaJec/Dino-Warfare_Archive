using UnityEngine;
using UnityEngine.AI;

//AI Base Class
public class AiAgent : MonoBehaviour
{
    [HideInInspector] public AiStateMachine stateMachine; //AIのステート管理クラス
    public AiAgentConfig config; //Ai status option
    public AiStateId initialState;

    //コンポネント
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public GameObject model;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Collider[] hitColliders;

    [field: SerializeField] public Transform attackRoot { get; private set; }
    [field: SerializeField] public Transform eyeTransform { get; private set; }

    //ターゲット＆レイヤマスク
    [field: SerializeField] public LayerMask whatIsTarget { get; private set; }
    public LivingEntity targetEntity { get; set; }

    //ターゲットを持っているか確認
    public bool hasTarget => targetEntity != null && !targetEntity.dead;
    public bool isAttacking { get; private set; }

    //アニメーションイベント（攻撃アニメーションから呼び出される）
    protected AnimationEvents animationEvents;

    //オブジェクト回転パラメータ
    [HideInInspector] public float turnSmoothTime;
    [HideInInspector] public float turnSmoothVelocity;

    //遠距離攻撃ポジション
    private Vector3 rangeAttackPosition = Vector3.zero;

    //軽油ルートポジション
    public Transform[] wayPoints;

    public Vector3 RangeAttackPosition
    {
        get { return rangeAttackPosition; }
    }

    //コンポネント取得
    protected virtual void Awake()
    {
        //Component Init
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        hitColliders = GetComponentsInChildren<Collider>();
        animationEvents = GetComponentInChildren<AnimationEvents>();
        model = transform.Find("Model").gameObject;
    }

    //初期化
    protected virtual void Start()
    {
        turnSmoothTime = 0.5f;
        navMeshAgent.speed = config.runSpeed;

        animationEvents.AnimationEvent.AddListener(OnAnimationEvent);
    }

    //ステートマシンの行動更新
    protected virtual void Update()
    {
        stateMachine.Update();
    }

    protected virtual void OnAnimationEvent(string evenName)
    {
        switch (evenName)
        {
            case "EnableAttack":
                EnableAttack();
                break;
            case "DisableAttack":
                DisableAttack();
                break;
            default:
                break;
        }
    }

    //ダメージを与えられる状態
    protected void EnableAttack()
    {
        isAttacking = true;
    }

    //ダメージを与えられない状態
    protected void DisableAttack()
    {
        isAttacking = false;
    }

    //ナビメッシュの回転処理を消して代わりに直接回転させる
    public void Rotate()
    {
        Vector2 forward = new Vector2(transform.position.z, transform.position.x);
        Vector2 steeringTarget = new Vector2(navMeshAgent.steeringTarget.z, navMeshAgent.steeringTarget.x);

        //方向を求めてAtan2を使って角度を求める
        Vector2 dir = steeringTarget - forward;
        float targetAngleY = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        targetAngleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, //ターゲットがいる方向に回転する
            ref turnSmoothVelocity, turnSmoothTime);
        //角度適用
        transform.eulerAngles = Vector3.up * targetAngleY;
    }

    //ターゲットにむいて回転する
    public void RotateToTarget()
    {
        var lookRotation = Quaternion.LookRotation(targetEntity.transform.position - transform.position, Vector3.up);
        var targetAngleY = lookRotation.eulerAngles.y; //

        targetAngleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY,
            ref turnSmoothVelocity, turnSmoothTime);

        transform.eulerAngles = Vector3.up * targetAngleY;
    } 

    //対象がオブジェクトが視野(eyeTransform)に入っているか確認
    public bool IsTargetOnSight(Transform target)
    {
        RaycastHit hit;

        var direction = target.position - eyeTransform.position;
        direction.y = eyeTransform.forward.y;

        if (Vector3.Angle(direction, eyeTransform.forward) > config.fieldOfView * 0.5f)
        {
            return false;
        }

        if (Physics.Raycast(eyeTransform.position, direction, out hit, config.viewDistance, whatIsTarget))
        {
            if (hit.transform == target)
                return true;
        }
        return false;
    }

    //NavMeshの目的地を更新
    public void UpdatePath()
    {
        navMeshAgent.SetDestination(targetEntity.transform.position);
    }

    //攻撃時にターゲットが攻撃範囲内に入っているか確認
    public void CheckCollider()
    {
        var colliders = Physics.OverlapSphere(eyeTransform.position, config.viewDistance, whatIsTarget);
        foreach (var collider in colliders)
        {
            //オブジェクトが視野範囲内にない
            if (!IsTargetOnSight(collider.transform))
                continue;

            var livingEntity = collider.GetComponent<LivingEntity>();

            //ターゲットを設定
            if (livingEntity != null && !livingEntity.dead)
            {
                targetEntity = livingEntity;
                break;
            }
        }
    }

    //Hitモーションに変わる
    public virtual void GetHit()
    {

    }

    //Aiagentクラス内での死亡処理
    public virtual void Die()
    {

    }

    public void SetTarget(LivingEntity target)
    {
        targetEntity = target;
    }

    public void SetRangeAttackPosition(Vector3 pos)
    {
        rangeAttackPosition = pos;
    }
}
