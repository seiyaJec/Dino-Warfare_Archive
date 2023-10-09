using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    //�J����
    [SerializeField]
    [HideInInspector] private Camera _camera;
    //�J??��
    [SerializeField]  private Image _imgCursor;
    //����
    [HideInInspector] private InputAction _shot;
    private JoyconCheckInput _joycon;
    //�I
    [HideInInspector] private GameObject _target;
    //�ˌ��Ԋu
    [SerializeField]  private float _shotDistance;
    //���Ɍ��Ă�e�̗�
    [SerializeField] public uint _magazineMax;
    //�c�e��
    [HideInInspector] public uint _magazineNow;
    //����?�h�ŉ񕜂����
    [SerializeField] public uint _reloadSize;
    //�ˌ��Ԋu�v���p
    [HideInInspector] private float _shotTimeCount;
    //�e��
    [SerializeField] private AudioClip _shotSE;
    //�󌂂��̏e��
    [SerializeField] private AudioClip _emptyShotSE;
    //�q�b�g��
    [SerializeField] private AudioClip _hitSE;

    //1�t��??�O�̒ʉ�?�C���g�Ƃ̋���
    float _preDistance;



    [Serializable]
    public class StickValue
    {
        public enum StickAngle { X = 0, Y }
        public StickAngle _stickAngle;
        [Range(-1f,1f)] public float _stickValue;
    }
    [Header("����?�h")]
    [SerializeField] private Joycon.Button _reloadButton;
    [SerializeField] private float _reloadCoolTime;
    [HideInInspector] private float _reloadCoolTimeCount;
    [SerializeField, Tooltip("�����[�h����̒��Ԓn�_�őҋ@���鎞��")]
    private float _reloadHalfTime;
    [SerializeField] private GunMove _gun;



    [Header("�U��")]
    [SerializeField] private JoyconRumble _nonHitRumb_L;
    [SerializeField] private JoyconRumble _nonHitRumb_R;
    [SerializeField] private JoyconRumble _hitRumb_L;
    [SerializeField] private JoyconRumble _hitRumb_R;

    [Space]
    //---------------------------------------
    //
    [SerializeField]
    private LayerMask targetLayer;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float maxDistance;
    //---------------------------------------

    private void Awake()
    {
        _camera = Camera.main;
        _joycon = JoyconCheckInput.Instance;
    }

    /// <summary>
    /// ����������
    /// </summary>
    void Start()
    {
        _shotTimeCount = 0;
        _reloadCoolTimeCount = 0;
        var input = GetComponent<PlayerInput>();
        _shot = input.actions["Shot"];
        _magazineNow = _magazineMax;
        UIManager.Instance.InitializeShot(_magazineMax);
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        //�J??���̐F�ύX����
        ChangeCursorColor();

        //�e��ł���
        if (_shot.IsPressed() || _joycon.m_pressedButtonR == Joycon.Button.SHOULDER_2)
        {
            if (_reloadCoolTimeCount <= 0)
            {
                if (_shotTimeCount >= _shotDistance)
                {
                    if (_magazineNow > 0)
                    {
                        Shot();
                    }
                    else
                    {
                        SoundManager.Instance.Play(_emptyShotSE);
                    }
                }
            }
        }


        //����?�h����ʂ�?��
        int reloadNum = 0;

        //����?�h����
        if (_reloadCoolTimeCount > 0)
        {
            _reloadCoolTimeCount -= Time.deltaTime;
            reloadNum = (int)_reloadSize;

            if (_reloadCoolTimeCount <= 0)
            {
                _magazineNow += _reloadSize;
                if (_magazineNow > _magazineMax)
                {
                    _magazineNow = _magazineMax;
                }
                reloadNum = 0;
            }
        }

        CheckReload();

        UIManager.Instance.UpDateShot(_magazineNow, reloadNum);

        //�A�ˑ�?�J�E���g
        if (_shotTimeCount < _shotDistance)
        {
            _shotTimeCount += Time.deltaTime;
        }
    }


    /// <summary>
    /// �e��ł�
    /// </summary>
    private void Shot()
    {
        Vector3 direction = _imgCursor.transform.position - _camera.transform.position;
        Ray ray = new Ray();
        ray.direction = direction.normalized;
        ray.origin = transform.position;

        //Ray����ʂ�?��
        Debug.DrawRay(transform.position, ray.direction * maxDistance, Color.green, 1, true);

        //�G�t�F�N�g�Đ�
        //EffectManager.Instance.PlayEffect(Vector3.zero , EffectManager.EffectType.Shot);

        //�ł��߂��W�I�ɓ����蔻��
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance, targetLayer))
        {
            var target = hit.collider.GetComponent<DamageReceiver>();

            if (target && target.IsDead() == false)
            {
                var message = new DamageMessage();
                message.Amount = damage;
                message.damager = gameObject;
                message.hitPoint = hit.point;
                message.hitNormal = hit.normal;

                target.SendDamage(message);

                _hitRumb_L.SetToJoycon(_joycon.m_joyconL);
                _hitRumb_R.SetToJoycon(_joycon.m_joyconR);

                SoundManager.Instance.Play(_hitSE, SoundManager.Sound.EFFECT);
            }
            else
            {
                EffectManager.Instance.PlayEffect(hit.point, hit.normal);
            }
        }
        //�ˌ����Ԃ�߂�
        _shotTimeCount -= _shotDistance;
        //?�K�W���̒e�����炷
        --_magazineNow;

        _nonHitRumb_L.SetToJoycon(_joycon.m_joyconL);
        _nonHitRumb_R.SetToJoycon(_joycon.m_joyconR);
        SoundManager.Instance.Play(_shotSE, SoundManager.Sound.EFFECT);
    }

    //����?�h�̊m�F
    private void CheckReload()
    {
        if(_reloadCoolTimeCount > 0)
        {
            return;
        }


        //����?�h
        if (_joycon.m_pressedButtonL == _reloadButton || Input.GetKeyDown(KeyCode.H))
        {
            //���Ԓn�_�ł̑ҋ@���Ԃ��l��
            _reloadCoolTimeCount = _reloadCoolTime + _reloadHalfTime;
            _gun.SetReloadMove(_reloadCoolTime, _reloadHalfTime);
        }
    }


    //�J??���̐F�ύX����
    private void ChangeCursorColor()
    {
        Vector3 direction = _imgCursor.transform.position - _camera.transform.position;
        Ray ray = new Ray();
        ray.direction = direction.normalized;
        ray.origin = transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, targetLayer))
        {
            //EnemyPool���̃R���C?�ł���ΓG�Ƃ݂Ȃ�
            if (hit.transform.root.CompareTag("EnemyPool"))
            {
                _imgCursor.color = Color.red;
                return;
            }
        }

        //�F�ύX�Ȃ�
        _imgCursor.color = Color.white;
    }

    //�������̏���
    public void SetRevival()
    {
        //�}�K�W����
        _magazineNow = _magazineMax;
    }
}