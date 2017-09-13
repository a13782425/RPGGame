using UnityEngine;
using System.Collections;
using RPGGame.Manager;
using UnityEngine.SceneManagement;
using RPGGame.Enums;
using RPGGame.Global;
using RPGGame.Utils;

public class GameStart : MonoBehaviour
{

    [SerializeField]
    private SceneEnum _loadScene = SceneEnum.MainScene;
    //[SerializeField]
    //private float _waitTime = 1f;

    //float beginTime;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == _loadScene.ToString())
        {
            Debug.LogError("加载场景相同，请重新设置！");
            return;
        }
	    Debug.LogError(Application.persistentDataPath + "/Default");
	    //#if UNITY_EDITOR
	    ZipUtils.UnzipFile(GlobalPath.StreamingAssetsPath + "/Default", Application.persistentDataPath + "/Default");
	    //#endif
        Observer.Instance.BeginGame(_loadScene);

        //StartCoroutine(BeginGame());
    }

    //private IEnumerator BeginGame()
    //{
    //    yield return StartCoroutine(GameManager.Instance.InitGame());
    //    yield return null;
    //    StartCoroutine(LevelManager.Instance.SwitchToLevel(_loadScene, null));
    //}
}
