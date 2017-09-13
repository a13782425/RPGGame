using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 对象类

public class BelongPrefabPool
{

    #region 构造函数

    public BelongPrefabPool(GameObject obj)
    {
        Game = obj;
        Tran = obj.transform;
    }

    public BelongPrefabPool(Transform tran)
    {
        Game = tran.gameObject;
        Tran = tran;
    }

    private BelongPrefabPool()
    {
    }

    #endregion

    #region 参数

    public string ShowName = string.Empty;

    /// <summary>
    /// 对象池控制器
    /// </summary>
    public BelongPool PoolEntity { get; set; }

    private bool _destroy = false;

    /// <summary>
    /// 是否清理，优先权大于自动清理
    /// </summary>
    public bool Destroy
    {
        get
        {
            return _destroy;
        }
        set
        {
            _destroy = value;
        }
    }

    private bool _destroying = false;

    /// <summary>
    /// 是否正在清理
    /// </summary>
    public bool Destroying
    {
        get
        {
            return _destroying;
        }
    }

    private bool _destroyed = false;

    /// <summary>
    /// 清理完成
    /// </summary>
    public bool Destroyed
    {
        get
        {
            return _destroyed;
        }
    }

    public int ClearCount = 0;

    private int _maxCount = 5;

    /// <summary>
    /// 对象池中最大个数
    /// </summary>
    public int MaxCount
    {
        get
        {
            return _maxCount;
        }
        set
        {
            _maxCount = value;
        }
    }

    private int _initCount = 0;

    /// <summary>
    /// 初始化个数
    /// </summary>
    public int InitCount
    {
        get
        {
            return _initCount;
        }
        set
        {
            _initCount = value;
        }
    }

    /// <summary>
    /// 此物体
    /// </summary>
    public Transform Tran;

    /// <summary>
    /// 此物体
    /// </summary>
    public GameObject Game;

    /// <summary>
    /// 使用中的对象池
    /// </summary>
    private List<Transform> _spawned = new List<Transform>();

    /// <summary>
    /// 使用中的对象池
    /// </summary>
    public List<Transform> Spawned
    {
        get
        {
            return new List<Transform>(this._spawned);
        }
    }

    /// <summary>
    /// 待使用的对象池
    /// </summary>
    private List<Transform> _despawned = new List<Transform>();

    /// <summary>
    /// 待使用的对象池
    /// </summary>
    public List<Transform> DeSpawned
    {
        get
        {
            return new List<Transform>(this._despawned);
        }
    }

    /// <summary>
    /// 共有多少个预制物体
    /// </summary>
    public int TotalCount
    {
        get
        {
            int count = 0;
            count += this._spawned.Count;
            count += this._despawned.Count;
            return count;
        }
    }

    #endregion 参数

    /// <summary>
    /// 初始化
    /// </summary>
    public void PreLoadInstances()
    {
        if (Tran == null)
        {
            throw new System.NullReferenceException("对象引用为Null,请检查是否设置了GameObject---->BelongPool");
            //Debug.LogError("对象引用为Null,请检查是否设置了GameObject---->BelongPool");
        }
        Transform tran;

        while (this.TotalCount < this._initCount)
        {
            tran = this.New();
            this.IsInstance(tran, false);
        }
    }

    public IEnumerator CoroutionLoad()
    {
        if (Tran == null)
        {
            throw new System.NullReferenceException("对象引用为Null,请检查是否设置了GameObject---->BelongPool");
            //Debug.LogError("对象引用为Null,请检查是否设置了GameObject---->BelongPool");
        }
        Transform tran;

        while (this.TotalCount < this._initCount)
        {
            tran = this.New();
            this.IsInstance(tran, false);
            yield return 1;
        }
    }


    /// <summary>
    /// 设置是否显示
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="isShow"></param>
    public void IsInstance(Transform tran, bool isShow)
    {
        if (isShow)
        {
            this._spawned.Add(tran);
            this._despawned.Remove(tran);
            tran.gameObject.SetActive(isShow);
        }
        else
        {
            this._spawned.Remove(tran);
            this._despawned.Add(tran);
            tran.gameObject.SetActive(isShow);
        }
    }

    public Transform New()
    {
        return this.New(Vector3.zero, Quaternion.identity);
    }

    public Transform New(Vector3 pos, Quaternion rot)
    {
        if (pos == Vector3.zero) pos = this.PoolEntity.transform.position;
        if (rot == Quaternion.identity) rot = this.PoolEntity.transform.rotation;
        Transform inst = (Transform)UnityEngine.Object.Instantiate(this.Tran, pos, rot);
        this.NameInstance(inst);
        inst.SetParent(this.PoolEntity.transform);
        //inst.parent = this.PoolEntity.transform;
        inst.localScale = Vector3.one;
        this.SetLayer(inst, this.PoolEntity.gameObject.layer);
        this._spawned.Add(inst);

        return inst;
    }

