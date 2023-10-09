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

    [Header("角度制限")]
    [SerializeField, Tooltip("水平")]
    float _horizontalAngleLimit;
    [SerializeField, Tooltip("垂直")]
    float _verticalAngleLimit;

    //動作の基準となる情報を更新
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

        ////カーソルの座標
        //Vector3 cursorPos = _cursor.Img.transform.position;
        //Vector3 lookPos = cursorPos + (_initForward * _distance);
        //Vector3 direction = lookPos - _screenCenter;

        //_mainCamera.transform.forward += direction * _speed;

        Vector3 cursorPos = _cursor.Img.transform.localPosition * _speed;

        //角度補正
        Vector3 eulerAngle = new Vector3(-cursorPos.y, cursorPos.x); 
        eulerAngle.x = CorrectAngle2(eulerAngle.x, _verticalAngleLimit);
        eulerAngle.y = CorrectAngle2(eulerAngle.y, _horizontalAngleLimit);
        _mainCamera.transform.eulerAngles = transform.parent.eulerAngles + eulerAngle;

        //カメラリセット
        if (Input.GetKeyDown(KeyCode.R))
            ResetCamera();
    }

    //カメラリセット
    void ResetCamera()
    {
        _mainCamera.transform.forward = _initForward;
        _cursor.ResetPos();
    }

    //角度制限
    float CorrectAngle(in float targetAngle, in float limitAngle)
    {
        float oneLap = 360.0f;          //一周の角度
        float halfLap = oneLap * 0.5f;  //半周の角度

        //制限する角度
        float smallSideLimit = limitAngle;              //   0 ~ 半周の制限
        float largeSideLimit = oneLap - smallSideLimit; //半周 ~ 一周の制限

        //半周以下
        bool isSmallSide = targetAngle <= halfLap;

        //0 ~ 半周
        if (isSmallSide)
        {
            //制限
            if (targetAngle > smallSideLimit)
                return smallSideLimit;
        }
        //半周 ~ 一周
        else
        {
            //制限
            if (targetAngle < largeSideLimit)
                return largeSideLimit;
        }

        return targetAngle;
    }

    //角度制限
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