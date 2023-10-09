using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStationEventPackage
{
    public PlayerMove              _player;                 //プレイヤー移動スクリプト
    public Vector3                 _stationPos;             //ポイント座標
    public List<IEventCheck>       _eventChecks;            //イベント終了判定
    public List<IMoveStationEvent> _moveStationEvents;      //行動処理

    public MoveStationEventPackage(PlayerMove player, Vector3 stationPos)
    {
        _player = player;
        _stationPos = stationPos;
        _eventChecks = new List<IEventCheck>();
        _moveStationEvents = new List<IMoveStationEvent>();
    }

    public void Begin()
    {
        foreach (var moveStationEvent in _moveStationEvents)
        {
            moveStationEvent.Begin(_player);
        }
    }
    public void UpDate()
    {
        foreach (var moveStationEvent in _moveStationEvents)
        {
            moveStationEvent.UpDate(_player);
        }
    }
    public void End()
    {
        foreach (var moveStationEvent in _moveStationEvents)
        {
            moveStationEvent.End(_player);
        }
    }


}
