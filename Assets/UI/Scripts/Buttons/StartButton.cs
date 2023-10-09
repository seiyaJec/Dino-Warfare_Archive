using UnityEngine;
using System.Collections;

//�X�^�[�g�{�^��
public class StartButton : MonoBehaviour
{
    [SerializeField, 
        Tooltip("�{�^���������Ă���V�[���J�ڂ���܂ł̎���(0���ƌ��ʉ�����Ȃ�)"),
        Range(0.0f, 1.0f)] 
    float waitTime;

    //�����҂��Ă��烁�C���̃Q�[���V�[���֑J��
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