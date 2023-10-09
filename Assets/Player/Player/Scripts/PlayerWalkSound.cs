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

    [SerializeField] private float _playDistance;   //音を鳴らす間隔
    [SerializeField] private WalkSoundCollection[] _walkSound;  //歩く音
    [SerializeField] private string _initSound;                 //最初の音
    [HideInInspector] private int _playIndex;                   //現在の音
    [HideInInspector] private float _walkedDistance;     //歩いた距離の計測
    [HideInInspector] private int _nextSound;          //音を順番に鳴らすためのカウンタ
    [HideInInspector] private Vector3 _prePos;      //前フレームの位置

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

        //歩いた距離がplayDistanceを超えたら鳴らす
        if(_walkedDistance >= _playDistance)
        {
            SoundManager.Instance.Play(_walkSound[_playIndex]._sounds[_nextSound % _walkSound[_playIndex]._sounds.Length], SoundManager.Sound.EFFECT);
            ++_nextSound;
            _walkedDistance -= _playDistance;
        }
    }

    //足音の変更
    public void SetWalkSound(float playDistance, string soundName)
    {
        _playDistance = playDistance;
        _playIndex = SearchIndex(soundName);
    }

    //文字列の合う要素を探す（見つからない場合は0）
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
