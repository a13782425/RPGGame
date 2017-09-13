using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池
/// </summary>
/// <typeparam name="T"></typeparam>
public class BelongPool : MonoBehaviour
{
    #region Unity自带方法

    private void Awake()
    {
        this.OneSelf = this.transform;
        if (!string.IsNullOrEmpty(this.PoolName))
        {
            BelongManager.Pools.Add(this);
            StartCoroutine(HandDestroy());
            //this.PoolName = this.OneSelf.name.Replace("Pool", "");
            //this.PoolName = this.PoolName.Replace("(Clone)", "");
        }
        if (this.GetComponent<BelongPoolManager>() == null)
        {
            this.gameObject.AddComponent<BelongPoolManager>();
        }
        //if (!BelongManager.IsOpenDestroy)
        //{
        //    BelongManager.IsOpenDestroy = true;
        //    StartCoroutine(BelongManager.AutoDestroy());
        //}
    }

    private void Update()
    {
        if (this.IsAutoDestroy && !this.IsBeginAutoDestroy)
        {
            this.IsBeginAutoDestroy = true;
            StartCoroutine(AutoDestroy());
        }
    }

    private void OnDestroy()
    {
        foreach (var item in _prefabPools)
        {
            item.Destroy = true;
        }
    }

    #endregion Unity自带方法

    #region 变量

    #region 公共

    /// <summary>
    /// 自身
    /// </summary>
    public Transform OneSelf { get; private set; }

    /// <summary>
    /// 是否自动清理
    /// </summary>
    public bool IsAutoDestroy
    {
        get
        {
            return _isAutoDestroy;
        }
        set
        {
            _isAutoDestroy = value;
            IsBeginAutoDestroy = false;
        }
    }

    /// <summary>
    /// 是否已经销毁
    /// </summary>
    public bool IsDestroyed
    {
        get
        {
            return _isDestroyed;
        }
    }

    public List<BelongPrefabPool> PrefabPools
    {
        get
        {
            return _prefabPools;
        }
    }

    /// <summary>
    /// 对象池名字
    /// </summary>
    public string PoolName
    {
        get
        {
            return _poolName;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            if (string.IsNullOrEmpty(_poolName))
            {
                _poolName = value;
                BelongManager.Pools.Add(this);
                StartCoroutine(HandDestroy());
            }

        }

    }

    /// <summary>
    /// 通过字典对对象池访问
    /// </summary>
    public BelongPrefabsDict DicBelongTrans
    {
        get
        {
            return _dic_prefabTrans;
        }
    }

    /// <summary>
    /// 清除及时，大于30自动销毁此脚本
    /// </summary>
    public int ClearCount { get; set; }

    #endregion


    #region 私有变量
    [SerializeField]
    private string _poolName = "";

    private bool _isDestroyed = false;

    private bool _isAutoDestroy = false;

    /// <summary>
    /// 是否开始自动清理
    /// </summary>
    private bool IsBeginAutoDestroy = false;

    private List<BelongPrefabPool> _prefabPools = new List<BelongPrefabPool>();

    private BelongPrefabsDict _dic_prefabTrans = new BelongPrefabsDict();

    /// <summary>
    /// 当前显示中的物体
    /// </summary>
    private List<Transform> _spawned = new List<Transform>();

    #endregion

    #endregion 变量

    #region 创建对象池

    /// <summary>
    /// 创建对象池
    /// </summary>
    /// <param name="useCoroution">是否使用协同</param>
    public void CreatePrefabPool(BelongPrefabPool prefab, bool useCoroution = false)
    {
        if (useCoroution)
        {
            StartCoroutine(CoroutionCreate(prefab));
        }
        else
        {
            prefab.PoolEntity = this;
            this._prefabPools.Add(prefab);
            string objName = string.IsNullOrEmpty(prefab.ShowName) ? prefab.Game.name : prefab.ShowName;
            this._dic_prefabTrans.Add(objName, prefab.Tran);
            prefab.PreLoadInstances();
        }
    }

    /// <summary>
    /// 创建对象池
    /// </summary>
    /// <param name="useCoroution">是否使用协同</param>
    public void CreatePrefabPool(IEnumerable<BelongPrefabPool> prefabList, bool useCoroution = false)
    {
        if (useCoroution)
        {
            StartCoroutine(CoroutionCreate(prefabList));
        }
        else
        {
            foreach (BelongPrefabPool item in prefabList)
            {
                CreatePrefabPool(item);
            }
        }
    }

    /// <summary>
    /// 协同创建对象池
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoroutionCreate(IEnumerable<BelongPrefabPool> prefabList)
    {
        foreach (BelongPrefabPool item in prefabList)
        {
            yield return StartCoroutine(CoroutionCreate(item));
        }
    }

