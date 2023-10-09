using UnityEngine;
using System.Collections;

//終了ボタン　ゲームを終了する
public class ExitButton : MonoBehaviour
{
    [SerializeField,
    Tooltip("ボタンを押してからシーン遷移するまでの時間(0だと効果音が鳴らない)"),
    Range(0.0f, 1.0f)]
    float waitTime;

    public void ExitGame_LittleWait()
    {
        StartCoroutine(ExitGame());
    }

    IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(waitTime);
        Application.Quit();
    }
}
