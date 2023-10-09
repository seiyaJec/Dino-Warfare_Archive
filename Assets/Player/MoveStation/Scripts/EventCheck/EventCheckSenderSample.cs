using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCheckSenderSample: EventCheckSender
{

    //変数の追加は可能ですが、基本は変更なしで大丈夫です
    //================================================
    //イベントチェック
    [SerializeField] private EventCHeck evchk_;

    //イベントチェックを渡すメソッド（削除不可）
    public override IEventCheck GetEventCheck()
    {
        return evchk_;
    }
    //================================================


    //---------------------------------------------------------
    //イベントチェック定義
    //変数、メソッドの追加など自由に行って大丈夫です
    [System.SerializableAttribute]
    public class EventCHeck : IEventCheck
    {


        //設定したMoveStationに到達すると更新開始。
        //trueを返すと終了し、プレイヤーが次へ進む
        public bool Finished()
        {




            return true;
        }
    }
    //---------------------------------------------------------
}

