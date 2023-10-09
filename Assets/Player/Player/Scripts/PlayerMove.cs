using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public float _moveSpeed;       //移動速度
    [SerializeField] public int _startStation;      //開始する場所
    [HideInInspector] public int _nextStation;      //次のポイントの番号
    [HideInInspector] public int _stations = 0;     //ポイント総数
    [HideInInspector] public Dictionary<int, MoveStationEventPackage> _moveStationsData = new Dictionary<int, MoveStationEventPackage>();       //MoveStationごとのイベントの配列
    [HideInInspector] public bool _finishedEventsFlag { get; private set; }    //停止イベントが終了したか
    [HideInInspector] public MoveStationEventPackage _nextStationData //次のポイントの情報
    {
        get
        {
            return _moveStationsData[_nextStation];
        }
    }
    [HideInInspector] public MoveStationEventPackage _nowStationData  //現在のポイントの情報
    {        
        get
        {
            return _moveStationsData[_nextStation - 1];
        }
    }
    public Vector3 _moveDirection       //移動の向き
    {
        get
        {
            return (_moveStationsData[_nextStation]._stationPos - this.transform.position).normalized;
        }
    }


    //別スクリプト
    [HideInInspector] public PlayerRotateForce _scRot;               //プレイヤーの回転情報
    [HideInInspector] public WalkCameraShake _scWalkSk;   //歩いているときのカメラの揺れ
    [HideInInspector] public PlayerTalkManager _talkManager;        //会話データの管理
    [HideInInspector] public PlayerWalkSound _walkSound;            //移動時の足音

    //初期化処理
    private void Awake()
    {
        _nextStation = _startStation + 1;
        _scRot = GetComponent<PlayerRotateForce>();
        _scWalkSk = GetComponentInChildren<WalkCameraShake>();
        _talkManager = GetComponent<PlayerTalkManager>();
        _walkSound = GetComponent<PlayerWalkSound>();
    }

    //最初の処理
    private void Start()
    {
        this.transform.position = _nowStationData._stationPos;
        _nowStationData.Begin();
    }

    //更新処理
    void Update()
    {
        MoveStationEventPackage mov;
        //最後だったら移動設定をしない
        if (_moveStationsData.TryGetValue(_nextStation, out mov) == false)
        {
            return;
        }


        //停止イベントのチェック
        _finishedEventsFlag = true;
        foreach (var evc in _nowStationData._eventChecks)
        {
            if (evc.Finished() == false)
            {
                _finishedEventsFlag = false;    
            }
        }




        //行動処理
        _nowStationData.UpDate();
    }


    //次のMoveStationへ進める
    public void SetMoveToNext()
    {
        _nowStationData.End();
        ++_nextStation;
        _nowStationData.Begin();
    }

    //通過ポイントの登録
    public void AddStation(int order, MoveStationEventPackage info)
    {
        _moveStationsData.Add(order, info);
        ++_stations;
    }

}