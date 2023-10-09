using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.EventSystems;

[System.Serializable]
public class ChangeWalkCameraShakeEvent : MoveStationEventSender
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

        [SerializeField] public float _shakeMin;
        [SerializeField] public float _shakeMax;
        [SerializeField] public float _shakeSpeed;      
        [SerializeField] public bool _shaking;          //true�Ȃ�J�����̗h�ꂪ�L��


        //�ݒ肵��MoveStation�ɓ��B�����
        //��x�����ǂݍ��܂�鏈��
        public void Begin(PlayerMove player)
        {
            player._scWalkSk._shakeMin = _shakeMin;
            player._scWalkSk._shakeMax = _shakeMax;
            player._scWalkSk._shakeSpeed = _shakeSpeed;
            player._scWalkSk._shaking = _shaking;
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
