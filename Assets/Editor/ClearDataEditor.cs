using UnityEngine;
using System.Collections;
using UnityEditor;

public class ClearDataEditor : Editor
{
    [MenuItem("Tools/ClearData", priority = 4)]
    public static void ClearData()
    {
        Debug.LogError("删除PlayerPrefs");
        PlayerPrefs.DeleteAll();
    }
}
