using RPGGame.Attr;
using RPGGame.Enums;
using RPGGame.Global;
using RPGGame.Manager;
using RPGGame.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmediateSceneConfig : MonoBehaviour
{
    [SerializeField, BField("要进入的场景")]
    private SceneEnum _loadScene = SceneEnum.MainScene;

    private bool _isInit = false;
    /// <summary>
    /// 是否初始化完毕
    /// </summary>
    public bool IsInit { get { return _isInit; } }

    void Start()
    {
        RPGGame.Global.GlobalData.SelectGame = "Default";
        //ZipUtils.UnzipFile(GlobalPath.StreamingAssetsPath + "/Default", Application.persistentDataPath + "/Default");
        ZipUtils.UnzipFile(GlobalPath.StreamingAssetsPath + "/GlobalData", Application.persistentDataPath);
        ArchivedUtils.Instance.LoadArchived();

        //#endif
        BeginGame(_loadScene);
    }
    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="loadScene"></param>
    public void BeginGame(SceneEnum loadScene)
    {
        Application.targetFrameRate = 60;
        this.StartCoroutine(BeginGameOperate(loadScene));
    }
    /// <summary>
    /// 开始游戏要执行的东西
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeginGameOperate(SceneEnum loadScene)
    {
        yield return null;
        GameManager.Instance.Load();
        yield return null;
        CameraManager.Instance.Load();
        yield return null;
        InputManager.Instance.Load();
        yield return null;
        TableManager.Instance.Load();
        yield return null;
        LevelManager.Instance.Load();
        yield return null;
        UIManager.Instance.Load();
        yield return null;
        MapManager.Instance.Load();
        yield return null;
        DeviceManager.Instance.Load();
        //yield return null;
        //MonsterManager.Instance.Load();
        yield return null;
        ScorpioManager.Instance.Load();
#if UNITY_EDITOR
        yield return null;
        QuickMemoryEditorManager.Instance.Load();
#endif
        yield return null;
        yield return null;
        yield return StartCoroutine(TableManager.Instance.LoadTables());
        StartCoroutine(LevelManager.Instance.SwitchToLevel(loadScene, null));
    }

    void Update()
    {
        if (IsInit)
        {
            GameManager.Instance.OnUpdate();
            CameraManager.Instance.OnUpdate();
            InputManager.Instance.OnUpdate();
            TableManager.Instance.OnUpdate();
            LevelManager.Instance.OnUpdate();
            UIManager.Instance.OnUpdate();
            MapManager.Instance.OnUpdate();
            DeviceManager.Instance.OnUpdate();
#if UNITY_EDITOR
            QuickMemoryEditorManager.Instance.OnUpdate();
#endif
        }
    }
}
