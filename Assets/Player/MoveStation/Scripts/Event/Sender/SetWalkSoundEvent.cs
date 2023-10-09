using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.EventSystems;

[System.Serializable]
public class SetWalkSoundEvent : MoveStationEventSender
{

    //�ϐ��̒ǉ��͉\�ł����A��{�͕ύX�Ȃ��ő��v�ł�
    //================================================
    //�ړ��C�x���g
    [SerializeField] public MoveEvent _moveEvent;

    //�ړ��C�x���g��n�����\�b�h�i�폜�s�j
    public override IMoveStationEvent getEvent()
    {
        return _moveEvent;
    }
    //================================================

    //---------------------------------------------------------
    //�C�x���g�`�F�b�N��`
    //�ϐ��A���\�b�h�̒ǉ��Ȃǎ��R�ɍs���đ��v�ł�
    [System.Serializable]
    public class MoveEvent : IMoveStationEvent
    {
        [SerializeField] private float _playDistance;
        [SerializeField] private string _soundsName;





        //�ݒ肵��MoveStation�ɓ��B�����
        //��x�����ǂݍ��܂�鏈��
        public void Begin(PlayerMove player)
        {
            player._walkSound.SetWalkSound(_playDistance, _soundsName);
        }

        //�ݒ肵��MoveStation����A����MoveStation�ɓ��B����܂�
        //���t���[���ǂݍ��܂ꑱ���鏈��
        public void UpDate(PlayerMove player)
        {

        }

        //�ݒ肵��MoveStation�́A����MoveStation�ɓ��B�����Ƃ���
        //��x�����ǂݍ��܂�鏈��
        public void End(PlayerMove player)
        {

        }
    }
    //---------------------------------------------------------
}
