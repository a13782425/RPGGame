using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using System.IO;

public class SceneEditor : EditorWindow
{

    private static EditorWindow _currentWindow;
    private Vector3 _scroll;
    private static List<string> _scenePath = new List<string>();
    private static string _path = null;
    [MenuItem("Tools/Scene &5", priority = 3)]
    static void OpenScene()
    {
        _scenePath.Clear();
        _currentWindow = EditorWindow.GetWindow(typeof(SceneEditor), false, "场景");
        _currentWindow.position = new Rect(100, 100, 200, 200);
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        _path = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/"));
        for (int i = 0; i < scenes.Length; i++)
        {
            _scenePath.Add(Path.Combine(_path, scenes[i].path));
        }
    }

    void Update()
    {
        if (EditorBuildSettings.scenes.Length != _scenePath.Count)
        {
            _scenePath.Clear();
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            for (int i = 0; i < scenes.Length; i++)
            {
                _scenePath.Add(Path.Combine(_path, scenes[i].path));
            }
        }
    }

    void OnGUI()
    {
        Rect windowRect = new Rect(20, 20, position.width - 20, position.height - 40);
        GUILayout.BeginArea(windowRect);
        _scroll = GUILayout.BeginScrollView(_scroll, false, true);
        SetBuildScene();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private void SetBuildScene()
    {
        for (int i = 0; i < _scenePath.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(Path.GetFileNameWithoutExtension(_scenePath[i]));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("打开"))
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(_scenePath[i], OpenSceneMode.Single);
                this.Close();
            }
            GUILayout.EndHorizontal();
        }
    }

}
