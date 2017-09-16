using RPGGame.Enums;
using RPGGame.Utils;
using Scorpio;
using Scorpio.Userdata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RPGGame.Manager
{
    public class ScorpioManager : RPGGame.Tools.Singleton<ScorpioManager>, IManager
    {
        private Dictionary<ScoScriptEnum, Script> _currentScript = new Dictionary<ScoScriptEnum, Script>();

        public Dictionary<ScoScriptEnum, Script> CurrentScript { get { return _currentScript; } }

        private Dictionary<ScoScriptEnum, List<string>> _currentLoadFileName = new Dictionary<ScoScriptEnum, List<string>>();
        public Dictionary<ScoScriptEnum, List<string>> CurrentLoadFileName { get { return _currentLoadFileName; } }

        //private Script _scriptEngine = null;         //脚本引擎
        ///// <summary>
        ///// 游戏脚本引擎
        ///// </summary>
        //public Script ScriptEngine { get { return _scriptEngine; } }

        //public string GetStackInfo { get { return ScriptEngine != null ? ScriptEngine.GetStackInfo() : ""; } }

        public ScriptTable LoadFile(ScoScriptEnum scriptEnum, string fileName)
        {
            switch (scriptEnum)
            {
                case ScoScriptEnum.Map:
                    return LoadMapFile(fileName);
                case ScoScriptEnum.Skill:
                case ScoScriptEnum.Task:
                case ScoScriptEnum.Npc:
                case ScoScriptEnum.Item:
                case ScoScriptEnum.Monster:
                default:
                    Debug.LogError("未知脚本类型!!!");
                    return null;
            }
        }

        public ScriptTable LoadMapFile(string fileName)
        {
            ScriptTable scriptTable = GetScriptTableByName(ScoScriptEnum.Map, fileName);
            if (scriptTable == null)
            {
                //获取到文件名称
                string name = Path.GetFileNameWithoutExtension(fileName);
                Script script = CurrentScript[ScoScriptEnum.Map];
                string str = FileUtils.Instance.ReadFile(Global.GlobalPath.MapScriptPath + name);
                str = str.Insert(0, name + "={");
                str += "}";
                script.LoadString(str);
                ScriptObject obj = script.GetValue(name);
                if (obj.IsNull)
                {
                    Debug.LogError(name + ",table not found");
                    return null;
                }
                ScriptObject map = obj.GetValue("Map");
                if (map.IsNull)
                {
                    Debug.LogError("map table not found");
                    return null;
                }
                map.SetValue("MainPlayer", script.CreateObject(ScoUtils.GetMainPlayer()));
            }
            return scriptTable;
        }


        public ScriptTable GetScriptTableByName(ScoScriptEnum scriptEnum, string fileName)
        {
            //获取到文件名称
            string name = Path.GetFileNameWithoutExtension(fileName);
            ScriptObject enumObj = CurrentScript[scriptEnum].GetValue(name);
            if (!enumObj.IsNull)
            {
                return enumObj.GetValue(scriptEnum.ToString()) as ScriptTable;
            }
            return null;
        }

        public object Call(ScriptObject table, string funcName, params object[] para)
        {
            ScriptObject fun = table.GetValue(funcName);
            if (fun != null)
            {
                return fun.call(para);
            }
            return null;
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
            foreach (int item in Enum.GetValues(typeof(ScoScriptEnum)))
            {
                ScoScriptEnum sco = (ScoScriptEnum)item;
                if (CurrentScript.ContainsKey(sco) || CurrentLoadFileName.ContainsKey(sco))
                {
                    Debug.LogWarning("CurrentScript or CurrentLoadFileName exist this enum :" + sco.ToString());
                    continue;
                }
                Script script = new Script();
                script.LoadLibrary();
                script.PushAssembly(typeof(System.Action).GetTypeInfo().Assembly);                        //System.Core.dll
                script.PushAssembly(typeof(GameObject).GetTypeInfo().Assembly);                           //UnityEngine.dll
                script.PushAssembly(typeof(UnityEngine.UI.CanvasScaler).GetTypeInfo().Assembly);          //UnityEngine.UI.dll
                script.PushAssembly(typeof(UnityEngine.Networking.MsgType).GetTypeInfo().Assembly);       //UnityEngine.Networking.dll
                script.LoadFile(Global.GlobalPath.GlobalMainPath);
                script.SetObject("print", new ScorpioFunction(print));
                script.SetObject("printWarn", new ScorpioFunction(print));
                script.SetObject("printError", new ScorpioFunction(print));
                CurrentScript.Add(sco, script);
                CurrentLoadFileName.Add(sco, new List<string>());
            }
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
            foreach (KeyValuePair<ScoScriptEnum, Script> item in CurrentScript)
            {
                item.Value.ClearStackInfo();
            }
            CurrentScript.Clear();
            CurrentLoadFileName.Clear();
            return true;
        }

        public void OnUpdate()
        {
        }
        #region ScoScript

        /// <summary>
        /// 脚本打印
        /// </summary>
        /// <param name="script"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        private object print(Script script, object[] Parameters)
        {
            for (int i = 0; i < Parameters.Length; ++i)
            {
                Debug.Log(Parameters[i].ToString());
            }
            return null;
        }
        /// <summary>
        /// 脚本打印
        /// </summary>
        /// <param name="script"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        private object printWarn(Script script, object[] Parameters)
        {
            for (int i = 0; i < Parameters.Length; ++i)
            {
                Debug.LogWarning(Parameters[i].ToString());
            }
            return null;
        }
        /// <summary>
        /// 脚本打印
        /// </summary>
        /// <param name="script"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        private object printError(Script script, object[] Parameters)
        {
            for (int i = 0; i < Parameters.Length; ++i)
            {
                Debug.LogError(Parameters[i].ToString());
            }
            return null;
        }
        #endregion


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