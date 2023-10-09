using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDefeatCountCheckSender: EventCheckSender
{

    //�ϐ��̒ǉ��͉\�ł����A��{�͕ύX�Ȃ��ő��v�ł�
    //================================================
    //�C�x���g�`�F�b�N
    [SerializeField] private EventCHeck evchk_;

    //�C�x���g�`�F�b�N��n�����\�b�h�i�폜�s�j
    public override IEventCheck GetEventCheck()
    {
        evchk_._enemyCounter = GameObject.Find("EnemyCounter").GetComponent<EnemyCounter>();
        evchk_._initDefeats = evchk_._enemyCounter.GetEnemyRemoved();
        return evchk_;
    }
    //================================================


    //---------------------------------------------------------
    //�C�x���g�`�F�b�N��`
    //�ϐ��A���\�b�h�̒ǉ��Ȃǎ��R�ɍs���đ��v�ł�
    [System.SerializableAttribute]
    public class EventCHeck : IEventCheck
    {
        [SerializeField] private int _defeatsNum;
        [HideInInspector] public EnemyCounter _enemyCounter;
        [HideInInspector] public int _initDefeats;

        //�ݒ肵��MoveStation�ɓ��B����ƍX�V�J�n�B
        //true��Ԃ��ƏI�����A�v���C���[�����֐i��
        public bool Finished()
        {
            return _enemyCounter.GetEnemyRemoved() - _initDefeats >= _defeatsNum;
        }
    }
    //---------------------------------------------------------
}

