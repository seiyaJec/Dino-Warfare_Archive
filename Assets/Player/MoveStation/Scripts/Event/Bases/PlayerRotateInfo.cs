using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

[System.SerializableAttribute]
public class PlayerRotateInfo
{
    [HideInInspector, Range(0f, 1f)] private float t;         //���Ԃ̌v��
    [HideInInspector] private List<IEventCheck> eventCheck = new List<IEventCheck>();   //�C�x���g�`�F�b�N

    [Header("�p�����[�^")]
    [SerializeField] private List<EventCheckSender> eventCheckSender;   //�C�x���g�`�F�b�N���擾���邽�߂̕ϐ�

    //��]
    [SerializeField] public bool freezeCursor;
    [SerializeField] public float yaw;
    [SerializeField] public float pitch;
    [SerializeField] public float roll;
    [SerializeField, Range(0.001f, 10000f)] public float duration;                    //��]�ɂ����鎞��
 
    //����
    [Header("�`�F�b�N��t����Ǝ���̕ύX�����f����܂�")]
    [SerializeField] public bool ChangeFov;                    //����̕ύX�����邩
    [SerializeField, Range(1f, 179f)] public float FOV;                         //����̕ύX

    //�C�[�W���O
    [Header("�C�[�W���O�i��������`�F�b�N����Ɣ��f����܂��j")]
    [SerializeField] public bool useEase;
    [SerializeField] public Ease easeType;

    //�C���^�[�t�F�[�X���擾����
    public void Initialize()
    {
        if (eventCheckSender.Count > 0)
        {
            foreach (var sender in eventCheckSender)
            {
                var eventcheck = sender.GetEventCheck();
                if (eventcheck != null)
                {
                    eventCheck.Add(eventcheck);
                }
                else
                {
                    Debug.Log("PlayerRotateInfo�F���g�̂Ȃ��C�x���g�`�F�b�N������܂�");
                }
            }
        }
        else
        {
            eventCheck.Add(new EventNoneCheck());
        }

        eventCheckSender.Clear();
    }

    //�ݒ肵���C�x���g�`�F�b�N���S�ďI��������
    public bool IsFinishedEvent()
    {
        if (eventCheck != null)
        {
            foreach (var evc in eventCheck)
            {
                if (evc.Finished() == false)
                {
                    return false;
                }
            }
        }

        return true;
    }




}
