using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EulerRotate : MonoBehaviour
{
    [SerializeField]
    Text _text;

    [SerializeField]
    CursorMove _cursor;

    [SerializeField]
    float _rotateSpeed;

    [Header("角度制限")]
    [SerializeField, Tooltip("水平")]
    float _horizontalAngleLimit;
    [SerializeField, Tooltip("垂直")]
    float _verticalAngleLimit;

    private float _edgeWidthRate;
    private float _edgeHeightRate;

    ScreenRectDraw _screenRectDraw;

    Vector3 _screenCenter;
    Vector3 _initForward;
    Rect _screenEdge;

    enum Mode
    {
        EightDirection,//8方向
        AllDirection,  //全方向
    }
    [SerializeField]
    Mode _nowMode;

    // Start is called before the first frame update
    void Start()
    {
        _screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f);

        _screenRectDraw = gameObject.AddComponent<ScreenRectDraw>();

        _initForward = Camera.main.transform.forward;

        _screenEdge = new Rect();

        _edgeWidthRate  = 0.0f;
        _edgeHeightRate = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = "";

        _screenEdge.xMin = Screen.width * _edgeWidthRate;
        _screenEdge.xMax = Screen.width * (1.0f - _edgeWidthRate);
        _screenEdge.yMin = Screen.height * _edgeHeightRate;
        _screenEdge.yMax = Screen.height * (1.0f - _edgeHeightRate);

        //画面端矩形表示
        _screenRectDraw.SetEdge(
            _screenEdge.yMax,
            _screenEdge.yMin,
            _screenEdge.xMin,
            _screenEdge.xMax);

        _nowMode = UpdateMode(_nowMode);

        //方向を求める
        Vector2 direction = Vector2.zero;
        if (IsScreenEdge(_cursor.Img.transform.position))
        {
            switch (_nowMode)
            {
                case Mode.EightDirection:
                    direction = CalcDirection(_cursor.Img.transform.position);
                    break;

                case Mode.AllDirection:
                    direction = CalcDirection(_screenCenter, _cursor.Img.transform.position);
                    break;
            }
        }

        _text.text += "方向：" + direction + "\n";

        Vector2 velocity = Vector2.zero;
        {
            //固定値 (秒間〇度ずつ)
            velocity = direction * (_rotateSpeed * Time.deltaTime);
            
            //距離割合？
            //float maxSpeed = 80.0f;
            //Vector2 diff = _cursor.Img.transform.position - _screenCenter;
            //float x = Mathf.Clamp(diff.x, 0.0f, 1.0f);
            //float y = Mathf.Clamp(diff.y, 0.0f, 1.0f);
            
            //if (speed > maxSpeed)
            //    speed = maxSpeed;
            //velocity = direction * (speed * Time.deltaTime);

            _text.text += "magnitude：" + (_cursor.Img.transform.position - _screenCenter).magnitude + "\n";
            _text.text += "velocity：" + -velocity.y + "," + velocity.x + "\n";
        }

        Vector3 cameraRotate = Camera.main.transform.localEulerAngles;
        //加算 (座標から回転軸への変換も伴う)
        cameraRotate += new Vector3(-velocity.y, velocity.x);

        //角度制限
        //水平方向
        cameraRotate.y = CorrectAngle(cameraRotate.y, _horizontalAngleLimit);
        //垂直方向
        cameraRotate.x = CorrectAngle(cameraRotate.x, _verticalAngleLimit);

        //反映
        Camera.main.transform.localEulerAngles = cameraRotate;
        _text.text += "補正後：" + cameraRotate + "\n";

        //画面リセット
        if (Input.GetKeyDown(KeyCode.R))
            ResetCamera();
    }

    Mode UpdateMode(Mode now)
    {
        Mode result = now;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            result = Mode.EightDirection;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            result = Mode.AllDirection;

        return result;
    }

    void ResetCamera()
    {
        _cursor.ResetPos();
        Camera.main.transform.LookAt(_initForward);
    }

    bool IsScreenEdge(in Vector3 pos)
    {
        if (pos.x < _screenEdge.xMin)
            return true;
        if (pos.x > _screenEdge.xMax)
            return true;

        if (pos.y < _screenEdge.yMin)
            return true;
        if (pos.y > _screenEdge.yMax)
            return true;

        return false;
    }

    float CorrectAngle(in float targetAngle, in float limitAngle)
    {
        float oneLap = 360.0f;          //一周の角度
        float halfLap = oneLap * 0.5f;  //半周の角度

        //制限する角度
        float smallSideLimit = limitAngle;              //  0 ~ 180
        float largeSideLimit = oneLap - smallSideLimit; //181 ~ 360

        bool isSmallSide = targetAngle <= halfLap;

        if (isSmallSide)
        {
            if (targetAngle > smallSideLimit)
                return smallSideLimit;
        }
        else
        {
            if (targetAngle < largeSideLimit)
                return largeSideLimit;
        }

        return targetAngle;
    }

    //画面中央からカーソル座標への方向を返す
    Vector2 CalcDirection(in Vector3 startingPos, in Vector3 targetPos)
    {
        Vector2 direction = _cursor.Img.transform.position - _screenCenter;
        direction.Normalize();
        return direction;
    }

    //垂直、平行、45度の方向を返す
    Vector2 CalcDirection(in Vector3 targetPos)
    {
        Vector2 direction = Vector2.zero;

        if (targetPos.x < _screenEdge.xMin)
            direction.x = -1.0f;
        else if (targetPos.x > _screenEdge.xMax)
            direction.x = 1.0f;

        if (targetPos.y < _screenEdge.yMin)
            direction.y = -1.0f;
        else if (targetPos.y > _screenEdge.yMax)
            direction.y = 1.0f;

        if (direction.sqrMagnitude > 1.0f)
            direction.Normalize();

        return direction;
    }
}
