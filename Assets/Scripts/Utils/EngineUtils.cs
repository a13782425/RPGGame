using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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

namespace RPGGame.Utils
{
    public class EngineUtils
    {
        public static GameObject FindChild(Component com, string str)
        {
            if (com == null) return null;
            return FindChild(com.gameObject, str);
        }
        public static GameObject FindChild(GameObject go, string str)
        {
            if (go == null) return null;
            if (string.IsNullOrEmpty(str)) return go;
            Transform trans = go.transform.Find(str);
            if (trans == null) return null;
            return trans.gameObject;
        }
        public static object FindChild(Component com, string str, Type type)
        {
            if (com == null) return null;
            return FindChild(com.gameObject, str, type);
        }
        public static object FindChild(GameObject go, string str, Type type)
        {
            GameObject obj = FindChild(go, str);
            if (obj == null) return null;
            return obj.GetComponent(type);
        }
        public static Component GetComponent(Component com, Type type)
        {
            if (com == null) return null;
            return GetComponent(com.gameObject, type);
        }
        public static Component GetComponent(GameObject obj, Type type)
        {
            if (obj == null) return null;
            return obj.GetComponent(type);
        }
        public static GameObject GetGameObject(Component com)
        {
            return com == null ? null : com.gameObject;
        }
        public static GameObject GetGameObject(GameObject obj)
        {
            return obj;
        }
    }
}
