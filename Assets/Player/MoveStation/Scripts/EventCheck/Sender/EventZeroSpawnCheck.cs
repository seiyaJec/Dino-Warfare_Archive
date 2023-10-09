using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventZeroSpawnCheck: EventCheckSender
{

    //�ϐ��̒ǉ��͉\�ł����A��{�͕ύX�Ȃ��ő��v�ł�
    //================================================
    //�C�x���g�`�F�b�N
    [SerializeField] private EventCHeck eventCheck_;

    //�C�x���g�`�F�b�N��n�����\�b�h�i�폜�s�j
    public override IEventCheck GetEventCheck()
    {
        return eventCheck_;
    }
    //================================================


    //---------------------------------------------------------
    //�C�x���g�`�F�b�N��`
    //�ϐ��A���\�b�h�̒ǉ��Ȃǎ��R�ɍs���đ��v�ł�
    [System.SerializableAttribute]
    public class EventCHeck : IEventCheck
    {
        [SerializeField] private Event_SpawnEnemy spawnScript;

        //�ݒ肵��MoveStation�ɓ��B����ƍX�V�J�n�B
        //true��Ԃ��ƏI�����A�v���C���[�����֐i��
        public bool Finished()
        {
            return !spawnScript.CheckEnemiesLiving();
        }
    }
    //---------------------------------------------------------
}

