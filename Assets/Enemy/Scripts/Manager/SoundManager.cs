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

    //����炷AudisoSource�𕡐�����
    [SerializeField] AudioSource[] audioSources = new AudioSource[(int)Sound.MaxCount];
    //directory�ƈꏏ�ɃT�E���h���\�[�X��Dictionary�ɕۑ�
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    //------------------------------------------
    //CI0329
    private class SwapSound
    {
        private AudioClip _nextSound;   //���ɗ����T�E���h
        private float _fadeCount;       //�t�F�[�h�C���A�A�E�g�̎��Ԍv��
        private float _fadeOutTime;     //�t�F�[�h�A�E�g����
        private float _fadeInTime;      //�t�F�[�h�C������
        private bool _finishFadeOut;    //�t�F�[�h�A�E�g���I��������
        private Sound _soundType;       //�ύX����T�E���h�̎��
        private float _originalVolume;  //���̉���
        private float _nextSoundsPlayOffset;      //���ɗ����T�E���h�̈ʒu

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

        //�I��������false
        public bool UpDate()
        {
            //�t�F�[�h�A�E�g
            if (_finishFadeOut == false)
            {
                if (_fadeCount < _fadeOutTime)
                {
                    SoundManager.Instance.SetVolume(_originalVolume * Mathf.Max(0.0f, 1 - (_fadeCount / _fadeOutTime)), _soundType);
                }
            }
            //�t�F�[�h�C��
            else if (_fadeCount < _fadeInTime)
            {
                SoundManager.Instance.SetVolume(_originalVolume * Mathf.Min(1.0f, _fadeCount / _fadeOutTime), _soundType);
            }
            else
            {
                SoundManager.Instance.SetVolume(_originalVolume, _soundType);
                return false;
            }


            //�J�E���g
            _fadeCount += Time.deltaTime;

            //�t�F�[�h�A�E�g�I������
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
        // ���ׂĂ�AudioSource Stop
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        // �T�E���h���\�[�X�폜
        audioClips.Clear();
    }

    //directory���󂯂Ă��̃X�N���v�g�̓��ōĐ�������
    public void Play(string path, Sound type = Sound.EFFECT, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type); //���\�[�X���擾
        Play(audioClip, type, pitch);
    }

    //�T�E���h�Đ��i���\�[�X����j
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

    //�ꎞ��~
    public void Pause(Sound type = Sound.BGM)
    {
        if (audioSources[(int)type].isPlaying == true)
        {
            audioSources[(int)type].Pause();
        }
    }

    //�ĊJ�i�ꎞ��~���̂ݎ��s�\�j
    public void Resume(Sound type = Sound.BGM)
    {
        if (audioSources[(int)type].isPlaying == false)
        {
            audioSources[(int)type].UnPause();
        }
    }

    //�؂�ւ�
    public void Swap(AudioClip nextSound, float fadeOutTime, float fadeInTime, Sound sound = Sound.BGM, float nextSoundsPlayOffset = 0)
    {
        _swapSounds.Add(new SwapSound(nextSound, fadeOutTime, fadeInTime, sound, nextSoundsPlayOffset));
    }

    //�T�E���h���\�[�X�擾
    AudioClip GetOrAddAudioClip(string path, Sound type = Sound.EFFECT)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}"; // directory��"Sounds/"���܂܂�Ă��Ȃ��ꍇ�ǉ�

        AudioClip audioClip = null;

        //���łɎ擾�ς݂̃T�E���h���m�F
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


    //�Đ����̃N���b�v�̎擾
    public AudioClip GetAudioClip(Sound type)
    {
        return audioSources[(int)type].clip;
    }

    //�Đ��ʒu�̃Z�b�g
    public void SetPlayTime(float time, Sound type)
    {
        audioSources[(int)type].time = time;
    }

    //�Đ��ʒu�̎擾
    public float GetPlayTime(Sound type)
    {
        return audioSources[(int)type].time;
    }


    private void Update()
    {
        //�T�E���h�؂�ւ������̍X�V
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

            //�I�������T�E���h�؂�ւ��̍폜
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

