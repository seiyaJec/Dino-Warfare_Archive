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

    //会話の情報
    [System.Serializable]
    public class TalkData
    {
        public string _text;
        public AudioClip _audioClip;
        public float _time;
    }

    List<TalkData> _talkData = new List<TalkData>();

    //背景のアルファ値変更
    private void SetBGAlpha(float alpha)
    {
        _backGround.color = new Color(_backGround.color.r, _backGround.color.g, _backGround.color.b, alpha);
    }
    //背景のアルファ値追加（最大になったらfalse）
    private bool AddBGAlpha(float alpha)
    {
        _backGround.color = new Color(_backGround.color.r, _backGround.color.g, _backGround.color.b, Mathf.Min(1f, _backGround.color.a + alpha));
        return _backGround.color.a < 1f;
    }
    //背景のアルファ値削減（最小になったらfalse）
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



    //ーーそれぞれの状態の更新ーー
    //会話無し状態
    private void UpDateNon()
    {
        if(_talkData.Count > 0)
        {
            _talkState = TalkState.FADEIN;
        }
    }
    //フェードイン
    private void UpDateFadeIn()
    {
        if(AddBGAlpha(_bgFadeSpeed) == false)
        {
            _talkState = TalkState.TALK;
            _text.enabled = true;
            InitTalk(_talkData[0]);
        }
    }
    //会話
    private void UpDateTalk()
    {
        //設定した時間が経過したら次の会話へ
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
    //フェードアウト
    private void UpDateFadeOut()
    {
        //フェードアウトが終了したら会話無し状態へ
        if (SubtractBGAlpha(_bgFadeSpeed) == false)
        {
            _talkState = TalkState.NON;
        }

        //フェードアウト中に追加されたら、フェードインに移動
        if (_talkData.Count > 0)
        {
            _talkState = TalkState.FADEIN;
        }
    }
    //ーーーーーーーーーーーーーー


    //会話の開始
    private void InitTalk(TalkData talkData)
    {
        _text.text = talkData._text;
        SoundManager.Instance.Play(talkData._audioClip, SoundManager.Sound.Talk);
        _talkTimeCount = 0;
    }

    //会話の追加
    public void AddTalk(TalkData talkData)
    {
        _talkData.Add(talkData);
    }

    //会話の取得
    public string GetTalk()
    {
        return _text.text;
    }
}