#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

[CustomEditor(typeof(PlayerMoveEvent))]
public class PlayerMoveEventEditor : Editor
{
    private PlayerMoveEvent _pEvent;

    private void Awake()
    {
        _pEvent = target as PlayerMoveEvent;
    }

    public override void OnInspectorGUI()
    {
        //Start
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.Space(10);

        //スピード設定
        _pEvent._moveEvent._setMove = EditorGUILayout.ToggleLeft("ChangeMoveSpeed", _pEvent._moveEvent._setMove, EditorStyles.boldLabel);
        if (_pEvent._moveEvent._setMove)
        {
            _pEvent._moveEvent._moveType = (PlayerMoveEvent.MoveEvent.MoveType)EditorGUILayout.EnumPopup("MoveType", _pEvent._moveEvent._moveType);

            EditorGUI.indentLevel++;
            //移動方法に応じてインスペクター変更
            switch (_pEvent._moveEvent._moveType)
            {
                //通常
                case PlayerMoveEvent.MoveEvent.MoveType.LEGACY:
                    _pEvent._moveEvent._playerMoveSpeed = EditorGUILayout.FloatField("speed", _pEvent._moveEvent._playerMoveSpeed);
                    break;
                //イージング
                case PlayerMoveEvent.MoveEvent.MoveType.EASE:
                    _pEvent._moveEvent._easeMoveData._easeType = (Ease)(EditorGUILayout.EnumPopup("EaseType", _pEvent._moveEvent._easeMoveData._easeType));
                    _pEvent._moveEvent._easeMoveData._MoveTime = EditorGUILayout.FloatField("MoveTime", _pEvent._moveEvent._easeMoveData._MoveTime);
                    break;

            }
            EditorGUI.indentLevel--;
        }

        //変更があったら実行
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_pEvent);
        }
    }

}
#endif