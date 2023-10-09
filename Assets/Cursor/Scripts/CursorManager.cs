using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�\���A��\�����@�t���O�ɂ����ʂ��Ǘ�

public class CursorManager : MonoBehaviour
{
    //�t���O�Ǘ��N���X
    CursorFlagManager flagManager;

    //�C�x���g�N���X
    private HideCursor _hideCursor;
    

    // Start is called before the first frame update
    void Start()
    {
        flagManager = GameObject.Find("CursorFlagManager").GetComponent<CursorFlagManager>();
        if (flagManager == null)
            Debug.LogError(this + "�̕ϐ�flagManager" + "��null�ł�");
        _hideCursor = new HideCursor(GetComponent<Image>());
    }

    // Update is called once per frame
    void Update()
    {
        if (flagManager.GetFlag() == CursorFlag.Flag.Hide)
        {
            _hideCursor.Hide();
        }
        else
        {
            _hideCursor.Visible();
        }
    }
}
