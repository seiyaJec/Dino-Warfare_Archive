using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroductionManager : MonoBehaviour
{

    [SerializeField] private VideoPlayer _video;    //再生中かチェックする動画
    [HideInInspector] private float _waitLoad = 3;      //シーン変更を待つ時間
    [HideInInspector] private float _timeCount = 0;     //時間計測

    private void Update()
    {
        if (_timeCount >= _waitLoad)
        {
            if (_video.isPlaying == false)
            {
                LoadingSceneController.LoadScene("CameraStageTest1");
            }
        }

        _timeCount += Time.deltaTime;
    }
}
