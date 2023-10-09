using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//表示、非表示等　フラグによる効果を管理

public class CursorManager : MonoBehaviour
{
    //フラグ管理クラス
    CursorFlagManager flagManager;

    //イベントクラス
    private HideCursor _hideCursor;
    

    // Start is called before the first frame update
    void Start()
    {
        flagManager = GameObject.Find("CursorFlagManager").GetComponent<CursorFlagManager>();
        if (flagManager == null)
            Debug.LogError(this + "の変数flagManager" + "がnullです");
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
