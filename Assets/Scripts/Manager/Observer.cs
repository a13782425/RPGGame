using UnityEngine;
using System.Collections;
using RPGGame.Manager;
using RPGGame.Enums;
using RPGGame.Table;
using RPGGame.Model.DTO;

public class Observer : MonoBehaviour
{

    #region 单例

    private static Observer _instance;

    public static Observer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("Observer").AddComponent<Observer>();
                _instance.gameObject.AddComponent<DontDestroyGameObject>();
            }
            return _instance;
        }
    }

    #endregion

    private bool _isInit = false;
    /// <summary>
    /// 是否初始化完毕
    /// </summary>
    public bool IsInit { get { return _isInit; } }

    private bool _isUnLoad = false;
    /// <summary>
    /// 是否已经卸载
    /// </summary>
    public bool IsUnLoad { get { return _isUnLoad; } set { _isUnLoad = value; } }

    #region UnityMethod

    // Update is called once per frame
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

    private void OnDestroy()
    {
        if (!_isUnLoad)
        {
            _isUnLoad = true;
            GameManager.Instance.UnLoad();
            CameraManager.Instance.UnLoad();
            InputManager.Instance.UnLoad();
            TableManager.Instance.UnLoad();
            LevelManager.Instance.UnLoad();
            UIManager.Instance.UnLoad();
            MapManager.Instance.UnLoad();
            DeviceManager.Instance.UnLoad();
#if UNITY_EDITOR
            QuickMemoryEditorManager.Instance.UnLoad();
#endif
        }
    }

    private void OnApplicationQuit()
    {
        if (!_isUnLoad)
        {
            _isUnLoad = true;
            GameManager.Instance.UnLoad();
            CameraManager.Instance.UnLoad();
            InputManager.Instance.UnLoad();
            TableManager.Instance.UnLoad();
            LevelManager.Instance.UnLoad();
            UIManager.Instance.UnLoad();
            MapManager.Instance.UnLoad();
            DeviceManager.Instance.UnLoad();
#if UNITY_EDITOR
            QuickMemoryEditorManager.Instance.UnLoad();
#endif
        }
    }

    #endregion

    #region 公共方法

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="loadScene"></param>
    public void BeginGame(SceneEnum loadScene)
    {
        Application.targetFrameRate = 60;
        this.StartCoroutine(BeginGameOperate(loadScene));
    }

    #endregion

    #region 私有方法

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
        //yield return null;
        //ScorpioManager.Instance.Load();
#if UNITY_EDITOR
        yield return null;
        QuickMemoryEditorManager.Instance.Load();
#endif
        yield return null;
        _isInit = true;
        yield return null;
        StartCoroutine(LevelManager.Instance.SwitchToLevel(loadScene, null));
    }

    #endregion

}
