using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Naya_Tools
{
    public class Number_Renamer : EditorWindow
    {
        [MenuItem("GameObject/子を数字でリネームする", false, 0)]
        public static void rename_Right_Menu()
        {

            Transform selected = Selection.activeGameObject.transform;
            if (selected == null) return;
            for (int i = 0; i < selected.childCount; i++)
                selected.GetChild(i).name = i + "";
        }

        [MenuItem("GameObject/子を親の名前+数字でリネームする", false, 0)]
        public static void rename_parent_Right_Menu()
        {
            Transform selected = Selection.activeGameObject.transform;
            if (selected == null) return;
            for (int i = 0; i < selected.childCount; i++)
                selected.GetChild(i).name = selected.name + i;
        }

        [MenuItem("GameObject/Naya_Tools/Utility/指定名+数字でリネームする")]
        public static void init()
        {
            GetWindow<Number_Renamer>();

        }

        private string _name;

        private void OnGUI()
        {
            _name = EditorGUILayout.TextField("オブジェクト名", _name);

            if (GUILayout.Button("rename"))
            {
                Transform selected = Selection.activeGameObject.transform;
                if (selected == null) return;
                for (int i = 0; i < selected.childCount; i++)
                    selected.GetChild(i).name = _name + i;

            }
        }
    }
}