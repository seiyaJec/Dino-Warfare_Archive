#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using System;

[CustomEditor(typeof(SetTestView))]
public class SetTestViewEditor : Editor
{
    private SetTestView _tv;
    private Camera _cameraIns;
    private Camera _camera
    {
        get
        {
            if (_cameraIns == null)
            {
                var cameraobj = GameObject.FindGameObjectWithTag("TestCamera");
                if (cameraobj == null)
                {
                    Debug.Log("�@�\���g���ɂ�TestCamera�v���n�u���V�[����ɓ���Ă�������");
                }
                else
                {
                    _cameraIns = cameraobj.GetComponent<Camera>();
                }
            }
            return _cameraIns;
        }
    }

    //
    private void Awake()
    {
        _tv = target as SetTestView;
    }

    public override void OnInspectorGUI()
    {
        //Start
        EditorGUI.BeginChangeCheck();
        _tv.CallViewCamera = EditorGUILayout.ToggleLeft("CallViewCamera", _tv.CallViewCamera);

        _tv.prot.yaw = EditorGUILayout.FloatField("yaw", _tv.prot.yaw);
        _tv.prot.pitch = EditorGUILayout.FloatField("pitch", _tv.prot.pitch);
        _tv.prot.roll = EditorGUILayout.FloatField("roll", _tv.prot.roll);

        _tv.prot.ChangeFov = EditorGUILayout.ToggleLeft("ChangeFov", _tv.prot.ChangeFov);
        if (_tv.prot.ChangeFov)
        {
            _tv.prot.FOV = EditorGUILayout.FloatField("FOV", _tv.prot.FOV);
        }

        if (_tv.CallViewCamera)
        {
            if (_camera != null)
            {
                _camera.transform.position = _tv.transform.position;
                _camera.transform.localRotation = Quaternion.Euler(_tv.prot.pitch, _tv.prot.yaw, _tv.prot.roll);
                if (_tv.prot.ChangeFov)
                {
                    _camera.fieldOfView = _tv.prot.FOV;
                }
                Debug.Log("�e�X�g�p�J�����Ăяo������");
                _tv.CallViewCamera = false;
            }
        }

        EditorGUILayout.Space(30);

        EditorGUILayout.LabelField("��]�����Ƃ��̌����A������m�F���邽�߂̃X�N���v�g�ł�");
        EditorGUILayout.LabelField("�V�[�����TestCamera�v���n�u��ǉ�����ƁA");
        EditorGUILayout.LabelField("display8�Ƀe�X�g�p�̉�ʂ��f��܂�");


        //�ύX������������s
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_tv);
        }
    }

}
#endif