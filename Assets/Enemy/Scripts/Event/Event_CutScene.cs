using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_CutScene : MonoBehaviour, IEvent
{
    private EventPlayer eventPlayer;

    [SerializeField] private CutSceneManager.SceneType type;

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
        CutSceneManager.Instance.PlayCutScene(type);
    }

}
