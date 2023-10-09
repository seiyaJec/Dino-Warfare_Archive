using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventFinishTalkCheckSender: EventCheckSender
{

    //�ϐ��̒ǉ��͉\�ł����A��{�͕ύX�Ȃ��ő��v�ł�
    //================================================
    //�C�x���g�`�F�b�N
    [SerializeField] private EventCheck evchk_;

    //�C�x���g�`�F�b�N��n�����\�b�h�i�폜�s�j
    public override IEventCheck GetEventCheck()
    {
        return evchk_;
    }
    //================================================


    //---------------------------------------------------------
    //�C�x���g�`�F�b�N��`
    //�ϐ��A���\�b�h�̒ǉ��Ȃǎ��R�ɍs���đ��v�ł�
    [System.SerializableAttribute]
    public class EventCheck : IEventCheck
    {
        private enum TalkAppearState
        {
            NotSeen,        //�܂��o�����Ă��Ȃ�
            Appear,         //�o������
            Disappear,      //���ł���
        }

        [SerializeField] private string _targetTalk;
        [SerializeField] private TalkAppearState _finishCondition = TalkAppearState.Disappear;
        [HideInInspector] private TalkAppearState _currentState = TalkAppearState.NotSeen;
        

        //�ݒ肵��MoveStation�ɓ��B����ƍX�V�J�n�B
        //true��Ԃ��ƏI�����A�v���C���[�����֐i��
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

