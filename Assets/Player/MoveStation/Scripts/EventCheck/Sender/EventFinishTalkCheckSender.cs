using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventFinishTalkCheckSender: EventCheckSender
{

    //変数の追加は可能ですが、基本は変更なしで大丈夫です
    //================================================
    //イベントチェック
    [SerializeField] private EventCheck evchk_;

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
    public class EventCheck : IEventCheck
    {
        private enum TalkAppearState
        {
            NotSeen,        //まだ出現していない
            Appear,         //出現した
            Disappear,      //消滅した
        }

        [SerializeField] private string _targetTalk;
        [SerializeField] private TalkAppearState _finishCondition = TalkAppearState.Disappear;
        [HideInInspector] private TalkAppearState _currentState = TalkAppearState.NotSeen;
        

        //設定したMoveStationに到達すると更新開始。
        //trueを返すと終了し、プレイヤーが次へ進む
        public bool Finished()
        {
            switch (_currentState)
            {
                case TalkAppearState.NotSeen:
                    if(UIManager.Instance.talkPanel.GetTalk() == _targetTalk)
                    {
                        _currentState = TalkAppearState.Appear;
                    }
                    break;
                case TalkAppearState.Appear:
                    if (UIManager.Instance.talkPanel.GetTalk() != _targetTalk)
                    {
                        _currentState = TalkAppearState.Disappear;
                    }
                    break;
                case TalkAppearState.Disappear:
                    break;
            }


            return _currentState >= _finishCondition;
        }
    }
    //---------------------------------------------------------
}

