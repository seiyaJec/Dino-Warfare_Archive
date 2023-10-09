using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 導入UI
/// </summary>
public class Panel_Introduction : MonoBehaviour
{
    bool isSkipped; //飛ばす場合はtrue

    [Header("アタッチ：自分")]
    [SerializeField] CanvasGroup CG;            //パネルの表示管理
    [Space(10)]
    [Header("アタッチ：子供")]
    [SerializeField] TMP_Text text_Mission; //MISSION
    [SerializeField] TMP_Text text_Subject; //ミッション名
    [SerializeField] TMP_Text text_Detail;  //ミッション詳細
    [SerializeField] Image image;           //ミッション用画像
    [SerializeField] GameObject button_Skip;    //スキップ用ボタン
    [Space(10)]
    [Header("デバッグ")]
    [SerializeField] float timeCnt;     //時間経過
    [SerializeField] int eventFlag;     //イベント発生回数


    // Start is called before the first frame update
    void Start()
    {
        isSkipped = false;
        timeCnt = 0;
        text_Mission.maxVisibleCharacters = 0;
        text_Mission.color = new Color(1.0f,1.0f,1.0f,0.0f);
        text_Subject.maxVisibleCharacters = 0;
        text_Detail.maxVisibleCharacters = 0;
        image.color = new Color(0,0,0,0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSkipped)
        {
            ShowIntroduction();
        }
        timeCnt += Time.deltaTime;
    }

    /// <summary>
    /// 導入表示イベント
    /// 経過時間に応じて各イベントをトリガー
    /// </summary>
    void ShowIntroduction()
    {
        StartCoroutine(Zero());
        if(timeCnt >= 1)StartCoroutine(One());
        if(timeCnt >= 20)StartCoroutine(Two());
        //以下必要に応じて追加
        //if(timeCnt >= 経過秒数)StartCoroutine(関数名()):
    }

    /// <summary>
    /// イベントを終了
    /// </summary>
    IEnumerator EndEvent()
    {
        /*パネルを消してシーン遷移*/
        DecreaseCGAlpha(CG);
        yield return new WaitForSeconds(1.5f);
        LoadingSceneController.LoadScene("CameraStageTest1");
    }

    //---------------------------------------------------
    /*時間経過で発生するイベント 各一回のみ発動するようにする*/
    IEnumerator Zero()
    {
        if(eventFlag != 0)yield break;
        eventFlag++;

        /*パネルを表示*/
        IncreaseCGAlpha(CG);
    }
    IEnumerator One()
    {
        if(eventFlag != 1)yield break;
        eventFlag++;

        /*テキストと画像を表示*/
        StartCoroutine(LoadTMPText(text_Mission,0.2f));
        yield return new WaitForSeconds(0.2f);
        text_Mission.color = new Color(1.0f,0.0f,0.0f,1.0f);
        yield return new WaitForSeconds(2);
        StartCoroutine(LoadTMPText(text_Subject,0.1f));
        yield return new WaitForSeconds(4);
        image.color = new Color(1.0f,1.0f,1.0f,1.0f);           //画像
        //button_Skip.SetActive(true);                             //スキップボタン
        StartCoroutine(LoadTMPText(text_Detail,0.05f));
        yield return new WaitForSeconds(4);
        OverWriteTMP(text_Detail,"被害がこれ以上広がる前に、\n彼らを制圧せよ",0.05f);  //すでに表示されているテキストを変更
        yield return new WaitForSeconds(4);
        OverWriteTMP(text_Detail,"恐竜たちは最新兵器を装備している\n攻撃に気を配りつつ、弱点を突き止めろ",0.05f);
    }
    IEnumerator Two()
    {
        if(eventFlag != 2)yield break;
        eventFlag++;
        StartCoroutine(EndEvent());
    }
    //同時並行で処理を行いたい場合は必要に応じて追加
    // IEnumerator 関数名()
    // {
    //     if(eventFlag != トリガー時の数値)yield break;
    //     eventFlag++;

    //     処理
    // }
    //---------------------------------------------------


    /*パネルを透明にする(CanvasGroupのアタッチが必要)*/
    async void DecreaseCGAlpha(CanvasGroup cg)
    {
        for(int i = 0; i < 10; ++i)
        {
            cg.alpha -= 0.1f;
            await Task.Delay(50);
        }
    }
    /*パネルを不透明にする(CanvasGroupのアタッチが必要)*/
    async void IncreaseCGAlpha(CanvasGroup cg)
    {
        for(int i = 0; i < 10; ++i)
        {
            cg.alpha += 0.1f;
            await Task.Delay(50);
        }
    }

    /// <summary>
    /// テキストの文字送り
    /// https://nekojara.city/unity-textmesh-pro-typewriter-effect
    /// </summary>
    /// <param name="_TMP">文字送りしたいテキスト</param>
    /// <param name="duration">壱文字ごとのウェイト</param>
    IEnumerator LoadTMPText(TMP_Text _TMP,float duration)
    {
        for(var i = 0; i <= _TMP.text.Length; i++)
        {
            yield return new WaitForSeconds(duration);
            _TMP.maxVisibleCharacters = i;
        }
    }
    /// <summary>
    /// テキストの情報を上書きして再表示
    /// </summary>
    /// <param name="_TMP">文字送りしたいテキスト</param>
    /// <param name="_text">新しいテキスト内容</param>
    /// <param name="duration">壱文字ごとのウェイト</param>
    void OverWriteTMP(TMP_Text _TMP,string _text,float duration)
    {
        _TMP.text = _text;
        _TMP.maxVisibleCharacters = 0;
        StartCoroutine(LoadTMPText(_TMP,duration));
    }

    //---------------------------------------------------
    //ボタン処理
    /// <summary>
    /// イベントをスキップして次に進む
    /// </summary>
    public void Skip()
    {
        isSkipped = true;
        StartCoroutine(EndEvent());
    }
    //---------------------------------------------------
}
