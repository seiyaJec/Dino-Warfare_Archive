using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public float _moveSpeed;       //�ړ����x
    [SerializeField] public int _startStation;      //�J�n����ꏊ
    [HideInInspector] public int _nextStation;      //���̃|�C���g�̔ԍ�
    [HideInInspector] public int _stations = 0;     //�|�C���g����
    [HideInInspector] public Dictionary<int, MoveStationEventPackage> _moveStationsData = new Dictionary<int, MoveStationEventPackage>();       //MoveStation���Ƃ̃C�x���g�̔z��
    [HideInInspector] public bool _finishedEventsFlag { get; private set; }    //��~�C�x���g���I��������
    [HideInInspector] public MoveStationEventPackage _nextStationData //���̃|�C���g�̏��
    {
        get
        {
            return _moveStationsData[_nextStation];
        }
    }
    [HideInInspector] public MoveStationEventPackage _nowStationData  //���݂̃|�C���g�̏��
    {        
        get
        {
            return _moveStationsData[_nextStation - 1];
        }
    }
    public Vector3 _moveDirection       //�ړ��̌���
    {
        get
        {
            return (_moveStationsData[_nextStation]._stationPos - this.transform.position).normalized;
        }
    }


    //�ʃX�N���v�g
    [HideInInspector] public PlayerRotateForce _scRot;               //�v���C���[�̉�]���
    [HideInInspector] public WalkCameraShake _scWalkSk;   //�����Ă���Ƃ��̃J�����̗h��
    [HideInInspector] public PlayerTalkManager _talkManager;        //��b�f�[�^�̊Ǘ�
    [HideInInspector] public PlayerWalkSound _walkSound;            //�ړ����̑���

    //����������
    private void Awake()
    {
        _nextStation = _startStation + 1;
        _scRot = GetComponent<PlayerRotateForce>();
        _scWalkSk = GetComponentInChildren<WalkCameraShake>();
        _talkManager = GetComponent<PlayerTalkManager>();
        _walkSound = GetComponent<PlayerWalkSound>();
    }

    //�ŏ��̏���
    private void Start()
    {
        this.transform.position = _nowStationData._stationPos;
        _nowStationData.Begin();
    }

    //�X�V����
    void Update()
    {
        MoveStationEventPackage mov;
        //�Ōゾ������ړ��ݒ�����Ȃ�
        if (_moveStationsData.TryGetValue(_nextStation, out mov) == false)
        {
            return;
        }


        //��~�C�x���g�̃`�F�b�N
        _finishedEventsFlag = true;
        foreach (var evc in _nowStationData._eventChecks)
        {
            if (evc.Finished() == false)
            {
                _finishedEventsFlag = false;    
            }
        }




        //�s������
        _nowStationData.UpDate();
    }


    //����MoveStation�֐i�߂�
    public void SetMoveToNext()
    {
        _nowStationData.End();
        ++_nextStation;
        _nowStationData.Begin();
    }

    //�ʉ߃|�C���g�̓o�^
    public void AddStation(int order, MoveStationEventPackage info)
    {
        _moveStationsData.Add(order, info);
        ++_stations;
    }

}