    /// <summary>
    /// 提取物体
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <returns></returns>
    internal Transform Instance(Vector3 pos, Quaternion rot)
    {
        if (this.Spawned.Count >= this.MaxCount)
        {
            Transform firstIn = this._spawned[0];
            return firstIn;
        }
        Transform inst;

        // If nothing is available, create a new instance
        if (this._despawned.Count == 0)
        {
            // This will also handle limiting the number of NEW instances
            inst = this.New(pos, rot);
        }
        else
        {
            // Switch the instance we are using to the spawned list
            // Use the first item in the list for ease
            inst = this._despawned[0];
            this._despawned.RemoveAt(0);
            this._spawned.Add(inst);

            // This came up for a user so this was added to throw a user-friendly error
            if (inst == null)
            {
                var msg = "Make sure you didn't delete a despawned instance directly.";
                throw new MissingReferenceException(msg);
            }

            inst.position = pos;
            inst.rotation = rot;
            BelongManagerUtils.SetActive(inst.gameObject, true);
        }

        //
        // NOTE: OnSpawned message broadcast was moved to main Spawn() to ensure it runs last
        //

        return inst;
    }

    /// <summary>
    /// 设置层
    /// </summary>
    /// <param name="inst"></param>
    /// <param name="layer"></param>
    private void SetLayer(Transform inst, int layer)
    {
        inst.gameObject.layer = layer;
        foreach (Transform child in inst)
            SetLayer(child, layer);
    }

    /// <summary>
    /// 起名字
    /// </summary>
    /// <param name="inst"></param>
    private void NameInstance(Transform inst)
    {
        inst.name += (this.TotalCount + 1).ToString("#000");
    }

    public IEnumerator Clear()
    {
        this._destroying = true;
        foreach (Transform item in this._despawned)
        {
            UnityEngine.Object.Destroy(item.gameObject);
            yield return 1;
        }
        foreach (var item in this._spawned)
        {
            UnityEngine.Object.Destroy(item.gameObject);
            yield return 1;
        }
        this._destroyed = true;
    }

    #region 重载运算符

    public static bool operator ==(BelongPrefabPool belong, string name)
    {
        //if (belong != null && !string.IsNullOrEmpty(name))
        //{
        if (ReferenceEquals(belong, null) && ReferenceEquals(name, null))
        {
            return true;
        }
        else if (!(ReferenceEquals(belong, null)) && !(ReferenceEquals(name, null)))
        {
            string objName = string.IsNullOrEmpty(belong.ShowName) ? belong.Game.name : belong.ShowName;
            return objName == name;
        }
        return false;
    }

    public static bool operator !=(BelongPrefabPool belong, string name)
    {
        return !(belong == name);
    }

    #endregion 重载运算符
}

#endregion 对象类

#region 管理BelongPool字典

/// <summary>
/// 对象池字典
/// </summary>
public class PoolManagerDic : IDictionary<string, BelongPool>
{
    private Dictionary<string, BelongPool> _pools = new Dictionary<string, BelongPool>();

    public BelongPool this[string key]
    {
        get
        {
            BelongPool pool;
            try
            {
                pool = this._pools[key];
            }
            catch (KeyNotFoundException)
            {
                string msg = string.Format("A Pool with the name '{0}' not found. " +
                                            "\nPools={1}",
                                            key, this.ToString());
                throw new KeyNotFoundException(msg);
            }

            return pool;
        }
        set
        {
            throw new System.Exception("不可以自行设置");
        }
    }

    public int Count
    {
        get
        {
            return this._pools.Count;
        }
    }

    public bool IsReadOnly
    {
        get
        {
            return true;
        }
    }

    public ICollection<string> Keys
    {
        get
        {
            string[] keysArray = new string[this._pools.Count];
            this._pools.Keys.CopyTo(keysArray, 0);

            // Return a comma-sperated list inside square brackets (Pythonesque)
            return keysArray;
        }
    }

    public ICollection<BelongPool> Values
    {
        get
        {
            BelongPool[] valueArray = new BelongPool[this._pools.Count];
            this._pools.Values.CopyTo(valueArray, 0);
            return valueArray;
        }
    }

    public void Add(KeyValuePair<string, BelongPool> item)
    {
        throw new NotImplementedException();
    }

    internal void Add(BelongPool pool)
    {
        // Don't let two pools with the same name be added. See error below for details
        if (this.ContainsKey(pool.PoolName))
        {
            Debug.LogError(string.Format("这个名为'{0}'的对象池已经存在！",
                                         pool.PoolName));
            return;
        }

        this._pools.Add(pool.PoolName, pool);
    }

    public void Add(string key, BelongPool value)
    {
        throw new NotImplementedException("请使用PoolManager.Pools[\"\"]进行创建");
    }

    public void Clear()
    {
        foreach (KeyValuePair<string, BelongPool> pair in this._pools)
            UnityEngine.Object.Destroy(pair.Value);

        // Clear the dict in case the user re-creates a SpawnPool of the same name later
        this._pools.Clear();
    }

    public bool Contains(KeyValuePair<string, BelongPool> item)
    {
        throw new System.NotImplementedException("请使用PoolManager.Pools.Contains(string poolName)");
    }

