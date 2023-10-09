using UnityEngine;

public class Event_SpawnEnemy : MonoBehaviour, IEvent
{
    private EventPlayer eventPlayer;

    [SerializeField] private Transform[] spawnTransform;
    [SerializeField] private GameObject spawnTarget;

    private Transform enemyPool;

    //スポーンした敵が生きているかチェックするためのメンバ
    [HideInInspector] public bool spawned = false;
    [HideInInspector] public LivingEntity[] livingEntities;

    private void Awake()
    {
        livingEntities = new LivingEntity[spawnTransform.Length];
        eventPlayer = GetComponent<EventPlayer>();
        enemyPool = GameObject.FindWithTag("EnemyPool").transform;
        AddAction(eventPlayer);
    }

    public void AddAction(EventPlayer eventPlayer)
    {
        eventPlayer.action += Action;
    }

    public void Action()
    {
        spawned = true;
        var target = GameObject.FindWithTag("Player").GetComponent<LivingEntity>();
        
        if (!target) { return; }

        int i = 0;
        foreach (var st in spawnTransform)
        {
            var obj = Instantiate(spawnTarget, st.position, Quaternion.identity, enemyPool);
            var agent = obj.GetComponent<AiAgent>();
            if (!agent) { return; }
            agent.SetTarget(target);
            livingEntities[i] = obj.GetComponent<LivingEntity>();
            ++i;
        }
    }


    //ーーーーーーーーーーーーーーーーーーーーーー
    //CI_0329
    //生きている敵がいればtrue
    public bool CheckEnemiesLiving()
    {
        if(spawned == false) 
        { 
            return true; 
        }


        foreach(var entity in livingEntities)
        {
            if(entity.dead == false)
            {
                return true;
            }
        }



        return false;
    }


    //ーーーーーーーーーーーーーーーーーーーーーー
}
