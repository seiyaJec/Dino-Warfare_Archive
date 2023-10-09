using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static SoundManager;

public class SoundManager : MonoBehaviour
{
    //Singleton
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
            }
            return instance;
        }
    }

    private AudioSource audioPlayer;

    public enum Sound
    {
        BGM,
        EFFECT,
        UI,
        Talk,
        MaxCount,  //Length
    }

    //音を鳴らすAudisoSourceを複数所持
    [SerializeField] AudioSource[] audioSources = new AudioSource[(int)Sound.MaxCount];
    //directoryと一緒にサウンドリソースをDictionaryに保存
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    //------------------------------------------
    //CI0329
    private class SwapSound
    {
        private AudioClip _nextSound;   //次に流すサウンド
        private float _fadeCount;       //フェードイン、アウトの時間計測
        private float _fadeOutTime;     //フェードアウト時間
        private float _fadeInTime;      //フェードイン時間
        private bool _finishFadeOut;    //フェードアウトが終了したか
        private Sound _soundType;       //変更するサウンドの種類
        private float _originalVolume;  //元の音量
        private float _nextSoundsPlayOffset;      //次に流すサウンドの位置

        public SwapSound(AudioClip nextSound, float fadeOutTime, float fadeInTime, Sound sound = Sound.BGM, float nextSoundsPlayOffset = 0)
        {
            _nextSound = nextSound;
            _fadeCount = 0;
            _fadeOutTime = fadeOutTime;
            _fadeInTime = fadeInTime;
            _finishFadeOut = false;
            _soundType = sound;
            _originalVolume = SoundManager.Instance.GetVolume(sound);
            _nextSoundsPlayOffset = nextSoundsPlayOffset;
        }

        //終了したらfalse
        public bool UpDate()
        {
            //フェードアウト
            if (_finishFadeOut == false)
            {
                if (_fadeCount < _fadeOutTime)
                {
                    SoundManager.Instance.SetVolume(_originalVolume * Mathf.Max(0.0f, 1 - (_fadeCount / _fadeOutTime)), _soundType);
                }
            }
            //フェードイン
            else if (_fadeCount < _fadeInTime)
            {
                SoundManager.Instance.SetVolume(_originalVolume * Mathf.Min(1.0f, _fadeCount / _fadeOutTime), _soundType);
            }
            else
            {
                SoundManager.Instance.SetVolume(_originalVolume, _soundType);
                return false;
            }


            //カウント
            _fadeCount += Time.deltaTime;

            //フェードアウト終了判定
            if (_finishFadeOut == false && _fadeCount >= _fadeOutTime)
            {
                _finishFadeOut = true;
                SoundManager.Instance.SetPlayTime(_nextSoundsPlayOffset, _soundType);
                SoundManager.Instance.Play(_nextSound, _soundType);
                _fadeCount = 0;
            }
            return true;
        }

    }

    [SerializeField] AudioClip initBGM;
    private void Start()
    {
        Play(initBGM, Sound.BGM);
    }


    [HideInInspector] List<SwapSound> _swapSounds = new List<SwapSound>();
    //------------------------------------------

    public void Clear()
    {
        // すべてのAudioSource Stop
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        // サウンドリソース削除
        audioClips.Clear();
    }

    //directoryを受けてこのスクリプトの内で再生させる
    public void Play(string path, Sound type = Sound.EFFECT, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type); //リソースを取得
        Play(audioClip, type, pitch);
    }

    //サウンド再生（リソースあり）
    public void Play(AudioClip audioClip, Sound type = Sound.EFFECT, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Sound.BGM) //Background music
        {
            AudioSource audioSource = audioSources[(int)Sound.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else if (type == Sound.EFFECT) // Effect Sound
        {
            AudioSource audioSource = audioSources[(int)Sound.EFFECT];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
        else if (type == Sound.UI) // UI Sosund
        {
            AudioSource audioSource = audioSources[(int)Sound.UI];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
        else if(type == Sound.Talk) //UI Sound
        {
            AudioSource audioSource = audioSources[(int)Sound.Talk];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    //一時停止
    public void Pause(Sound type = Sound.BGM)
    {
        if (audioSources[(int)type].isPlaying == true)
        {
            audioSources[(int)type].Pause();
        }
    }

    //再開（一時停止時のみ実行可能）
    public void Resume(Sound type = Sound.BGM)
    {
        if (audioSources[(int)type].isPlaying == false)
        {
            audioSources[(int)type].UnPause();
        }
    }

    //切り替え
    public void Swap(AudioClip nextSound, float fadeOutTime, float fadeInTime, Sound sound = Sound.BGM, float nextSoundsPlayOffset = 0)
    {
        _swapSounds.Add(new SwapSound(nextSound, fadeOutTime, fadeInTime, sound, nextSoundsPlayOffset));
    }

    //サウンドリソース取得
    AudioClip GetOrAddAudioClip(string path, Sound type = Sound.EFFECT)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}"; // directoryに"Sounds/"が含まれていない場合追加

        AudioClip audioClip = null;

        //すでに取得済みのサウンドか確認
        if (audioClips.TryGetValue(path, out audioClip) == false)
        {
            audioClip = Resources.Load<AudioClip>(path);
            audioClips.Add(path, audioClip);
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }

    //Volume Setting 
    public void SetMusicVolume(float volume)
    {
        audioSources[(int)Sound.BGM].volume = volume;
    }
    public void SetSfxVolume(float volume)
    {
        audioSources[(int)Sound.EFFECT].volume = volume;
    }
    public void SetTalkVolume(float volume)
    {
        audioSources[(int)Sound.Talk].volume = volume;
    }

    public void SetVolume(float volume, Sound type)
    {
        audioSources[(int)type].volume = volume;
    }

    //GetVolume
    public float GetMusicVolume()
    {
        return audioSources[(int)Sound.BGM].volume;
    }
    public float GetSfxVolume()
    {
        return audioSources[(int)Sound.EFFECT].volume;
    }
    public float GetTalkVolume()
    {
        return audioSources[(int)Sound.Talk].volume;
    }

    public float GetVolume(Sound type)
    {
        return audioSources[(int)type].volume;
    }


    //再生中のクリップの取得
    public AudioClip GetAudioClip(Sound type)
    {
        return audioSources[(int)type].clip;
    }

    //再生位置のセット
    public void SetPlayTime(float time, Sound type)
    {
        audioSources[(int)type].time = time;
    }

    //再生位置の取得
    public float GetPlayTime(Sound type)
    {
        return audioSources[(int)type].time;
    }


    private void Update()
    {
        //サウンド切り替え処理の更新
        if(_swapSounds.Count > 0)
        {
            List<SwapSound> finishedSwapSounds = new List<SwapSound>();
            foreach(var swapSound in _swapSounds)
            {
                if(swapSound.UpDate() == false)
                {
                    finishedSwapSounds.Add(swapSound);
                }
            }

            //終了したサウンド切り替えの削除
            if (finishedSwapSounds.Count > 0)
            {
                foreach(var swapSound in finishedSwapSounds)
                {
                    _swapSounds.Remove(swapSound);
                }
            }
        }
    }
}

