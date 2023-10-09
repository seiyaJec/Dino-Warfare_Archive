using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveStation : MonoBehaviour
{

    [HideInInspector] public int _order;                     //順番
    [HideInInspector]private PlayerMove _player;             //プレイヤーオブジェクト
    [SerializeField] public MoveStationEventPackage _moveManager; //行動処理

    private void Awake()
    {
        _order = transform.GetSiblingIndex();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        _moveManager = new MoveStationEventPackage(_player, this.transform.position);

        //イベントチェック設定
        var eventCheck = gameObject.GetComponents<EventCheckSender>();
        if(eventCheck == null)
        {
            _moveManager._eventChecks.Add(new EventNoneCheck());
        }
        else
        {
            foreach(var evc in eventCheck)
            {
                _moveManager._eventChecks.Add(evc.GetEventCheck());
            }
        }

        //行動処理設定
        MoveStationEventSender[] events = gameObject.GetComponentsInChildren<MoveStationEventSender>();
        foreach (var ev in events)
        {
            _moveManager._moveStationEvents.Add(ev.getEvent());
        }
        _moveManager._stationPos = transform.position;
        _player.GetComponent<PlayerMove>().AddStation(_order, _moveManager);
    }


#if UNITY_EDITOR
    //ギズモの?示
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(this.transform.position, "moveStation", true);
        GUIStyle style = new GUIStyle();
        style.richText = true;
        Handles.Label(this.transform.position + new Vector3(0, 2, 0), transform.GetSiblingIndex().ToString());
    }
#endif
}
