using UnityEngine;

//敵のパラメータはここで設定
[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    [Header("Commons")]
    public string temp;
    [field: SerializeField, Range(0,10f)] public float updateTargetTime { get; private set; }
    [field: SerializeField, Range(0,10f)] public float maxDistance { get; private set; }
    [field : SerializeField] public float maxSightDistance { get; private set; }
    [field : SerializeField] public float deathDuration { get; private set; }
    [field : SerializeField] public float patrolDistance { get; private set; }

    [Space (10f)]
    [Header("Attack Stats")]
    public string temp1;
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float attackRadius { get; private set; }
    [field: SerializeField] public float attackDistance { get; private set; }
    [field: SerializeField] public float attackDuration { get; private set; }
    [field: SerializeField] public float stanbyTime { get; private set; }
    [field: SerializeField] public float rangeAttackDuration { get; private set; }
    [HideInInspector] public Vector3 attackPosition; 

    [Space(10f)]
    [Header("Sight & Speed")]
    public string temp2;
    [field: SerializeField] public float fieldOfView { get; private set; }
    [field: SerializeField] public float viewDistance { get; private set; }
    [field: SerializeField] public float runSpeed { get; private set; }
    [field: SerializeField] public float rushSpeed { get; private set; }

    public DamageReceiverID[] damageReceiverIds;

    [Space(10f)]
    [Header("HitStatus")]
    public string temp3;
    [field: SerializeField] public float p1 { get; private set; }

}
