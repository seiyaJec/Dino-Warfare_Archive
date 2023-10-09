using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerMoveEvent : MoveStationEventSender
{
    [SerializeField] public MoveEvent _moveEvent;

   public override IMoveStationEvent getEvent()
    {
        return _moveEvent;
    }

    [System.Serializable]
    public class MoveEvent : IMoveStationEvent
    {
        [SerializeField] public bool _setMove;                              //プレイヤーの速度を変更するか

        public enum MoveType { LEGACY, EASE };
        [SerializeField] public MoveType _moveType = MoveType.LEGACY;       //移動方法
        [SerializeField] public float _playerMoveSpeed;                     //移動の速さ（直線移動）
        [SerializeField] public PlayerMoveEase _easeMoveData;               //移動の速さ（イージング）

        [HideInInspector] private float _preDistance;       //前フレームのポイントとの距離
        [HideInInspector] private float _moveCount = 0;         //イージング用のタイムカウント
        [HideInInspector] private bool _enabledEase = false;       //前フレームのイベントフラグ



        public void Begin(PlayerMove player)
        {

            //移動設定
            if (_setMove == true)
            {
                if (_moveType == MoveType.LEGACY)
                {
                    player._moveSpeed = _playerMoveSpeed;
                }
            }
            else
            {
                _moveType = MoveType.LEGACY;
            }
            _preDistance = Mathf.Infinity;
        }




        public void UpDate(PlayerMove player)
        {
            //イベント中なら停止
            if (player._finishedEventsFlag == false)
            {
                return;
            }

            //イージング開始処理
            if(_enabledEase == false)
            {
                if (_moveType == MoveType.EASE)
                {
                    player.transform.DOMove(player._moveStationsData[player._nextStation]. _stationPos,
                        _easeMoveData._MoveTime).SetEase(_easeMoveData._easeType);
                }
            }
            _enabledEase = true;

            //直線移動の処理
            if (_moveType == MoveType.LEGACY)
            {
                player.transform.position += player._moveDirection * player._moveSpeed * Time.deltaTime;
            }

            //終了なら次へ
            if (IsMoveFinished(player))
            {
                player.SetMoveToNext();
            }
        }




        public void End(PlayerMove player) 
        {

        }



        //直線移動
        private bool IsMoveFinished(PlayerMove player)
        {
            //通常
            if (_moveType == MoveType.LEGACY)
            {
                //通過ポイントまでの距離が最小になったら次の通過ポイント
                float distance = (player._moveStationsData[player._nextStation]._stationPos - player.transform.position).magnitude;
                if (_preDistance < distance)
                {
                    return true;
                }
                _preDistance = distance;
            }

            //イージング
            else if (_moveType == MoveType.EASE)
            {
                if (_moveCount >= _easeMoveData._MoveTime)
                {
                    return true;
                }
                _moveCount += Time.deltaTime;
            }

            return false;
        }
    }

    //イージング設定クラス
    [System.Serializable]
    public class PlayerMoveEase
    {
        public Ease _easeType;
        public float _MoveTime;
    }

}
