using UnityEngine;

//EffectManager使いたかったけど、ScreenToWorldPointと相性が悪そうなため、いったんべた書き
public class ShotEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particle;

    [SerializeField]
    float _particleDuration;

    Camera _mainCamera;
    bool _isPlayingParticle;
    float _particleTime;

    private void Awake()
    {
        _mainCamera = Camera.main;
        Debug.Log(_particle.name);
        _isPlayingParticle = false;
        _particleTime = 0.0f;
    }

    private void Update()
    {
        //再生開始
        if (InputReciever.Instance.OneShot.WasPressedThisFrame())
        {
            EffectPosSet();
            //エフェクト再生
            _particle.Play();
            _isPlayingParticle = true;
        }

        //再生中
        if (_isPlayingParticle)
        {
            //再生時間カウント
            _particleTime += Time.deltaTime;
            EffectPosSet();
        }

        //持続時間過ぎたら再生停止
        if (_particleTime > _particleDuration)
        {
            _particle.Stop();
            _isPlayingParticle = false;
            _particleTime = 0.0f;
        }
    }

    private void EffectPosSet()
    {
        _particle.transform.position = this.transform.position;
    }
}