    public bool ContainsKey(string key)
    {
        return this._pools.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<string, BelongPool>[] array, int arrayIndex)
    {
        throw new NotImplementedException("不需要拷贝");
    }

    public IEnumerator<KeyValuePair<string, BelongPool>> GetEnumerator()
    {
        return this._pools.GetEnumerator();
    }

    public bool Remove(KeyValuePair<string, BelongPool> item)
    {
        return Remove(item.Key);// throw new NotImplementedException();
    }

    public bool Remove(string key)
    {
        return _pools.Remove(key);
    }

    public bool TryGetValue(string key, out BelongPool value)
    {
        return this._pools.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this._pools.GetEnumerator();
    }
}

#endregion 管理BelongPool字典

#region 管理Transform的字典

public class BelongPrefabsDict : IDictionary<string, Transform>
{
    private Dictionary<string, Transform> _prefabs = new Dictionary<string, Transform>();

    public Transform this[string key]
    {
        get
        {
            Transform prefab = null;
            try
            {
                prefab = this._prefabs[key];
            }
            catch (KeyNotFoundException)
            {
                Debug.LogError("不存在这个Key");
            }

            return prefab;
        }

        set
        {
            Debug.LogError("这是只读的");
        }
    }

    public int Count
    {
        get
        {
            return _prefabs.Count;
        }
    }

    public bool IsReadOnly
    {
        get
        {
            return true;
        }
    }

    public ICollection<string> Keys
    {
        get
        {
            return _prefabs.Keys;
        }
    }

    public ICollection<Transform> Values
    {
        get
        {
            return _prefabs.Values;
        }
    }

    public void Add(KeyValuePair<string, Transform> item)
    {
        Debug.LogError("不能添加");
    }

    public void Add(string key, Transform value)
    {
        this._prefabs.Add(key, value);
    }

    public void Clear()
    {
        Debug.LogError("请不要自行清理");
    }

    public bool Contains(KeyValuePair<string, Transform> item)
    {
        Debug.LogError("请不要自行操作");
        return false;
    }

    public bool ContainsKey(string key)
    {
        return this._prefabs.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<string, Transform>[] array, int arrayIndex)
    {
        Debug.LogError("请不要自行操作");
    }

    public IEnumerator<KeyValuePair<string, Transform>> GetEnumerator()
    {
        return this._prefabs.GetEnumerator();
    }

    public bool Remove(KeyValuePair<string, Transform> item)
    {
        Debug.LogError("请不要自行操作");
        return false;// this._prefabs.Remove(item.Key);
    }

    public bool Remove(string key)
    {
        return this._prefabs.Remove(key);
    }

    public bool TryGetValue(string key, out Transform value)
    {
        value = null;
        Debug.LogError("请不要自行操作");
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this._prefabs.GetEnumerator();
    }
}

#endregion 管理Transform的字典

#region BelongPool真实管理者

/// <summary>
/// 对象池管理器
/// </summary>
public static class BelongManager
{
    public static readonly PoolManagerDic Pools = new PoolManagerDic();
}

/// <summary>
/// 控制BelongPool销毁
/// </summary>
public class BelongPoolManager : MonoBehaviour
{
    private bool des = true;
    void Awake()
    {
        StartCoroutine(AutoDestroy());
    }
    /// <summary>
    /// 清除BelongPool 减少冗余
    /// </summary>
    /// <returns></returns>
    IEnumerator AutoDestroy()
    {
        List<string> list = new List<string>();
        while (des)
        {
            foreach (KeyValuePair<string, BelongPool> item in BelongManager.Pools)
            {
                if (item.Value.PrefabPools == null || item.Value.PrefabPools.Count < 1)
                {
                    item.Value.ClearCount++;
                }
                else
                {
                    item.Value.ClearCount = 0;
                }
                if (item.Value.ClearCount > 30)
                {
                    list.Add(item.Key);
                }
            }
            foreach (string item in list)
            {
                UnityEngine.Object.Destroy(BelongManager.Pools[item]);
                BelongManager.Pools.Remove(item);
            }
            list.Clear();
            yield return new WaitForSeconds(1f);
        }
    }

    public static void HandDestroy()
    {
        ICollection<string> coll = BelongManager.Pools.Keys;
        foreach (string item in coll)
        {
            UnityEngine.Object.Destroy(BelongManager.Pools[item]);
            BelongManager.Pools.Remove(item);
        }
    }

    void OnDestroy()
    {
        des = false;
        StopCoroutine(AutoDestroy());
    }
}

public static class BelongManagerUtils
{
    /// <summary>
    /// 设置这个物体的显示状态
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="state"></param>
    internal static void SetActive(GameObject obj, bool state)
    {
        obj.SetActive(state);
    }

    /// <summary>
    /// 这个物体是否在场景中
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    internal static bool activeInHierarchy(GameObject obj)
    {
        return obj.activeInHierarchy;
    }
}

#endregion BelongPool真实管理者