using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    //カメラ
    [SerializeField]
    [HideInInspector] private Camera _camera;
    //カ??ル
    [SerializeField]  private Image _imgCursor;
    //入力
    [HideInInspector] private InputAction _shot;
    private JoyconCheckInput _joycon;
    //的
    [HideInInspector] private GameObject _target;
    //射撃間隔
    [SerializeField]  private float _shotDistance;
    //一回に撃てる弾の量
    [SerializeField] public uint _magazineMax;
    //残弾数
    [HideInInspector] public uint _magazineNow;
    //リロ?ドで回復する量
    [SerializeField] public uint _reloadSize;
    //射撃間隔計測用
    [HideInInspector] private float _shotTimeCount;
    //銃声
    [SerializeField] private AudioClip _shotSE;
    //空撃ちの銃声
    [SerializeField] private AudioClip _emptyShotSE;
    //ヒット音
    [SerializeField] private AudioClip _hitSE;

    //1フレ??前の通過?イントとの距離
    float _preDistance;



    [Serializable]
    public class StickValue
    {
        public enum StickAngle { X = 0, Y }
        public StickAngle _stickAngle;
        [Range(-1f,1f)] public float _stickValue;
    }
    [Header("リロ?ド")]
    [SerializeField] private Joycon.Button _reloadButton;
    [SerializeField] private float _reloadCoolTime;
    [HideInInspector] private float _reloadCoolTimeCount;
    [SerializeField, Tooltip("リロード動作の中間地点で待機する時間")]
    private float _reloadHalfTime;
    [SerializeField] private GunMove _gun;



    [Header("振動")]
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
    /// 初期化処理
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
    /// 更新処理
    /// </summary>
    void Update()
    {
        //カ??ルの色変更処理
        ChangeCursorColor();

        //弾を打つ処理
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


        //リロ?ドする量の?示
        int reloadNum = 0;

        //リロ?ド処理
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

        //連射待?カウント
        if (_shotTimeCount < _shotDistance)
        {
            _shotTimeCount += Time.deltaTime;
        }
    }


    /// <summary>
    /// 弾を打つ
    /// </summary>
    private void Shot()
    {
        Vector3 direction = _imgCursor.transform.position - _camera.transform.position;
        Ray ray = new Ray();
        ray.direction = direction.normalized;
        ray.origin = transform.position;

        //Rayを画面に?示
        Debug.DrawRay(transform.position, ray.direction * maxDistance, Color.green, 1, true);

        //エフェクト再生
        //EffectManager.Instance.PlayEffect(Vector3.zero , EffectManager.EffectType.Shot);

        //最も近い標的に当たり判定
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
        //射撃時間を戻す
        _shotTimeCount -= _shotDistance;
        //?ガジンの弾を減らす
        --_magazineNow;

        _nonHitRumb_L.SetToJoycon(_joycon.m_joyconL);
        _nonHitRumb_R.SetToJoycon(_joycon.m_joyconR);
        SoundManager.Instance.Play(_shotSE, SoundManager.Sound.EFFECT);
    }

    //リロ?ドの確認
    private void CheckReload()
    {
        if(_reloadCoolTimeCount > 0)
        {
            return;
        }


        //リロ?ド
        if (_joycon.m_pressedButtonL == _reloadButton || Input.GetKeyDown(KeyCode.H))
        {
            //中間地点での待機時間も考慮
            _reloadCoolTimeCount = _reloadCoolTime + _reloadHalfTime;
            _gun.SetReloadMove(_reloadCoolTime, _reloadHalfTime);
        }
    }


    //カ??ルの色変更処理
    private void ChangeCursorColor()
    {
        Vector3 direction = _imgCursor.transform.position - _camera.transform.position;
        Ray ray = new Ray();
        ray.direction = direction.normalized;
        ray.origin = transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, targetLayer))
        {
            //EnemyPool内のコライ?であれば敵とみなす
            if (hit.transform.root.CompareTag("EnemyPool"))
            {
                _imgCursor.color = Color.red;
                return;
            }
        }

        //色変更なし
        _imgCursor.color = Color.white;
    }

    //復活時の処理
    public void SetRevival()
    {
        //マガジン回復
        _magazineNow = _magazineMax;
    }
}