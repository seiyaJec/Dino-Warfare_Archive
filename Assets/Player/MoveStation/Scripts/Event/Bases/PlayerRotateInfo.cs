using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

[System.SerializableAttribute]
public class PlayerRotateInfo
{
    [HideInInspector, Range(0f, 1f)] private float t;         //時間の計測
    [HideInInspector] private List<IEventCheck> eventCheck = new List<IEventCheck>();   //イベントチェック

    [Header("パラメータ")]
    [SerializeField] private List<EventCheckSender> eventCheckSender;   //イベントチェックを取得するための変数

    //回転
    [SerializeField] public bool freezeCursor;
    [SerializeField] public float yaw;
    [SerializeField] public float pitch;
    [SerializeField] public float roll;
    [SerializeField, Range(0.001f, 10000f)] public float duration;                    //回転にかかる時間
 
    //視野
    [Header("チェックを付けると視野の変更が反映されます")]
    [SerializeField] public bool ChangeFov;                    //視野の変更をするか
    [SerializeField, Range(1f, 179f)] public float FOV;                         //視野の変更

    //イージング
    [Header("イージング（こちらもチェックすると反映されます）")]
    [SerializeField] public bool useEase;
    [SerializeField] public Ease easeType;

    //インターフェースを取得する
    public void Initialize()
    {
        if (eventCheckSender.Count > 0)
        {
            foreach (var sender in eventCheckSender)
            {
                var eventcheck = sender.GetEventCheck();
                if (eventcheck != null)
                {
                    eventCheck.Add(eventcheck);
                }
                else
                {
                    Debug.Log("PlayerRotateInfo：中身のないイベントチェックがあります");
                }
            }
        }
        else
        {
            eventCheck.Add(new EventNoneCheck());
        }

        eventCheckSender.Clear();
    }

    //設定したイベントチェックが全て終了したか
    public bool IsFinishedEvent()
    {
        if (eventCheck != null)
        {
            foreach (var evc in eventCheck)
            {
                if (evc.Finished() == false)
                {
                    return false;
                }
            }
        }

        return true;
    }




}
