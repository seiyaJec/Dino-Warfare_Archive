using UnityEngine;

public class Event_SetEnemyTarget : MonoBehaviour, IEvent
{
    private EventPlayer eventPlayer;

    [SerializeField] private AiAgent[] enemy;

    //CI_0329
    public AiAgent[] GetEnemy 
    { 
        get { return enemy; } 
    }


    private void Awake()
    {
        eventPlayer = GetComponent<EventPlayer>();
        AddAction(eventPlayer);
    }

    public void AddAction(EventPlayer eventPlayer)
    {
        eventPlayer.action += Action;
    }

    public void Action()
    {
        var target = GameObject.FindWithTag("Player").GetComponent<LivingEntity>();

        if (!target) { return; }

        foreach (var e in enemy)
        {
            e.SetTarget(target);
        }
    }
}
