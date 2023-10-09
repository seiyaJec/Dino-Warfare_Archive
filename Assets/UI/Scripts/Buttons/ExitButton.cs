using UnityEngine;
using System.Collections;

//�I���{�^���@�Q�[�����I������
public class ExitButton : MonoBehaviour
{
    [SerializeField,
    Tooltip("�{�^���������Ă���V�[���J�ڂ���܂ł̎���(0���ƌ��ʉ�����Ȃ�)"),
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
