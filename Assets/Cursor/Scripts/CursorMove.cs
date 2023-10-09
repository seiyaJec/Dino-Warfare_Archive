using UnityEngine;
using UnityEngine.UI;

public class CursorMove : MonoBehaviour
{
    [SerializeField]
    private Image _img;
    private JoyconCheckInput _joycon;

    public Image Img
    { get
        {
            return _img;
        }
    }
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _moveSpeedGyro;
    [SerializeField]
    private Joycon.Button _resetPosButton;


    private Vector3 _initPos;

    private Rect _screenEdgeRect;
    private bool freezeMove;                    //�s���s�\

    //�B��J�[�\�����W
    private Vector3 _hidePos;

    public void ResetPos()
    {
        _hidePos = _initPos;
        _img.transform.localPosition = _initPos;
    }

    private void Awake()
    {
        _joycon = JoyconCheckInput.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        freezeMove = false;
        _initPos = _img.transform.localPosition;
        _hidePos = _initPos;
        _screenEdgeRect = new Rect
        {
            //����
            xMin = Screen.width  * -0.5f,
            yMin = Screen.height * -0.5f,
            //�E��
            xMax = Screen.width  * 0.5f,
            yMax = Screen.height * 0.5f
        };
    }

    // �ړ�
    void Update()
    {
        //�ړ��ł��Ȃ��^�C�~���O�Ȃ珈������߂�
        if(freezeMove)return;
        
        Vector2 _moveDirection = Vector2.zero;
        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        _moveDirection.y = Input.GetAxisRaw("Vertical");
        
        Vector3 _velocity = _moveSpeed * Time.deltaTime * _moveDirection;
        _velocity += GetJoyConGyro() * _moveSpeedGyro;
        //���͕����ֈړ�
        _hidePos += _velocity;

        //�ړ�����
        Vector3 postPos = _img.transform.localPosition;
        //����
        if (!OutOfScreen_Vertical(_hidePos.x))
        {
            postPos.x += _velocity.x;
        }
        else
        {
            if(_hidePos.x > 0)
            {
                postPos.x = _screenEdgeRect.xMax;
            }
            else
            {
                postPos.x = _screenEdgeRect.xMin;
            }
        }
        //����
        if (!OutOfScreen_Horizontal(_hidePos.y))
        {
            postPos.y += _velocity.y;
        }
        else
        {
            if (_hidePos.y > 0)
            {
                postPos.y = _screenEdgeRect.yMax;
            }
            else
            {
                postPos.y = _screenEdgeRect.yMin;
            }
        }
        _img.transform.localPosition = postPos;


        if (Input.GetKeyDown(KeyCode.R)
            || _joycon.m_pressedButtonL == _resetPosButton)
            ResetPos();
    }

    //�߂荞�܂Ȃ��ړ�
    //Vector3 MoveInScreen(in Vector2 _setVelocity)
    //{
    //    Vector3 _result = _img.transform.localPosition;
    //    Vector2 _velocity = _setVelocity;

    //    //��������
    //    while (_velocity.x != 0.0f)
    //    {
    //        float _prePos = _result.x;

    //        if (_velocity.x > 1.0f)
    //        {
    //            _result.x += 1.0f;
    //            _velocity.x -= 1.0f;
    //        }
    //        else if (_velocity.x < -1.0f)
    //        {
    //            _result.x -= 1.0f;
    //            _velocity.x += 1.0f;
    //        }
    //        else
    //        {
    //            _result.x += _velocity.x;
    //            _velocity.x = 0.0f;
    //        }

    //        //����
    //        if (OutOfScreen(_result))
    //        {
    //            //OutOfScreen��0.0f��true��Ԃ�
    //            //prePos��0.0f�̂Ƃ���
    //            //��ʊO�ɏo���̂ɁA��ʊO�ɂ����߂��Ă��Ȃ�
    //            //����āA?�܂�H
    //            _result.x = _prePos;
    //        }
    //    }
        
    //    //��������
    //    while (_velocity.y != 0.0f)
    //    {
    //        float _prePos = _result.y;

    //        if (_velocity.y > 1.0f)
    //        {
    //            _result.y += 1.0f;
    //            _velocity.y -= 1.0f;
    //        }
    //        else if (_velocity.y < -1.0f)
    //        {
    //            _result.y -= 1.0f;
    //            _velocity.y += 1.0f;
    //        }
    //        else
    //        {
    //            _result.y += _velocity.y;
    //            _velocity.y = 0.0f;
    //        }

    //        //����
    //        if (OutOfScreen(_result))
    //        {
    //            _result.y = _prePos;
    //        }
    //    }

    //    return _result;
    //}

    //�W���C�����͏����擾
    private Vector3 GetJoyConGyro()
    {
        Vector3 velocity = Vector3.zero;
        if (_joycon.m_joyconL != null)
        {
            velocity.x = _joycon.m_joyconL.GetGyro().z;
            velocity.y = _joycon.m_joyconL.GetGyro().y;
        }
        return velocity;
    }

    //��ʊO�Ȃ�true
    bool OutOfScreen_Vertical(float pos)
    {
        if (pos <= _screenEdgeRect.xMin)
            return true;
        if (pos >= _screenEdgeRect.xMax)
            return true;

        return false;
    }
    bool OutOfScreen_Horizontal(float pos)
    {
        if (pos <= _screenEdgeRect.yMin)
            return true;
        if (pos >= _screenEdgeRect.yMax)
            return true;

        return false;
    }

    //"�J�[�\���s���s�\"�ϐ��Z�b�^�[
    public void FreezeThawCursor(bool freeze)
    {
        freezeMove = freeze;
    }
}


