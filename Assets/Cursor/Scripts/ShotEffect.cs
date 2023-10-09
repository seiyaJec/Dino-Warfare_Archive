using UnityEngine;

//EffectManager�g�������������ǁAScreenToWorldPoint�Ƒ������������Ȃ��߁A��������ׂ�����
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
        //�Đ��J�n
        if (InputReciever.Instance.OneShot.WasPressedThisFrame())
        {
            EffectPosSet();
            //�G�t�F�N�g�Đ�
            _particle.Play();
            _isPlayingParticle = true;
        }

        //�Đ���
        if (_isPlayingParticle)
        {
            //�Đ����ԃJ�E���g
            _particleTime += Time.deltaTime;
            EffectPosSet();
        }

        //�������ԉ߂�����Đ���~
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
