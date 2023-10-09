using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroductionManager : MonoBehaviour
{

    [SerializeField] private VideoPlayer _video;    //�Đ������`�F�b�N���铮��
    [HideInInspector] private float _waitLoad = 3;      //�V�[���ύX��҂���
    [HideInInspector] private float _timeCount = 0;     //���Ԍv��

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
