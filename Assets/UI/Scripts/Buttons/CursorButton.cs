using UnityEngine;
using UnityEngine.UI;

//�J�[�\���̎ˌ��ɑΉ������{�^���@
//"���̃X�N���v�g���t�����I�u�W�F�N�g" �ɂ��Ă��� �{�^���R���|�[�l���g �ɓo�^���ꂽ�C�x���g�����s
//BoxCollider2D�͎����ł���K�v����(Size��Image�Ɠ����ɂ��邱��)
public class CursorButton : MonoBehaviour
{
    [SerializeField, Tooltip("�����ݒ肵�Ȃ������ꍇ�͖���")]
    private AudioClip clickSE;

    private Button _button;
    bool _isHitCursor;

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (_button == null)
            Debug.LogError(this.name + "��Button�R���|�[�l���g���t���Ă��Ȃ��̂ŁA�t���Ă��������B");
    }

    private void Start()
    {
        //�C���v�b�g�n�̃C���X�^���X�����݂��Ă��邩�m�F
        if (InputReciever.Instance == null)
            Debug.LogError("InputReciever�̃C���X�^���X���쐬����Ă��܂���B\n�q�G�����L�[�ɋ�̃I�u�W�F�N�g(���O�͎��R)��ݒu���AInputReciever��t���Ă��������B");
        if (JoyconCheckInput.Instance == null)
            Debug.LogError("JoyconCheckInput�̃C���X�^���X���쐬����Ă��܂���B");
    }

    private void Update()
    {
        //OnTriggerStay2D����1�񉟂��������ŕ����Ă΂�邽�߁A�t���O��p�ӂ��đΉ�
        if (_isHitCursor)
        {
            
            //�ˌ��{�^�������œo�^����Ă���C�x���g���Ă� InputReciever.Instance.OneShot.WasPressedThisFrame()
            if (InputReciever.Instance.OneShot.WasPressedThisFrame() || JoyconCheckInput.Instance.m_pressedButtonR == Joycon.Button.SHOULDER_2)
            {
                //���ʉ��Đ�
                if (clickSE != null)
                    SoundManager.Instance.Play(clickSE);

                _button.onClick.Invoke();
            }
        }
    }

    //============�J�[�\���d�Ȃ�t���O�Ǘ�============
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
