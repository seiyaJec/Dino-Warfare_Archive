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

    [Header("�p�x����")]
    [SerializeField, Tooltip("����")]
    float _horizontalAngleLimit;
    [SerializeField, Tooltip("����")]
    float _verticalAngleLimit;

    private float _edgeWidthRate;
    private float _edgeHeightRate;

    ScreenRectDraw _screenRectDraw;

    Vector3 _screenCenter;
    Vector3 _initForward;
    Rect _screenEdge;

    enum Mode
    {
        EightDirection,//8����
        AllDirection,  //�S����
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

        //��ʒ[��`�\��
        _screenRectDraw.SetEdge(
            _screenEdge.yMax,
            _screenEdge.yMin,
            _screenEdge.xMin,
            _screenEdge.xMax);

        _nowMode = UpdateMode(_nowMode);

        //���������߂�
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

        _text.text += "�����F" + direction + "\n";

        Vector2 velocity = Vector2.zero;
        {
            //�Œ�l (�b�ԁZ�x����)
            velocity = direction * (_rotateSpeed * Time.deltaTime);
            
            //���������H
            //float maxSpeed = 80.0f;
            //Vector2 diff = _cursor.Img.transform.position - _screenCenter;
            //float x = Mathf.Clamp(diff.x, 0.0f, 1.0f);
            //float y = Mathf.Clamp(diff.y, 0.0f, 1.0f);
            
            //if (speed > maxSpeed)
            //    speed = maxSpeed;
            //velocity = direction * (speed * Time.deltaTime);

            _text.text += "magnitude�F" + (_cursor.Img.transform.position - _screenCenter).magnitude + "\n";
            _text.text += "velocity�F" + -velocity.y + "," + velocity.x + "\n";
        }

        Vector3 cameraRotate = Camera.main.transform.localEulerAngles;
        //���Z (���W�����]���ւ̕ϊ�������)
        cameraRotate += new Vector3(-velocity.y, velocity.x);

        //�p�x����
        //��������
        cameraRotate.y = CorrectAngle(cameraRotate.y, _horizontalAngleLimit);
        //��������
        cameraRotate.x = CorrectAngle(cameraRotate.x, _verticalAngleLimit);

        //���f
        Camera.main.transform.localEulerAngles = cameraRotate;
        _text.text += "�␳��F" + cameraRotate + "\n";

        //��ʃ��Z�b�g
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
        float oneLap = 360.0f;          //����̊p�x
        float halfLap = oneLap * 0.5f;  //�����̊p�x

        //��������p�x
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

    //��ʒ�������J�[�\�����W�ւ̕�����Ԃ�
    Vector2 CalcDirection(in Vector3 startingPos, in Vector3 targetPos)
    {
        Vector2 direction = _cursor.Img.transform.position - _screenCenter;
        direction.Normalize();
        return direction;
    }

    //�����A���s�A45�x�̕�����Ԃ�
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