    /// <summary>
    /// 协同创建对象池
    /// </summary>
    private IEnumerator CoroutionCreate(BelongPrefabPool prefab)
    {
        prefab.PoolEntity = this;
        this._prefabPools.Add(prefab);
        string objName = string.IsNullOrEmpty(prefab.ShowName) ? prefab.Game.name : prefab.ShowName;
        this._dic_prefabTrans.Add(objName, prefab.Tran);
        yield return StartCoroutine(prefab.CoroutionLoad());
    }

    #endregion 创建对象池

    #region 提取对象

    /// <summary>
    /// 提取对象
    /// </summary>
    /// <returns></returns>
    /// <param name="prefabName">对象名字</param>
    public Transform Extract(string prefabName)
    {
        Transform prefab = this._dic_prefabTrans[prefabName];
        return this.Extract(prefab);
    }

    /// <summary>
    /// 提取对象
    /// </summary>
    /// <returns></returns>
    /// <param name="prefabName">对象名字</param>
    /// <param name="parent">父亲</param>
    public Transform Extract(string prefabName, Transform parent)
    {
        Transform prefab = this._dic_prefabTrans[prefabName];
        return this.Extract(prefab, parent);
    }

    /// <summary>
    /// 提取对象
    /// </summary>
    /// <returns></returns>
    /// <param name="prefabName">对象名字</param>
    /// <param name="zero">坐标</param>
    public Transform Extract(string prefabName, Vector3 pos)
    {
        Transform prefab = this._dic_prefabTrans[prefabName];
        return this.Extract(prefab, pos);
    }

    /// <summary>
    /// 提取对象
    /// </summary>
    /// <returns></returns>
    /// <param name="prefabName">对象名字</param>
    /// <param name="zero">坐标</param>
    /// <param name="identity">旋转</param>
    public Transform Extract(string prefabName, Vector3 pos, Quaternion rot)
    {
        Transform prefab = this._dic_prefabTrans[prefabName];
        return this.Extract(prefab, pos, rot);
    }

    /// <summary>
    /// 提取对象
    /// </summary>
    /// <returns></returns>
    /// <param name="prefabName">对象名字</param>
    /// <param name="zero">坐标</param>
    /// <param name="identity">旋转</param>
    /// <param name="parent">父亲</param>
    public Transform Extract(string prefabName, Vector3 pos, Quaternion rot, Transform parent)
    {
        Transform prefab = this._dic_prefabTrans[prefabName];
        return this.Extract(prefab, pos, rot, parent);
    }

