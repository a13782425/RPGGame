﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPGGame.Utils
{
    public class ScoUtils
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

        public static GameObject GetMainPlayer()
        {
            return Global.GlobalData.CurrentPlayerController.gameObject;
        }
    }
}
