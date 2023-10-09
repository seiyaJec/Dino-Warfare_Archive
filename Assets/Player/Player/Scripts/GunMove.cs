using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMove : MonoBehaviour
{
    [SerializeField] private GameObject _cursor;               //カーソル
    [SerializeField] private float _moveScale;                 //移動の大きさ
    [SerializeField, Tooltip("リロードする時の回転量")]
    private Vector3 _reloadRotateAmount;
    [SerializeField, Tooltip("リロードする時の移動量")]
    private Vector3 _reloadMoveAmount;
    [SerializeField] private AudioClip _reloadSound;           //リロード時のサウンド
    [SerializeField] private ParticleSystem _reloadParticle;   //リロード時のエフェクト
    [HideInInspector] private Vector3 _initAngle;              //初期の角度
    [HideInInspector] private Vector3 _initPosition;           //初期の位置
    [HideInInspector] private Vector3 _reloadAngle;            //リロードするときの最大の傾き
    [HideInInspector] private Vector3 _reloadPosition;         //リロードするときに移動する位置
    [HideInInspector] private float _reloadCount;              //カウント
    [HideInInspector] private float _reloadTime;               //リロードにかかる時間
    [HideInInspector] private float _halfTime;                 //リロード時、中間地点で一時停止する時間
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

    //リロード
    private Vector3 ReloadMove(Vector3 cursorRotate)
    {
        Vector3 result = Vector3.zero;
        if (_reloadCount > _reloadTime)
        {
            return result;
        }
        else
        {
            //0～1～0にするためにsinを利用
            float time = Mathf.Sin((_reloadCount / _reloadTime) * 180 * Mathf.Deg2Rad);

            result.x = Mathf.Lerp(cursorRotate.x, _reloadAngle.x, time);
            result.y = Mathf.Lerp(cursorRotate.y, _reloadAngle.y, time);
            result.z = Mathf.Lerp(cursorRotate.z, _reloadAngle.z, time);

            _reloadCount += Time.deltaTime;

            return result;
        }
    }

    //リロード時の動き
    //引数には 基準となる、座標・オイラー角を入れたGunMoveInfoを入れる
    private GunMoveInfo ReloadMove(in GunMoveInfo anchorInfo)
    {
        GunMoveInfo result = anchorInfo;

        if (_reloadCount > _reloadTime)
        {
            return result;
        }

        //中間地点では現状維持で待機
        if (_reloadCount > _reloadTime * 0.5f &&
            _halfTimeCount <= _halfTime)
        {
            _halfTimeCount += Time.deltaTime;
            return new GunMoveInfo(transform.localPosition, transform.localEulerAngles);
        }

        //0～1～0にするためにsinを利用
        float time = Mathf.Sin((_reloadCount / _reloadTime) * 180 * Mathf.Deg2Rad);

        //最大回転角度を計算 実行中の数値変更を考慮してここで計算
        _reloadAngle = anchorInfo.eulerAngle + _reloadRotateAmount;
        //回転
        result.eulerAngle.x = Mathf.Lerp(anchorInfo.eulerAngle.x, _reloadAngle.x, time);
        result.eulerAngle.y = Mathf.Lerp(anchorInfo.eulerAngle.y, _reloadAngle.y, time);
        result.eulerAngle.z = Mathf.Lerp(anchorInfo.eulerAngle.z, _reloadAngle.z, time);

        //最大移動位置を計算 実行中の数値変更を考慮してここで計算
        _reloadPosition = _initPosition + _reloadMoveAmount;
        //移動
        result.position.x = Mathf.Lerp(anchorInfo.position.x, _reloadPosition.x, time);
        result.position.y = Mathf.Lerp(anchorInfo.position.y, _reloadPosition.y, time);
        result.position.z = Mathf.Lerp(anchorInfo.position.z, _reloadPosition.z, time);

        _reloadCount += Time.deltaTime;

        return result;
    }

    //リロード開始　引数にはリロードにかかる時間が渡されます
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
