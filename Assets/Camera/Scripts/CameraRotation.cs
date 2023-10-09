using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private GameObject _parentObject;

    [SerializeField]
    private CursorMove _cursor;
    private Camera _mainCamera;

    private Vector3 _screenCenter;
    private Vector3 _initForward;

    [SerializeField, Range(0.0f, 1000.0f)]
    private float _distance;
    [SerializeField, Range(0.0f, 1000.0f)]
    private float _speed;

    [Header("�p�x����")]
    [SerializeField, Tooltip("����")]
    float _horizontalAngleLimit;
    [SerializeField, Tooltip("����")]
    float _verticalAngleLimit;

    //����̊�ƂȂ�����X�V
    void UpdateStandardInfos()
    {
        _initForward = _parentObject.transform.forward;
        _screenCenter = _parentObject.transform.position + _initForward;
    }

    private void Awake()
    {
        _parentObject = transform.parent.gameObject;
    }

    private void Start()
    {
        _mainCamera = Camera.main;

        UpdateStandardInfos();
    }
    private void Update()
    {
        UpdateStandardInfos();

        ////�J�[�\���̍��W
        //Vector3 cursorPos = _cursor.Img.transform.position;
        //Vector3 lookPos = cursorPos + (_initForward * _distance);
        //Vector3 direction = lookPos - _screenCenter;

        //_mainCamera.transform.forward += direction * _speed;

        Vector3 cursorPos = _cursor.Img.transform.localPosition * _speed;

        //�p�x�␳
        Vector3 eulerAngle = new Vector3(-cursorPos.y, cursorPos.x); 
        eulerAngle.x = CorrectAngle2(eulerAngle.x, _verticalAngleLimit);
        eulerAngle.y = CorrectAngle2(eulerAngle.y, _horizontalAngleLimit);
        _mainCamera.transform.eulerAngles = transform.parent.eulerAngles + eulerAngle;

        //�J�������Z�b�g
        if (Input.GetKeyDown(KeyCode.R))
            ResetCamera();
    }

    //�J�������Z�b�g
    void ResetCamera()
    {
        _mainCamera.transform.forward = _initForward;
        _cursor.ResetPos();
    }

    //�p�x����
    float CorrectAngle(in float targetAngle, in float limitAngle)
    {
        float oneLap = 360.0f;          //����̊p�x
        float halfLap = oneLap * 0.5f;  //�����̊p�x

        //��������p�x
        float smallSideLimit = limitAngle;              //   0 ~ �����̐���
        float largeSideLimit = oneLap - smallSideLimit; //���� ~ ����̐���

        //�����ȉ�
        bool isSmallSide = targetAngle <= halfLap;

        //0 ~ ����
        if (isSmallSide)
        {
            //����
            if (targetAngle > smallSideLimit)
                return smallSideLimit;
        }
        //���� ~ ���
        else
        {
            //����
            if (targetAngle < largeSideLimit)
                return largeSideLimit;
        }

        return targetAngle;
    }

    //�p�x����
    float CorrectAngle2(in float targetAngle, in float limitAngle)
    {
        if(targetAngle < 0 && targetAngle < -limitAngle)
        {
            return -limitAngle;
        }
        else if(targetAngle > 0 &&  targetAngle > limitAngle) 
        {
            return limitAngle;
        }
        else
        {
            return targetAngle;
        }
    }

}