    /// <summary>
    /// 提取对象
    /// </summary>
    /// <returns></returns>
    /// <param name="prefab">对象</param>
    public Transform Extract(Transform prefab)
    {
        return this.Extract(prefab, Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// 提取对象
    /// </summary>
    /// <returns></returns>
    /// <param name="prefab">对象</param>
    /// <param name="parent">父亲</param>
    public Transform Extract(Transform prefab, Transform parent)
    {
        return this.Extract(prefab, Vector3.zero, Quaternion.identity, parent);
    }

    /// <summary>
    /// 提取对象
    /// </summary>
    /// <param name="prefab">对象</param>
    /// <param name="zero">坐标</param>
    /// <param name="identity">旋转</param>
    /// <returns></returns>
    public Transform Extract(Transform prefab, Vector3 pos)
    {
        Transform inst = this.Extract(prefab, pos, Quaternion.identity);
        if (inst == null) return null;
        return inst;
    }

    /// <summary>
    /// 提取对象
    /// </summary>
    /// <param name="prefab">对象</param>
    /// <param name="zero">坐标</param>
    /// <param name="identity">旋转</param>
    /// <returns></returns>
    public Transform Extract(Transform prefab, Vector3 pos, Quaternion rot)
    {
        Transform inst = this.Extract(prefab, pos, rot, null);
        if (inst == null) return null;
        return inst;
    }

    /// <summary>
    /// 提取对象
    /// </summary>
    /// <param name="prefab">对象</param>
    /// <param name="pos">坐标</param>
    /// <param name="rot">旋转</param>
    /// <param name="parent">父亲</param>
    /// <returns></returns>
    public Transform Extract(Transform prefab, Vector3 pos, Quaternion rot, Transform parent)
    {
        if (prefab == null)
        {
            return null;
        }
        Transform inst;
        for (int i = 0; i < this._prefabPools.Count; i++)
        {
            if (this._prefabPools[i].Game == prefab.gameObject)
            {
                inst = this._prefabPools[i].Instance(pos, rot);
                if (inst == null)
                {
                    return null;
                }
                if (parent != null)
                {
                    inst.parent = parent;
                }
                else if (inst.parent != this.OneSelf)
                {
                    inst.parent = this.OneSelf;
                }
                this._spawned.Add(inst);
                return inst;
            }
        }
        BelongPrefabPool newPrefabPool = new BelongPrefabPool(prefab);
        this.CreatePrefabPool(newPrefabPool);
        inst = newPrefabPool.Instance(pos, rot);
        if (parent != null)
        {
            inst.parent = parent;
        }
        else
        {
            inst.parent = this.OneSelf;
        }
        this._spawned.Add(inst);
        return inst;
    }

    #endregion 提取对象

    #region 放回池子

    /// <summary>
    /// 全部放回池子
    /// </summary>
    public void PutBackAll()
    {
        List<Transform> spawned = new List<Transform>(this._spawned);
        for (int i = 0; i < spawned.Count; i++)
            this.PutBack(spawned[i]);
    }

    /// <summary>
    /// 将某个对象放回池子
    /// </summary>
    /// <param name="instance">取出的对象</param>
    public void PutBack(Transform instance)
    {
        for (int i = 0; i < this._prefabPools.Count; i++)
        {
            if (this._prefabPools[i].Spawned.Contains(instance))
            {
                this._prefabPools[i].IsInstance(instance, false);
                this._spawned.Remove(instance);
                break;
            }
            else if (this._prefabPools[i].DeSpawned.Contains(instance))
            {
                Debug.LogError(
                    string.Format("对象池 {0}中的 {1}物体已经没有使用!",
                                    this.PoolName,
                                    instance.name));
                return;
            }
        }
        
    }

    #endregion 放回池子

    #region 对象是否在使用中

    /// <summary>
    /// 判断对象是否在场景中使用
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public bool IsUse(Transform instance)
    {
        return this._spawned.Contains(instance);
    }

    /// <summary>
    /// 判断初始化物体有没有清空
    /// </summary>
    /// <param name="init"></param>
    /// <returns></returns>
    public bool IsInitUse(Transform init)
    {
        foreach (BelongPrefabPool item in _prefabPools)
        {
            if (item.Tran.Equals(init))
            {
                return true;
            }
        }
        return false;

    }


    #endregion 对象是否在使用中

    #region 销毁
    /// <summary>
    /// 销毁
    /// </summary>
    public void Destroy()
    {
        foreach (var item in _prefabPools)
        {
            item.Destroy = true;
        }
    }
    #endregion

    #region 自动清理

    private IEnumerator AutoDestroy()
    {
        while (IsBeginAutoDestroy)
        {
            BelongPrefabPool pre = null;
            foreach (BelongPrefabPool item in this._prefabPools)
            {
                if (item.TotalCount == item.DeSpawned.Count)
                {
                    item.ClearCount++;
                }
                else
                {
                    item.ClearCount = 0;
                }
                if (item.ClearCount >= 30)
                {
                    item.Destroy = true;
                    item.ClearCount = 0;
                }
            }
            if (pre != null)
            {
                string objName = string.IsNullOrEmpty(pre.ShowName) ? pre.Game.name : pre.ShowName;
                this._dic_prefabTrans.Remove(objName);
                this._prefabPools.Remove(pre);
                pre = null;
            }
            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// 手动清理
    /// </summary>
    /// <param name="pool"></param>
    /// <returns></returns>
    private IEnumerator HandDestroy()
    {
        while (true)
        {
            BelongPrefabPool pre = null;
            foreach (BelongPrefabPool item in this._prefabPools)
            {
                if (item.Destroy && !item.Destroying)
                {
                    yield return StartCoroutine(item.Clear());
                }
                if (item.Destroyed)
                {
                    pre = item;
                }
            }
            if (pre != null)
            {
                string objName = string.IsNullOrEmpty(pre.ShowName) ? pre.Game.name : pre.ShowName;
                this._dic_prefabTrans.Remove(objName);
                this._prefabPools.Remove(pre);
                if (pre.Game.activeInHierarchy)
                {
                    Destroy(pre.Game);
                }
                pre = null;
            }
            yield return new WaitForSeconds(1);
            if (this._prefabPools == null || this._prefabPools.Count < 1)
            {
                this._isDestroyed = true;
            }
        }
    }

    //public void HandDestroy(BelongPrefabPool pool)
    //{
    //    this._dic_prefabTrans.Remove(pool.Game.name);
    //    this._prefabPools.Remove(pool);
    //    pool = null;
    //}

    #endregion 自动清理
}