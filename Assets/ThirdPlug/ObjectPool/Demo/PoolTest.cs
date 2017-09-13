using UnityEngine;
using System.Collections;

public class PoolTest : MonoBehaviour
{

    //public GameObject obj;
    public GameObject obj1;
    public GameObject obj2;
    public Transform tran;
    BelongPool pool;
    private string objname = "Cube";
    // Use this for initialization
    void Start()
    {
        pool = BelongManager.Pools["abc"];
        BelongPrefabPool b1 = new BelongPrefabPool(obj1);
        BelongPrefabPool b2 = new BelongPrefabPool(obj2);
        b1.InitCount = 2;
        b2.InitCount = 2;
        //PoolManager pool = BelongManger.Pools["abc"];
        pool.CreatePrefabPool(b1);
        pool.CreatePrefabPool(b2);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnGUI()
    {
        objname = GUILayout.TextArea(objname);
        if (GUILayout.Button("加载"))
        {
            tran = pool.Extract(objname, new Vector3(10, 20), Quaternion.identity, this.transform);
        }
        if (GUILayout.Button("放回最后一个"))
        {
            pool.PutBack(tran);
            //tran = pool.Extract(objname, new Vector3(10, 20), Quaternion.identity, this.transform);
        }
        if (GUILayout.Button("全部放回"))
        {
            pool.PutBackAll();
        }
        if (GUILayout.Button("自动销毁（30S后看结果）"))
        {
            pool.IsAutoDestroy = true;
        }
        if (GUILayout.Button("手动销毁"))
        {
            foreach (var item in pool.PrefabPools)
            {
                if (item.Game.name=="Cube")
                {
                    item.Destroy = true;
                }
             
            }
        }
    }
}
