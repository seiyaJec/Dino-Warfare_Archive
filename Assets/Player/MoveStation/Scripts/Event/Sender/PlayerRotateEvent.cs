using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerRotateEvent: MoveStationEventSender
{

    //変数の追加は可能ですが、基本は変更なしで大丈夫です
    //================================================
    //移動イベント
    [SerializeField] public MoveEvent _moveEvent;

    //移動イベントを渡すメソッド（削除不可）
    public override IMoveStationEvent getEvent()
    {
        return _moveEvent;
    }
    //================================================

    //---------------------------------------------------------
    //イベントチェック定義
    //変数、メソッドの追加など自由に行って大丈夫です
    [System.Serializable]
    public class MoveEvent : IMoveStationEvent
    {

        [Header("アタッチ")]
        [SerializeField] public PlayerRotateInfo[] rotater; //回転タイミングを示すオブジェクト

        //設定したMoveStationに到達すると
        //一度だけ読み込まれる処理
        public void Begin(PlayerMove player)
        {
            foreach(var rot in rotater)
            {
                rot.Initialize();
                player._scRot.rotater.Add(rot);
            }
        }

        //設定したMoveStationから、次のMoveStationに到達するまで
        //毎フレーム読み込まれ続ける処理
        //rotaterの状況を観察し、必要とあらばプレイヤーを回転
        public void UpDate(PlayerMove player)
        {
            
        }

        //設定したMoveStationの、次のMoveStationに到達したときに
        //一度だけ読み込まれる処理
        public void End(PlayerMove player)
        {

        }
    }
    //---------------------------------------------------------
}
