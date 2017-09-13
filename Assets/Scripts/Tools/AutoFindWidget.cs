using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//
//                  _ooOoo_
//                 o8888888o
//                 88' . '88
//                 (| -_- |)
//                 O\  =  /O
//              ____/`---'\____
//            .'  \\|     |//  `.
//          /  \\|||  :  |||//  \
//          /  _||||| -:- |||||-  \
//          |   | \\\  -  /// |   |
//          | \_|  ''\---/''  |   |
//          \  .-\__  `-`  ___/-. /
//        ___`. .'  /--.--\  `. . __
//     .'' '<  `.___\_<|>_/___.'  >'''.
//    | | :  `- \`.;`\ _ /`;.`/ - ` : | |
//    \  \ `-.   \_ __\ /__ _/   .-` /  /
//=====`-.____`-.___\_____/___.-`____.-'======
//                  `=---='
//
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
//          佛祖保佑       永无Bug
//          快加工资       不改需求
//

/// <summary>
/// 存储GameObject的类
/// </summary>
public class GameObjectDic
{
    private string name;
    /// <summary>
    /// gameobject的名字
    /// </summary>
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    private GameObject gameEntity;
    /// <summary>
    /// gameobject的实体
    /// </summary>
    public GameObject GameEntity
    {
        get { return gameEntity; }
        set { gameEntity = value; }
    }
}


public class AutoFindWidget : MonoBehaviour
{

    #region New

    private AutoFindWidget()
    {

    }

    /// <summary>
    /// 维护的游戏对象字典
    /// </summary>
    private List<GameObjectDic> _allGameDic;

    /// <summary>
    /// 存储传进来的物体对象
    /// </summary>
    public GameObject _gameObject { get; private set; }

    /// <summary>
    /// 存储传进来的Transform
    /// </summary>
    public Transform _transform { get; private set; }

    /// <summary>
    /// 实例化AutoFindWidget对象
    /// </summary>
    /// <param name="go">该物体对象</param>
    public static AutoFindWidget MyGet(GameObject go)
    {
        AutoFindWidget afw = go.GetComponent<AutoFindWidget>();
        if (afw == null) afw = go.AddComponent<AutoFindWidget>();
        afw._gameObject = go;
        afw._transform = go.transform;
        afw._allGameDic = new List<GameObjectDic>();
        afw.GetAllGame(afw);
        return afw;
    }

    /// <summary>
    /// 实例化AutoFindWidget对象
    /// </summary>
    /// <param name="tran"></param>
    /// <returns></returns>
    public static AutoFindWidget MyGet(Transform tran)
    {
        return MyGet(tran.gameObject);
    }

    /// <summary>
    /// 清空缓存
    /// </summary>
    /// <param name="afw">要清空的对象</param>
    public static void MyClearDic(AutoFindWidget afw)
    {
        afw._allGameDic.Clear();
    }

    /// <summary>
    /// 查找组件,请确保名字唯一
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="name">带有此组件物体的名字</param>
    /// <returns></returns>
    public T MyGetComponentByName<T>(string GameName) where T : Component
    {
        GameObject obj = null;
    BiaoQian: if (_allGameDic != null)
        {
            foreach (GameObjectDic item in _allGameDic)
            {
                if (item.Name == GameName)
                {
                    obj = item.GameEntity;
                    break;
                }
            }
        }
        else
        {
            _allGameDic.Clear();
            AllRecursion(this);
            goto BiaoQian;
        }
        if (obj != null)
        {
            return obj.GetComponent<T>();
        }
        else
        {
            Debug.Log("未找到！");
            return null;
        }
    }

    /// <summary>
    /// 获取gameObject对象
    /// </summary>
    /// <param name="GameName">物体名字</param>
    /// <returns></returns>
    public IEnumerable<GameObject> MyGetAppointGameObject(string GameName)
    {
        List<GameObject> list = new List<GameObject>();
    BiaoQian: if (_allGameDic != null)
        {
            foreach (GameObjectDic item in _allGameDic)
            {
                if (item.Name == GameName)
                {
                    GameObject obj = item.GameEntity;
                    list.Add(obj);
                }
            }
        }
        else
        {
            _allGameDic.Clear();
            AllRecursion(this);
            goto BiaoQian;
        }
        return list;
    }

    /// <summary>
    /// 获取gameObject对象（请确保名字唯一）
    /// </summary>
    /// <param name="GameName">物体名字</param>
    /// <returns></returns>
    public GameObject MyGetOneGameObject(string GameName)
    {
        GameObject obj = null;
    BiaoQian: if (_allGameDic != null)
        {
            foreach (GameObjectDic item in _allGameDic)
            {
                if (item.Name == GameName)
                {
                    obj = item.GameEntity;
                }
            }
        }
        else
        {
            _allGameDic.Clear();
            AllRecursion(this);
            goto BiaoQian;
        }
        return obj;
    }

    public List<GameObject> GetAllGame()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObjectDic item in _allGameDic)
        {
            list.Add(item.GameEntity);
        }
        return list;
    }

    /// <summary>
    /// 找到其下面所有gameobject
    /// </summary>
    /// <param name="auto">实例化的对象</param>
    /// <returns></returns>
    private void GetAllGame(AutoFindWidget auto)
    {
        AllRecursion(auto);
    }

    /// <summary>
    /// 总递归遍历
    /// </summary>
    /// <param name="auto">实例化的对象</param>
    private static void AllRecursion(AutoFindWidget auto)
    {
        if (auto._transform == null || auto._gameObject != null)
        {
            auto._transform = auto._gameObject.transform;
        }
        else
        {
            Debug.Log("参数错误！");
        }
        TranRecursion(auto, auto._transform);
    }

    /// <summary>
    /// Transfrom递归遍历
    /// </summary>
    /// <param name="auto">实例化的对象</param>
    private static void TranRecursion(AutoFindWidget auto, Transform tran)
    {
        int num = tran.childCount;
        for (int i = 0; i < num; i++)
        {
            auto._allGameDic.Add(new GameObjectDic()
            {
                Name = tran.GetChild(i).gameObject.name,
                GameEntity = tran.GetChild(i).gameObject
            });
            if (tran.GetChild(i).childCount > 0)
            {
                TranRecursion(auto, tran.GetChild(i));
            }
        }
    }
    #endregion

}
