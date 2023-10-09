using UnityEngine;
using UnityEngine.UI;

//カーソルの射撃に対応したボタン　
//"このスクリプトが付いたオブジェクト" についている ボタンコンポーネント に登録されたイベントを実行
//BoxCollider2Dは自分でつける必要あり(SizeはImageと同じにすること)
public class CursorButton : MonoBehaviour
{
    [SerializeField, Tooltip("何も設定しなかった場合は無音")]
    private AudioClip clickSE;

    private Button _button;
    bool _isHitCursor;

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (_button == null)
            Debug.LogError(this.name + "にButtonコンポーネントが付いていないので、付けてください。");
    }

    private void Start()
    {
        //インプット系のインスタンスが存在しているか確認
        if (InputReciever.Instance == null)
            Debug.LogError("InputRecieverのインスタンスが作成されていません。\nヒエラルキーに空のオブジェクト(名前は自由)を設置し、InputRecieverを付けてください。");
        if (JoyconCheckInput.Instance == null)
            Debug.LogError("JoyconCheckInputのインスタンスが作成されていません。");
    }

    private void Update()
    {
        //OnTriggerStay2Dだと1回押しただけで複数呼ばれるため、フラグを用意して対応
        if (_isHitCursor)
        {
            
            //射撃ボタン押下で登録されているイベントを呼ぶ InputReciever.Instance.OneShot.WasPressedThisFrame()
            if (InputReciever.Instance.OneShot.WasPressedThisFrame() || JoyconCheckInput.Instance.m_pressedButtonR == Joycon.Button.SHOULDER_2)
            {
                //効果音再生
                if (clickSE != null)
                    SoundManager.Instance.Play(clickSE);

                _button.onClick.Invoke();
            }
        }
    }

    //============カーソル重なりフラグ管理============
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cursor"))
            _isHitCursor = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cursor"))
            _isHitCursor = false;
    }
    //==============================================
}
