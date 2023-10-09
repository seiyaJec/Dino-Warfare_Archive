using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDefeatCountCheckSender: EventCheckSender
{

    //変数の追加は可能ですが、基本は変更なしで大丈夫です
    //================================================
    //イベントチェック
    [SerializeField] private EventCHeck evchk_;

    //イベントチェックを渡すメソッド（削除不可）
    public override IEventCheck GetEventCheck()
    {
        evchk_._enemyCounter = GameObject.Find("EnemyCounter").GetComponent<EnemyCounter>();
        evchk_._initDefeats = evchk_._enemyCounter.GetEnemyRemoved();
        return evchk_;
    }
    //================================================


    //---------------------------------------------------------
    //イベントチェック定義
    //変数、メソッドの追加など自由に行って大丈夫です
    [System.SerializableAttribute]
    public class EventCHeck : IEventCheck
    {
        [SerializeField] private int _defeatsNum;
        [HideInInspector] public EnemyCounter _enemyCounter;
        [HideInInspector] public int _initDefeats;

        //設定したMoveStationに到達すると更新開始。
        //trueを返すと終了し、プレイヤーが次へ進む
        public bool Finished()
        {
            return _enemyCounter.GetEnemyRemoved() - _initDefeats >= _defeatsNum;
        }
    }
    //---------------------------------------------------------
}

