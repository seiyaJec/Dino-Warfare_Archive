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
        [SerializeField] public bool _setMove;                              //�v���C���[�̑��x��ύX���邩

        public enum MoveType { LEGACY, EASE };
        [SerializeField] public MoveType _moveType = MoveType.LEGACY;       //�ړ����@
        [SerializeField] public float _playerMoveSpeed;                     //�ړ��̑����i�����ړ��j
        [SerializeField] public PlayerMoveEase _easeMoveData;               //�ړ��̑����i�C�[�W���O�j

        [HideInInspector] private float _preDistance;       //�O�t���[���̃|�C���g�Ƃ̋���
        [HideInInspector] private float _moveCount = 0;         //�C�[�W���O�p�̃^�C���J�E���g
        [HideInInspector] private bool _enabledEase = false;       //�O�t���[���̃C�x���g�t���O



        public void Begin(PlayerMove player)
        {

            //�ړ��ݒ�
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
            //�C�x���g���Ȃ��~
            if (player._finishedEventsFlag == false)
            {
                return;
            }

            //�C�[�W���O�J�n����
            if(_enabledEase == false)
            {
                if (_moveType == MoveType.EASE)
                {
                    player.transform.DOMove(player._moveStationsData[player._nextStation]. _stationPos,
                        _easeMoveData._MoveTime).SetEase(_easeMoveData._easeType);
                }
            }
            _enabledEase = true;

            //�����ړ��̏���
            if (_moveType == MoveType.LEGACY)
            {
                player.transform.position += player._moveDirection * player._moveSpeed * Time.deltaTime;
            }

            //�I���Ȃ玟��
            if (IsMoveFinished(player))
            {
                player.SetMoveToNext();
            }
        }




        public void End(PlayerMove player) 
        {

        }



        //�����ړ�
        private bool IsMoveFinished(PlayerMove player)
        {
            //�ʏ�
            if (_moveType == MoveType.LEGACY)
            {
                //�ʉ߃|�C���g�܂ł̋������ŏ��ɂȂ����玟�̒ʉ߃|�C���g
                float distance = (player._moveStationsData[player._nextStation]._stationPos - player.transform.position).magnitude;
                if (_preDistance < distance)
                {
                    return true;
                }
                _preDistance = distance;
            }

            //�C�[�W���O
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

    //�C�[�W���O�ݒ�N���X
    [System.Serializable]
    public class PlayerMoveEase
    {
        public Ease _easeType;
        public float _MoveTime;
    }

}
