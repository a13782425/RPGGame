using UnityEngine;
using System.Collections;
using RPGGame.Attr;
using UnityEditor;
using Object = UnityEngine.Object;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

[CanEditMultipleObjects]
[CustomEditor(typeof(Object), true)]
public class InspectorEditor : Editor
{

    private List<FieldInfo> fieldInfoList = new List<FieldInfo>();
    #region 自定义Inspector

    [MenuItem("Tools/Inspector/UseUnityInspector", priority = 2)]
    static void ToggleInspector()
    {
        bool toggle = GetToggle();
        EditorPrefs.SetBool("UseUnityInspector", !toggle);
    }
    [MenuItem("Tools/Inspector/UseUnityInspector", true)]
    static bool ToggleInspectorValidate()
    {
        Menu.SetChecked("Tools/Inspector/UseUnityInspector", GetToggle());
        return true;
    }
    static bool GetToggle()
    {
        bool toggle = false;
        if (EditorPrefs.HasKey("UseUnityInspector"))
        {
            toggle = EditorPrefs.GetBool("UseUnityInspector");
        }
        else
        {
            EditorPrefs.SetBool("UseUnityInspector", false);
        }
        return toggle;
    }

    #endregion

    #region 是否使用BOX包裹

    [MenuItem("Tools/Inspector/UseBoxInspector")]
    static void ToggleBoxInspector()
    {
        bool toggle = GetBoxToggle();
        EditorPrefs.SetBool("UseBoxInspector", !toggle);
    }
    [MenuItem("Tools/Inspector/UseBoxInspector", true)]
    static bool ToggleBoxInspectorValidate()
    {
        Menu.SetChecked("Tools/Inspector/UseBoxInspector", GetBoxToggle());
        return true;
    }
    static bool GetBoxToggle()
    {
        bool toggle = false;
        if (EditorPrefs.HasKey("UseBoxInspector"))
        {
            toggle = EditorPrefs.GetBool("UseBoxInspector");
        }
        else
        {
            EditorPrefs.SetBool("UseBoxInspector", false);
        }
        return toggle;
    }

    #endregion

    public override void OnInspectorGUI()
    {
        if (GetToggle())
        {
            base.OnInspectorGUI();
            return;
        }
        Type baseType = target.GetType();
        if (baseType == typeof(UnityEngine.Object))
        {
            base.OnInspectorGUI();
            return;
        }
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();
        fieldInfoList.Clear();
        GetField(baseType);
        using (new EditorGUI.DisabledScope(true))
        {
            SerializedProperty serializedProperty = serializedObject.FindProperty("m_Script");
            if (serializedProperty != null)
            {
                EditorGUILayout.PropertyField(serializedProperty, true, new GUILayoutOption[0]);
            }
        }
        if (fieldInfoList.Count < 1)
        {
            return;
        }
        if (GetBoxToggle())
        {
            EditorGUILayout.BeginVertical("box");
        }
        for (int i = 0; i < fieldInfoList.Count; i++)
        {
            FieldInfo info = fieldInfoList[i];
            object[] objs = info.GetCustomAttributes(typeof(BFieldAttribute), false);
            string showName = info.Name;
            if (objs.Length > 0)
            {
                showName = (objs[0] as BFieldAttribute).ShowName;
            }
            else
            {
                if (showName[0] == '_')
                {
                    showName = showName.Remove(0, 1);
                }
                else if (showName[0] == 'm')
                {
                    if (showName[1] == '_')
                    {
                        showName = showName.Remove(0, 2);
                    }
                }
                string head = showName[0].ToString().ToUpper();
                showName = showName.Insert(1, head);
                showName = showName.Remove(0, 1);
                char[] chs = showName.ToCharArray();
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < chs.Length; j++)
                {
                    if (chs[j] >= 65 && chs[j] <= 90)
                    {
                        if (j == 0)
                        {
                            goto End;
                        }
                        sb.Append(" ");
                    }
                End: sb.Append(chs[j]);
                }
                showName = sb.ToString();
            }
            SerializedProperty serializedProperty = serializedObject.FindProperty(info.Name);
            objs = info.GetCustomAttributes(typeof(SerializeField), false);
            if (info.IsPublic || objs.Length > 0)
            {
                if (serializedProperty != null)
                {
                    EditorGUILayout.PropertyField(serializedProperty, new GUIContent(showName), true, new GUILayoutOption[0]);
                }
            }
            else
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    if (!typeof(UnityEngine.Object).IsAssignableFrom(info.FieldType))
                    {
                        EditorGUILayout.TextField(showName, info.GetValue(target).ToString());
                    }
                    else
                    {
                        EditorGUILayout.ObjectField(new GUIContent(showName), (UnityEngine.Object)info.GetValue(target), info.FieldType, false);
                    }
                }
            }

        }
        if (GetBoxToggle())
        {
            EditorGUILayout.EndVertical();
        }
        serializedObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();
    }

    private void GetField(Type type)
    {
        if (type == null)
        {
            return;
        }
        if (type.BaseType != typeof(MonoBehaviour))
        {
            GetField(type.BaseType);
        }
        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            FieldInfo info = fieldInfos[i];
            if (info.DeclaringType == info.ReflectedType)
            {
                object[] objs = info.GetCustomAttributes(typeof(BFieldAttribute), false);
                if (objs.Length > 0)
                {
                    fieldInfoList.Add(info);
                    continue;
                }
                if (info.IsPublic)
                {
                    objs = info.GetCustomAttributes(typeof(HideInInspector), false);
                    if (objs.Length > 0)
                    {
                        continue;
                    }
                    else
                    {
                        fieldInfoList.Add(info);
                    }
                }
                else
                {
                    objs = info.GetCustomAttributes(typeof(SerializeField), false);
                    if (objs.Length > 0)
                    {
                        fieldInfoList.Add(info);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
    }
}
