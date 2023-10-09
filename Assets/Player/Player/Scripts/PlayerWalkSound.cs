using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkSound : MonoBehaviour
{
    [System.Serializable]
    public class WalkSoundCollection
    {
        [SerializeField] public string _soundName;
        [SerializeField] public AudioClip[] _sounds;
    }

    [SerializeField] private float _playDistance;   //����炷�Ԋu
    [SerializeField] private WalkSoundCollection[] _walkSound;  //������
    [SerializeField] private string _initSound;                 //�ŏ��̉�
    [HideInInspector] private int _playIndex;                   //���݂̉�
    [HideInInspector] private float _walkedDistance;     //�����������̌v��
    [HideInInspector] private int _nextSound;          //�������Ԃɖ炷���߂̃J�E���^
    [HideInInspector] private Vector3 _prePos;      //�O�t���[���̈ʒu

    void Start()
    {
        _walkedDistance = 0;
        _prePos = transform.position;
        _playIndex = SearchIndex(_initSound);
        _nextSound = 0;
    }


    void Update()
    {
        if (_walkSound[_playIndex]._sounds.Length <= 0)
        {
            _walkedDistance = 0;
            _prePos = transform.position;
            return;
        }
        _walkedDistance += (transform.position - _prePos).magnitude;
        _prePos = transform.position;

        //������������playDistance�𒴂�����炷
        if(_walkedDistance >= _playDistance)
        {
            SoundManager.Instance.Play(_walkSound[_playIndex]._sounds[_nextSound % _walkSound[_playIndex]._sounds.Length], SoundManager.Sound.EFFECT);
            ++_nextSound;
            _walkedDistance -= _playDistance;
        }
    }

    //�����̕ύX
    public void SetWalkSound(float playDistance, string soundName)
    {
        _playDistance = playDistance;
        _playIndex = SearchIndex(soundName);
    }

    //������̍����v�f��T���i������Ȃ��ꍇ��0�j
    private int SearchIndex(string name)
    {
        int result = 0;
        foreach(var wsound in _walkSound)
        {
            if(wsound._soundName == name)
            {
                return result;
            }
            ++result;
        }
        return 0;
    }
}
