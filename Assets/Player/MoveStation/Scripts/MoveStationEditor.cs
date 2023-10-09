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

        //èáî‘ê›íË
        EditorGUILayout.LabelField("OrderÅF" + _moveStation.transform.GetSiblingIndex());

        //ïœçXÇ™Ç†Ç¡ÇΩÇÁé¿çs
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_moveStation);
        }
    }

}
#endif