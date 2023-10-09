using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMove : MonoBehaviour
{
    [SerializeField] private GameObject _cursor;               //�J�[�\��
    [SerializeField] private float _moveScale;                 //�ړ��̑傫��
    [SerializeField, Tooltip("�����[�h���鎞�̉�]��")]
    private Vector3 _reloadRotateAmount;
    [SerializeField, Tooltip("�����[�h���鎞�̈ړ���")]
    private Vector3 _reloadMoveAmount;
    [SerializeField] private AudioClip _reloadSound;           //�����[�h���̃T�E���h
    [SerializeField] private ParticleSystem _reloadParticle;   //�����[�h���̃G�t�F�N�g
    [HideInInspector] private Vector3 _initAngle;              //�����̊p�x
    [HideInInspector] private Vector3 _initPosition;           //�����̈ʒu
    [HideInInspector] private Vector3 _reloadAngle;            //�����[�h����Ƃ��̍ő�̌X��
    [HideInInspector] private Vector3 _reloadPosition;         //�����[�h����Ƃ��Ɉړ�����ʒu
    [HideInInspector] private float _reloadCount;              //�J�E���g
    [HideInInspector] private float _reloadTime;               //�����[�h�ɂ����鎞��
    [HideInInspector] private float _halfTime;                 //�����[�h���A���Ԓn�_�ňꎞ��~���鎞��
    [HideInInspector] private float _halfTimeCount;

    struct GunMoveInfo
    {
        public Vector3 position;
        public Vector3 eulerAngle;

        public GunMoveInfo(in Vector3 position, in Vector3 eulerAngle)
        {
            this.position = position;
            this.eulerAngle = eulerAngle;
        }

        public void ApplyTo(Transform transform)
        {
            transform.localPosition = this.position;
            transform.localEulerAngles = this.eulerAngle;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _initAngle = transform.localEulerAngles;
        _initPosition = transform.localPosition;
        _reloadCount = 999;
        _reloadTime = 0;

        _halfTimeCount = 0.0f;
    }

    // Update is called once per frame    
    void Update()
    {
        Vector3 cursorRotate = _initAngle + (new Vector3(-_cursor.transform.localPosition.y, _cursor.transform.localPosition.x) * _moveScale * 0.001f);
        GunMoveInfo moveInfo = ReloadMove(new GunMoveInfo(_initPosition, cursorRotate));
        moveInfo.ApplyTo(transform);
    }

    //�����[�h
    private Vector3 ReloadMove(Vector3 cursorRotate)
    {
        Vector3 result = Vector3.zero;
        if (_reloadCount > _reloadTime)
        {
            return result;
        }
        else
        {
            //0�`1�`0�ɂ��邽�߂�sin�𗘗p
            float time = Mathf.Sin((_reloadCount / _reloadTime) * 180 * Mathf.Deg2Rad);

            result.x = Mathf.Lerp(cursorRotate.x, _reloadAngle.x, time);
            result.y = Mathf.Lerp(cursorRotate.y, _reloadAngle.y, time);
            result.z = Mathf.Lerp(cursorRotate.z, _reloadAngle.z, time);

            _reloadCount += Time.deltaTime;

            return result;
        }
    }

    //�����[�h���̓���
    //�����ɂ� ��ƂȂ�A���W�E�I�C���[�p����ꂽGunMoveInfo������
    private GunMoveInfo ReloadMove(in GunMoveInfo anchorInfo)
    {
        GunMoveInfo result = anchorInfo;

        if (_reloadCount > _reloadTime)
        {
            return result;
        }

        //���Ԓn�_�ł͌���ێ��őҋ@
        if (_reloadCount > _reloadTime * 0.5f &&
            _halfTimeCount <= _halfTime)
        {
            _halfTimeCount += Time.deltaTime;
            return new GunMoveInfo(transform.localPosition, transform.localEulerAngles);
        }

        //0�`1�`0�ɂ��邽�߂�sin�𗘗p
        float time = Mathf.Sin((_reloadCount / _reloadTime) * 180 * Mathf.Deg2Rad);

        //�ő��]�p�x���v�Z ���s���̐��l�ύX���l�����Ă����Ōv�Z
        _reloadAngle = anchorInfo.eulerAngle + _reloadRotateAmount;
        //��]
        result.eulerAngle.x = Mathf.Lerp(anchorInfo.eulerAngle.x, _reloadAngle.x, time);
        result.eulerAngle.y = Mathf.Lerp(anchorInfo.eulerAngle.y, _reloadAngle.y, time);
        result.eulerAngle.z = Mathf.Lerp(anchorInfo.eulerAngle.z, _reloadAngle.z, time);

        //�ő�ړ��ʒu���v�Z ���s���̐��l�ύX���l�����Ă����Ōv�Z
        _reloadPosition = _initPosition + _reloadMoveAmount;
        //�ړ�
        result.position.x = Mathf.Lerp(anchorInfo.position.x, _reloadPosition.x, time);
        result.position.y = Mathf.Lerp(anchorInfo.position.y, _reloadPosition.y, time);
        result.position.z = Mathf.Lerp(anchorInfo.position.z, _reloadPosition.z, time);

        _reloadCount += Time.deltaTime;

        return result;
    }

    //�����[�h�J�n�@�����ɂ̓����[�h�ɂ����鎞�Ԃ��n����܂�
    public void SetReloadMove(float reloadTime, float halfTime)
    {
        _reloadTime = reloadTime;
        _reloadCount = 0;
        _halfTime = halfTime;
        _halfTimeCount = 0;

        SoundManager.Instance.Play(_reloadSound, SoundManager.Sound.EFFECT);

        SetReloadEffectParam(_reloadParticle.main);
        _reloadParticle.Play();
    }

    private void SetReloadEffectParam(ParticleSystem.MainModule mainModule)
    {
        mainModule.duration = _reloadTime + _halfTime;
    }

    public bool IsReloading()
    {
        return _reloadCount <= _reloadTime;
    }
}
