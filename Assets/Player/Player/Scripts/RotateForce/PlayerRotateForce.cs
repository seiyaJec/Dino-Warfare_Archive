/*this code is written in UTF-8*/
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using DG.Tweening;

public class PlayerRotateForce : MonoBehaviour
{
    [Header("アタッチ")]
    [HideInInspector] private CursorMove cursor;                                                  //これの移動を封じたい
    [HideInInspector] private Camera mainCam;                                                     //メインカメラ
    [HideInInspector] public List<PlayerRotateInfo> rotater = new List<PlayerRotateInfo>();       //回転タイミングを示すオブジェクト
    [HideInInspector, Range(0f, 1f)] private float t;

    public class RotRange
    {
        public float start;
        public float now;
    }
    [HideInInspector]private RotRange rrYaw = new RotRange();
    [HideInInspector]private RotRange rrPitch = new RotRange();
    [HideInInspector]private RotRange rrRoll = new RotRange();
    [HideInInspector]private RotRange rrFov = new RotRange();

    [HideInInspector] private bool initialized = false;
    [HideInInspector] private bool finishedEase = false;


    private void Awake()
    {
        mainCam = GetComponentInChildren<Camera>();
        cursor = mainCam.GetComponent<CursorMove>();
    }



    /// <summary>
    /// rotaterによって回転
    /// </summary>
    private void Update()
    {
        if (rotater.Count > 0)
        {
            if (rotater[0].IsFinishedEvent())
            {
                //初期化
                if (initialized == false)
                {
                    RotateInitialize(rotater[0]);
                    initialized = true;
                }
                //更新
                if (isFinishRotate(rotater[0]) == false)
                {
                    RotateUpDate(rotater[0]);
                }
                //終了＋次に進める
                else
                {
                    initialized = false;
                    rotater.RemoveAt(0);
                }
            }
        }
    }



    //回転処理の初期化
    void RotateInitialize(PlayerRotateInfo rotI)
    {
        cursor.FreezeThawCursor(rotI.freezeCursor);

        rrYaw.start = transform.eulerAngles.y;
        rrYaw.now = rrYaw.start;

        rrPitch.start = transform.eulerAngles.x;
        rrPitch.now = rrPitch.start;

        rrRoll.start = transform.eulerAngles.z;
        rrRoll.now = rrRoll.start;

        rrFov.start = mainCam.fieldOfView;
        rrFov.now = rrFov.start;

        t = 0f;

        //イージングが有効なら行う
        if(rotI.useEase)
        {
            transform.DORotate(new Vector3(rotI.pitch, rotI.yaw, rotI.roll), rotI.duration).SetEase(rotI.easeType).OnComplete(SetFinishEase);
            finishedEase = false;
        }
    }



    //プレイヤーの回転
    void RotateUpDate(PlayerRotateInfo rotI)
    {
        //時間計測
        t += Time.deltaTime / rotI.duration;

        //視野は変更が有効なら行う
        if (rotI.ChangeFov == true)
        {
            rrFov.now = Mathf.Lerp(rrFov.start, rotI.FOV, t);
            mainCam.fieldOfView = rrFov.now;
        }

        //イージングの場合はここで終了
        if (rotI.useEase)
        {
            return;
        }

        //時間に合わせて値を変更
        rrYaw.now = Mathf.Lerp(rrYaw.start, rotI.yaw, t);
        rrPitch.now = Mathf.Lerp(rrPitch.start, rotI.pitch, t);
        rrRoll.now = Mathf.Lerp(rrRoll.start, rotI.roll, t);

        //現在の値を反映
        transform.rotation = Quaternion.Euler(rrPitch.now, rrYaw.now, rrRoll.now);

    }


    //DoTweenにコールバックとして入れる関数
    void SetFinishEase()
    {
        finishedEase = true;
    }



    //回転が終わったかを確認する
    bool isFinishRotate(PlayerRotateInfo rotI)
    {
        //視野
        if (rotI.ChangeFov == true && rrFov.now != rotI.FOV)
            return false;

        //イージング
        if (rotI.useEase == true)
        {
            if (finishedEase == true)
                return true;
            else 
                return false;
        }

        //それぞれの角度
        if(rrYaw.now != rotI.yaw)
            return false;
        if (rrPitch.now != rotI.pitch)
            return false;
        if (rrRoll.now != rotI.roll) 
            return false;


        return true;
    }

}
