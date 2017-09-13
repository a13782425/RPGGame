using RPGGame.Utils;
using Scorpio;
using Scorpio.Userdata;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Manager
{
    public class ScorpioManager : RPGGame.Tools.Singleton<ScorpioManager>, IManager
    {
        private Script _scriptEngine = null;         //脚本引擎
        /// <summary>
        /// 游戏脚本引擎
        /// </summary>
        public Script ScriptEngine { get { return _scriptEngine; } }

        public string GetStackInfo { get { return ScriptEngine != null ? ScriptEngine.GetStackInfo() : ""; } }

        public void LoadFile(string file)
        {
            Script script = new Script();
            script.LoadLibrary();
            script.PushAssembly(typeof(System.Action).GetTypeInfo().Assembly);                        //System.Core.dll
            script.PushAssembly(typeof(GameObject).GetTypeInfo().Assembly);                           //UnityEngine.dll
            script.PushAssembly(typeof(UnityEngine.UI.CanvasScaler).GetTypeInfo().Assembly);          //UnityEngine.UI.dll
            script.PushAssembly(typeof(UnityEngine.Networking.MsgType).GetTypeInfo().Assembly);       //UnityEngine.Networking.dll
            script.LoadFile(Global.GlobalPath.GlobalMainPath);
            script.SetObject("MainPlayer", ScoUtils.GetMainPlayer());
            script.LoadFile(file);
            script.GetValue("Map").GetValue("Take").call();


          

            //_currentScript.SetValue("MainPlayer", ScorpioManager.Instance.ScriptEngine.CreateObject(ScoUtils.GetMainPlayer()));
            //Resource文件后缀改成txt  否则Unity不能识别TextAsset
            //string content = FileUtils.Instance.ReadFile(file);
            //TextAsset text = Resources.Load<TextAsset>("Scripts/" + file);
            //if (!FileUtils.Instance.ExistsFile(file))
            //{
            //    //第一个参数是脚本摘要 有需求可以自己定义
            //    Debug.LogError("找不到File : " + file);
            //    return null;
            //}
            //return ScriptEngine.LoadFile(file);

            //Debug.LogError("找不到File : " + file);
            //return null;
        }

        //public ScriptTable AddComponent(Component component, string name)
        //{
        //    if (component == null || string.IsNullOrEmpty(name)) return null;
        //    return AddComponent(component.gameObject, name);
        //}
        //public ScriptTable AddComponent(GameObject gameObject, string name)
        //{
        //    if (gameObject == null || string.IsNullOrEmpty(name)) return null;
        //    var table = m_Script.GetValue(name) as ScriptTable;
        //    if (table == null) return null;
        //    return AddComponent(gameObject, table, name);
        //}
        //public ScriptTable AddComponent(Component component, ScriptTable table)
        //{
        //    if (component == null || table == null) return null;
        //    return AddComponent(component.gameObject, table, "");
        //}
        //public ScriptTable AddComponent(GameObject gameObject, ScriptTable table)
        //{
        //    if (gameObject == null || table == null) return null;
        //    return AddComponent(gameObject, table, "");
        //}
        //public ScriptTable AddComponent(Component component, ScriptTable table, string name)
        //{
        //    if (component == null || table == null) return null;
        //    return AddComponent(component.gameObject, table, name);
        //}
        //public ScriptTable AddComponent(GameObject gameObject, ScriptTable table, string name)
        //{
        //    if (gameObject == null || table == null) return null;
        //    if (table.HasValue("Update") || table.HasValue("FixedUpdate"))
        //        gameObject.AddComponent<ScriptUpdateComponent>().Initialize(m_Script, table, name);
        //    else
        //        gameObject.AddComponent<ScriptComponent>().Initialize(m_Script, table, name);
        //    return table;
        //}
        //public ScriptTable GetComponent(Component component)
        //{
        //    if (component == null) return null;
        //    return GetComponent(component.gameObject);
        //}
        //public ScriptTable GetComponent(GameObject gameObject)
        //{
        //    if (gameObject == null) return null;
        //    ScriptComponent component = gameObject.GetComponent<ScriptComponent>();
        //    if (component == null) return null;
        //    return component.Table;
        //}
        //public ScriptTable GetComponent(Component component, string name)
        //{
        //    if (component == null) return null;
        //    return GetComponent(component.gameObject, name);
        //}
        //public ScriptTable GetComponent(GameObject gameObject, string name)
        //{
        //    if (gameObject == null) return null;
        //    ScriptComponent[] components = gameObject.GetComponents<ScriptComponent>();
        //    foreach (ScriptComponent component in components)
        //    {
        //        if (component.Name == name)
        //            return component.Table;
        //    }
        //    return null;
        //}

        //public void DelComponent(Component component)
        //{
        //    if (component == null) return;
        //    DelComponent(component.gameObject);
        //}
        //public void DelComponent(GameObject gameObject)
        //{
        //    if (gameObject == null) return;
        //    ScriptComponent component = gameObject.GetComponent<ScriptComponent>();
        //    if (component == null) return;
        //    Object.Destroy(component);
        //}
        //public void DelComponent(Component component, string name)
        //{
        //    if (component == null) return;
        //    DelComponent(component.gameObject, name);
        //}
        //public void DelComponent(GameObject gameObject, string name)
        //{
        //    if (gameObject == null) return;
        //    ScriptComponent[] components = gameObject.GetComponents<ScriptComponent>();
        //    foreach (ScriptComponent component in components)
        //    {
        //        if (component.Name == name)
        //        {
        //            Object.Destroy(component);
        //            return;
        //        }
        //    }
        //}

        public bool Load()
        {
            //ScorpioDelegateFactory.Initialize();
            //_scriptEngine = new Script();
            //ScriptEngine.LoadLibrary();
            //ScriptEngine.PushAssembly(typeof(System.Action).GetTypeInfo().Assembly);                        //System.Core.dll
            //ScriptEngine.PushAssembly(typeof(GameObject).GetTypeInfo().Assembly);                           //UnityEngine.dll
            //ScriptEngine.PushAssembly(typeof(UnityEngine.UI.CanvasScaler).GetTypeInfo().Assembly);          //UnityEngine.UI.dll
            //ScriptEngine.PushAssembly(typeof(UnityEngine.Networking.MsgType).GetTypeInfo().Assembly);       //UnityEngine.Networking.dll
            ////ScriptEngine.SetObject("print", ScriptEngine.CreateFunction(new ScriptPrint()));                    //载入print函数
            ////ScriptEngine.SetObject("loadfile", ScriptEngine.CreateFunction(new ScriptLoadScript(m_Script)));    //载入loadfile函数  根据自己需求自己修改，如果是普通路径可以查看 require 函数
            //LoadFile(Global.GlobalPath.ScriptPath + "Main");
            return true;
        }


        public bool UnLoad()
        {
            return true;
        }

        public void OnUpdate()
        {
        }



        //private void print(ScriptObject[] args)
        //{
        //    var stackInfo = ScorpioManager.Instance.ScriptEngine.GetCurrentStackInfo();
        //    var prefix = stackInfo.Breviary + ":" + stackInfo.Line + " : ";
        //    string str = "";
        //    for (int i = 0; i < args.Length; ++i)
        //    {
        //        str += args[i].ToString() + " ";
        //    }
        //    Debug.Log(prefix + str);
        //}
    }
    public class ScorpioDelegateFactory : DelegateTypeFactory
    {
        public static void Initialize()
        {
            ScriptUserdataDelegateType.SetFactory(new ScorpioDelegateFactory());
        }
        public Delegate CreateDelegate(Script script, Type type, ScriptFunction func)
        {
            if (type == typeof(System.Action))
                return new System.Action(() => { func.call(); });
            if (type == typeof(System.Action<System.String>))
                return new System.Action<System.String>((arg0) => { func.call(arg0); });
            if (type == typeof(System.Comparison<UnityEngine.Transform>))
                return new System.Comparison<UnityEngine.Transform>((arg0, arg1) => { return (System.Int32)Convert.ChangeType(script.CreateObject(func.call(arg0, arg1)).ObjectValue, typeof(System.Int32)); });
            //if (type == typeof(TimerCallBack))
            //    return new TimerCallBack((arg0, arg1) => { func.call(arg0, arg1); });
            if (type == typeof(UnityEngine.Application.LogCallback))
                return new UnityEngine.Application.LogCallback((arg0, arg1, arg2) => { func.call(arg0, arg1, arg2); });
            if (type == typeof(UnityEngine.Events.UnityAction))
                return new UnityEngine.Events.UnityAction(() => { func.call(); });
            if (type == typeof(UnityEngine.Events.UnityAction<System.Boolean>))
                return new UnityEngine.Events.UnityAction<System.Boolean>((arg0) => { func.call(arg0); });
            if (type == typeof(UnityEngine.Events.UnityAction<System.Int32>))
                return new UnityEngine.Events.UnityAction<System.Int32>((arg0) => { func.call(arg0); });
            if (type == typeof(UnityEngine.Events.UnityAction<System.Single>))
                return new UnityEngine.Events.UnityAction<System.Single>((arg0) => { func.call(arg0); });
            if (type == typeof(UnityEngine.Events.UnityAction<System.String>))
                return new UnityEngine.Events.UnityAction<System.String>((arg0) => { func.call(arg0); });
            if (type == typeof(UnityEngine.Events.UnityAction<UnityEngine.Vector2>))
                return new UnityEngine.Events.UnityAction<UnityEngine.Vector2>((arg0) => { func.call(arg0); });
            if (type == typeof(UnityEngine.Networking.NetworkMessageDelegate))
                return new UnityEngine.Networking.NetworkMessageDelegate((arg0) => { func.call(arg0); });
            //if (type == typeof(YieldCallback))
            //    return new YieldCallback(() => { func.call(); });
            throw new Exception("Delegate Type is not found : " + type + "  func : " + func);
        }
    }
}