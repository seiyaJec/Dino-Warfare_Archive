#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

[CustomEditor(typeof(MoveStation))]
public class MoveStationEditor : Editor
{
    private MoveStation _moveStation;

    private void Awake()
    {
        _moveStation = target as MoveStation;
    }

    public override void OnInspectorGUI()
    {
        //Start
        EditorGUI.BeginChangeCheck();

        //順番設定
        EditorGUILayout.LabelField("Order：" + _moveStation.transform.GetSiblingIndex());

        //変更があったら実行
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_moveStation);
        }
    }

}
#endif