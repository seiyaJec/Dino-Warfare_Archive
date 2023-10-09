using UnityEngine;
using System.Collections;

//スタートボタン
public class StartButton : MonoBehaviour
{
    [SerializeField, 
        Tooltip("ボタンを押してからシーン遷移するまでの時間(0だと効果音が鳴らない)"),
        Range(0.0f, 1.0f)] 
    float waitTime;

    //少し待ってからメインのゲームシーンへ遷移
    public void StartGame_LittleWait()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(waitTime);
        LoadingSceneController.LoadScene("Introduction");
    }
}