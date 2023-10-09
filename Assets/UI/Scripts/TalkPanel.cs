using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TalkPanel : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Image _backGround;
    [Space]
    [SerializeField] private float _bgFadeSpeed;

    [HideInInspector] private float _talkTimeCount;

    private enum TalkState { NON, FADEIN, TALK, FADEOUT}
    [HideInInspector] private TalkState _talkState;
    
    private delegate void TalkUpDate();

    //��b�̏��
    [System.Serializable]
    public class TalkData
    {
        public string _text;
        public AudioClip _audioClip;
        public float _time;
    }

    List<TalkData> _talkData = new List<TalkData>();

    //�w�i�̃A���t�@�l�ύX
    private void SetBGAlpha(float alpha)
    {
        _backGround.color = new Color(_backGround.color.r, _backGround.color.g, _backGround.color.b, alpha);
    }
    //�w�i�̃A���t�@�l�ǉ��i�ő�ɂȂ�����false�j
    private bool AddBGAlpha(float alpha)
    {
        _backGround.color = new Color(_backGround.color.r, _backGround.color.g, _backGround.color.b, Mathf.Min(1f, _backGround.color.a + alpha));
        return _backGround.color.a < 1f;
    }
    //�w�i�̃A���t�@�l�팸�i�ŏ��ɂȂ�����false�j
    private bool SubtractBGAlpha(float alpha)
    {
        _backGround.color = new Color(_backGround.color.r, _backGround.color.g, _backGround.color.b, Mathf.Max(0f, _backGround.color.a - alpha));
        return _backGround.color.a > 0f;
    }



    // Start is called before the first frame update
    void Start()
    {
        SetBGAlpha(0f);
        _talkTimeCount = 0f;
        _talkState = TalkState.NON;
        _text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch(_talkState)
        {
            case TalkState.NON:
                UpDateNon();
                break;
            case TalkState.FADEIN:
                UpDateFadeIn();
                break;    
            case TalkState.TALK:
                UpDateTalk();
                break;
            case TalkState.FADEOUT:
                UpDateFadeOut();
                break;
        }
    }



    //�[�[���ꂼ��̏�Ԃ̍X�V�[�[
    //��b�������
    private void UpDateNon()
    {
        if(_talkData.Count > 0)
        {
            _talkState = TalkState.FADEIN;
        }
    }
    //�t�F�[�h�C��
    private void UpDateFadeIn()
    {
        if(AddBGAlpha(_bgFadeSpeed) == false)
        {
            _talkState = TalkState.TALK;
            _text.enabled = true;
            InitTalk(_talkData[0]);
        }
    }
    //��b
    private void UpDateTalk()
    {
        //�ݒ肵�����Ԃ��o�߂����玟�̉�b��
        if (_talkTimeCount < _talkData[0]._time)
        {
            _talkTimeCount += Time.deltaTime;
        }
        else
        {
            _talkData.RemoveAt(0);
            if (_talkData.Count > 0)
            {
                InitTalk(_talkData[0]);
            }
            else
            {
                _text.text = "";
                _text.enabled = false;
                _talkState = TalkState.FADEOUT;
            }
        }
    }
    //�t�F�[�h�A�E�g
    private void UpDateFadeOut()
    {
        //�t�F�[�h�A�E�g���I���������b������Ԃ�
        if (SubtractBGAlpha(_bgFadeSpeed) == false)
        {
            _talkState = TalkState.NON;
        }

        //�t�F�[�h�A�E�g���ɒǉ����ꂽ��A�t�F�[�h�C���Ɉړ�
        if (_talkData.Count > 0)
        {
            _talkState = TalkState.FADEIN;
        }
    }
    //�[�[�[�[�[�[�[�[�[�[�[�[�[�[


    //��b�̊J�n
    private void InitTalk(TalkData talkData)
    {
        _text.text = talkData._text;
        SoundManager.Instance.Play(talkData._audioClip, SoundManager.Sound.Talk);
        _talkTimeCount = 0;
    }

    //��b�̒ǉ�
    public void AddTalk(TalkData talkData)
    {
        _talkData.Add(talkData);
    }

    //��b�̎擾
    public string GetTalk()
    {
        return _text.text;
    }
}