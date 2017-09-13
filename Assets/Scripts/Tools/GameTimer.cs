using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPGGame.Global;
//#if UNITY_EDITOR
using System.Diagnostics;
using System;
//#endif
namespace RPGGame.Tools
{
    public class GameTimer : MonoBehaviour
    {

        private static GameObject _mainObject = null;
        private static List<TimerEvent> _activeList = new List<TimerEvent>();
        private static List<TimerEvent> _poolList = new List<TimerEvent>();

        private static TimerEvent _newEvent = null;
        private static int _eventCount = 0;


        private static int _eventBatch = 0;
        private static int _eventIterator = 0;

        private static bool _appQuitting = false;

        public static int MaxEventsPerFrame = 500;

        public struct Stats
        {
            public int Created;
            public int Inactive;
            public int Active;
        }
        public bool WasAddedCorrectly
        {
            get
            {
                if (!Application.isPlaying || gameObject != _mainObject)
                {
                    return false;
                }
                return true;
            }
        }


        void Awake()
        {
            if (!WasAddedCorrectly)
            {
                Destroy(this);
                return;
            }
        }

        void Update()
        {
            _eventBatch = 0;
            while ((GameTimer._activeList.Count > 0) && _eventBatch < MaxEventsPerFrame)
            {
                if (_eventIterator < 0)
                {
                    _eventIterator = GameTimer._activeList.Count;
                    break;
                }
                if (_eventIterator > GameTimer._activeList.Count - 1)
                {
                    _eventIterator = GameTimer._activeList.Count - 1;
                }
                if (GlobalData.GameTime > GameTimer._activeList[_eventIterator].DueTime || GameTimer._activeList[_eventIterator].Id == 0)
                {
                    GameTimer._activeList[_eventIterator].Execute();
                }
                else
                {
                    if (GameTimer._activeList[_eventIterator].Paused)
                    {
                        GameTimer._activeList[_eventIterator].DueTime += Time.deltaTime;
                    }
                    else
                    {
                        GameTimer._activeList[_eventIterator].LifeTime += Time.deltaTime;
                    }
                }
                _eventIterator--;
                _eventBatch++;
            }
        }


        public static void In(float delay, CallBackDelegate callBack, TimerHandle timerHandle = null)
        {
            Schedule(delay, callBack, null, null, timerHandle, 1, -1.0f);
        }
        public static void In(float delay, CallBackDelegate callBack, int iterations, TimerHandle timerHandle = null)
        {
            Schedule(delay, callBack, null, null, timerHandle, iterations, -1.0f);
        }
        public static void In(float delay, CallBackDelegate callBack, int iterations, float interval, TimerHandle timerHandle = null)
        {
            Schedule(delay, callBack, null, null, timerHandle, iterations, interval);
        }

        public static void In(float delay, ArgCallBackDelegate callBack, object arg, TimerHandle timerHandle = null)
        {
            Schedule(delay, null, callBack, arg, timerHandle, 1, -1.0f);
        }
        public static void In(float delay, ArgCallBackDelegate callBack, object arg, int iterations, TimerHandle timerHandle = null)
        {
            Schedule(delay, null, callBack, arg, timerHandle, iterations, -1.0f);
        }
        public static void In(float delay, ArgCallBackDelegate callBack, object arg, int iterations, float interval, TimerHandle timerHandle = null)
        {
            Schedule(delay, null, callBack, arg, timerHandle, iterations, interval);
        }


        public static void Start(TimerHandle timerHandle)
        {
            Schedule(315360000.0f, delegate () { }, null, null, timerHandle, 1, -1.0f);
        }

        private static void Schedule(float time, CallBackDelegate func, ArgCallBackDelegate argFunc, object args, TimerHandle timerHandle, int iterations, float interval)
        {
            if (func == null && argFunc == null)
            {
                UnityEngine.Debug.LogError("没有要执行的方法");
                return;
            }
            if (_mainObject == null)
            {
                if (!_appQuitting)
                {
                    _mainObject = new GameObject("GameTimer");
                    _mainObject.AddComponent<GameTimer>();
                    DontDestroyOnLoad(_mainObject);
                }
                else
                    return;
            }
            time = Mathf.Max(0.0f, time);
            iterations = Mathf.Max(0, iterations);
            interval = (interval == -1.0f) ? time : Mathf.Max(0.0f, interval);
            _newEvent = null;
            if (_poolList.Count > 0)
            {
                _newEvent = _poolList[0];
                _poolList.Remove(_newEvent);
            }
            else
            {
                _newEvent = new TimerEvent();
            }
            GameTimer._eventCount++;
            _newEvent.Id = GameTimer._eventCount;
            if (func != null)
            {
                _newEvent.Function = func;
            }
            else if (argFunc != null)
            {
                _newEvent.ArgFunction = argFunc;
                _newEvent.Arguments = args;
            }
            _newEvent.StartTime = Time.time;
            _newEvent.DueTime = Time.time + time;
            _newEvent.Iterations = iterations;
            _newEvent.Interval = interval;
            _newEvent.LifeTime = 0.0f;
            _newEvent.Paused = false;
            GameTimer._activeList.Add(_newEvent);
            if (timerHandle != null)
            {
                if (timerHandle.Active)
                {
                    timerHandle.Cancel();
                }
                timerHandle.Id = _newEvent.Id;
            }
#if UNITY_EDITOR
            _newEvent.StoreCallingMethod();
            EditorRefresh();
#endif
        }

        private static void Cancel(TimerHandle timerHandle)
        {
            if (timerHandle == null)
            {
                return;
            }
            if (timerHandle.Active)
            {
                timerHandle.Id = 0;
                return;
            }
        }

        public static void CancelAll()
        {
            for (int i = GameTimer._activeList.Count - 1; i > -1; i--)
            {
                GameTimer._activeList[i].Id = 0;
            }
        }
        public static void CancelAll(string methodName)
        {
            for (int i = GameTimer._activeList.Count - 1; i > -1; i--)
            {
                if (GameTimer._activeList[i].MethodName == methodName)
                {
                    GameTimer._activeList[i].Id = 0;
                }
            }
        }

        public static void DestoryAll()
        {
            GameTimer._activeList.Clear();
            GameTimer._poolList.Clear();
#if UNITY_EDITOR
            EditorRefresh();
#endif
        }

        private void OnLevelWasLoaded()
        {
            for (int i = GameTimer._activeList.Count - 1; i > -1; i--)
            {
                if (GameTimer._activeList[i].CancelOnLoad)
                {
                    GameTimer._activeList[i].Id = 0;
                }
            }
        }

        public static Stats EditorGetStats()
        {
            Stats stats;
            stats.Created = _activeList.Count + _poolList.Count;
            stats.Inactive = _poolList.Count;
            stats.Active = _activeList.Count;
            return stats;
        }

        public static string EditorGetMethodInfo(int eventIndex)
        {
            if (eventIndex < 0 || eventIndex > _activeList.Count - 1)
            {
                return "参数越界！";
            }
            return _activeList[eventIndex].MethodInfo;
        }

        public static int EditorGetMethodId(int eventIndex)
        {
            if (eventIndex < 0 || eventIndex > _activeList.Count - 1)
            {
                return 0;
            }
            return _activeList[eventIndex].Id;
        }

#if UNITY_EDITOR
        private static void EditorRefresh()
        {
            if (!_appQuitting)
            {
                _mainObject.name = "Times (" + _activeList.Count + "/" + (_poolList.Count + _activeList.Count).ToString() + ")";
            }
        }
#endif

        private class TimerEvent
        {
            public int Id;
            public CallBackDelegate Function = null;
            public ArgCallBackDelegate ArgFunction = null;
            public object Arguments = null;

            public int Iterations = 1;
            public float Interval = -1.0f;
            public float DueTime = 0.0f;
            public float StartTime = 0.0f;
            public float LifeTime = 0.0f;
            public bool Paused = false;
            public bool CancelOnLoad = true;

#if UNITY_EDITOR
            private string _callingMethod = "";
#endif

            public void Execute()
            {
                if (Id == 0 || DueTime == 0.0f)
                {
                    Recycle();
                    return;
                }
                if (Function != null)
                {
                    Function();
                }
                else if (ArgFunction != null)
                {
                    ArgFunction(Arguments);
                }
                else
                {
                    UnityEngine.Debug.LogError("没有执行的方法！");
                    Recycle();
                    return;
                }
                if (Iterations > 0)
                {
                    Iterations--;
                    if (Iterations < 1)
                    {
                        Recycle();
                        return;
                    }
                }
                DueTime = Time.time - Interval;

            }

            private void Recycle()
            {
                Id = 0;
                DueTime = 0.0f;
                StartTime = 0.0f;
                CancelOnLoad = false;

                Function = null;
                ArgFunction = null;
                Arguments = null;
                if (GameTimer._activeList.Remove(this))
                {
                    GameTimer._poolList.Add(this);
                }
#if UNITY_EDITOR
                EditorRefresh();
#endif
            }

            private void Destroy()
            {
                GameTimer._activeList.Remove(this);
                GameTimer._poolList.Remove(this);
            }

#if UNITY_EDITOR
            public void StoreCallingMethod()
            {
                StackTrace stackTrace = new StackTrace();
                string result = "";
                string declaringType = "";
                for (int i = 3; i < stackTrace.FrameCount; i++)
                {
                    StackFrame stackFrame = stackTrace.GetFrame(i);
                    declaringType = stackFrame.GetMethod().DeclaringType.ToString();
                    result += "<-" + declaringType + ":" + stackFrame.GetMethod().Name;
                }
                _callingMethod = result;
            }
#endif
            private void Error(string message)
            {
                string msg = "Error:(" + this + ")" + message;
#if UNITY_EDITOR
                msg += MethodInfo;
#endif
                UnityEngine.Debug.LogError(msg);
            }

            public string MethodName
            {
                get
                {
                    if (Function != null)
                    {
                        if (Function.Method != null)
                        {
                            if (Function.Method.Name[0] == '<')
                            {
                                return "delegate";
                            }
                            else
                            {
                                return ArgFunction.Method.Name;
                            }
                        }
                    }
                    return null;
                }
            }
            public string MethodInfo
            {
                get
                {
                    string s = MethodName;
                    if (!string.IsNullOrEmpty(s))
                    {
                        s += "(";
                        if (Arguments != null)
                        {
                            if (Arguments.GetType().IsArray)
                            {
                                object[] array = (object[])Arguments;
                                foreach (object item in array)
                                {
                                    s += item.ToString();
                                    if (Array.IndexOf(array, item) < array.Length - 1)
                                    {
                                        s += ",";
                                    }
                                }
                            }
                            else
                            {
                                s += Arguments;
                            }
                        }
                        s += ")";
                    }
                    else
                    {
                        s += "Function == null";
                    }
#if UNITY_EDITOR
                    s += _callingMethod;
#endif
                    return s;
                }
            }
        }



        public class TimerHandle
        {
            private TimerEvent _event = null;
            private int _id = 0;
            private int _startIterations = 1;
            private float _firstDueTime = 0.0f;

            public bool Paused
            {
                get { return Active && _event.Paused; }
                set
                {
                    if (Active)
                    {
                        _event.Paused = value;
                    }
                }
            }

            public float TimeOfInitiation
            {
                get
                {
                    if (Active)
                    {
                        return _event.StartTime;
                    }
                    else
                        return 0.0f;
                }
            }
            public float TimeOfFirstIteration
            {
                get
                {
                    if (Active)
                    {
                        return _firstDueTime;
                    }
                    else
                        return 0.0f;
                }
            }

            public float TimeOfNextIteration
            {
                get
                {
                    if (Active)
                    {
                        return _event.DueTime;
                    }
                    else
                        return 0.0f;
                }
            }
            public float TimeOfLastIteration
            {
                get
                {
                    if (Active)
                    {
                        return Time.time + DurarionLeft;
                    }
                    else
                        return 0.0f;
                }
            }

            public float Delay
            {
                get
                {
                    return (Mathf.Round((_firstDueTime - TimeOfInitiation) * 1000.0f) / 1000.0f);
                }
            }
            public float Interval
            {
                get
                {
                    if (Active)
                        return _event.Interval;
                    else
                        return 0.0f;
                }
            }
            public float TimeUntilNextIteration
            {
                get
                {
                    if (Active)
                        return _event.DueTime - Time.time;
                    else
                        return 0.0f;
                }
            }

            public float DurarionLeft
            {
                get
                {
                    if (Active)
                    {
                        return TimeUntilNextIteration + ((_startIterations - 1) * _event.Interval);
                    }
                    return 0.0f;
                }
            }

            public float DurationTotal
            {
                get
                {
                    if (Active)
                        return Delay + ((_startIterations) * ((_startIterations > 1) ? Interval : 0.0f));
                    else
                        return 0.0f;
                }
            }

            public float Duration
            {
                get
                {
                    if (Active)
                        return _event.LifeTime;
                    else
                        return 0.0f;
                }
            }

            public int IterationsTotal
            {
                get
                {
                    if (Active)
                    {
                        return _startIterations;
                    }
                    return 0;
                }
            }

            public int IterationsLeft
            {
                get
                {
                    if (Active)
                    {
                        return _event.Iterations;
                    }
                    return 0;
                }
            }

            public int Id
            {
                get { return _id; }
                set
                {
                    _id = value;
                    if (_id == 0)
                    {
                        _event.DueTime = 0.0f;
                        return;
                    }
                    _event = null;
                    for (int i = GameTimer._activeList.Count - 1; i > -1; i--)
                    {
                        if (GameTimer._activeList[i].Id == _id)
                        {
                            _event = GameTimer._activeList[i];
                            break;
                        }
                    }
                    if (_event == null)
                        UnityEngine.Debug.LogError("没有找到事件");
                    _startIterations = _event.Iterations;
                    _firstDueTime = _event.DueTime;
                }
            }

            public bool Active
            {
                get
                {
                    if (_event == null || Id == 0 || _event.Id == 0)
                    {
                        return false;
                    }
                    return _event.Id == Id;
                }
            }

            public string MethodName
            {
                get
                {
                    if (Active)
                    {
                        return _event.MethodName;
                    }
                    return "";
                }
            }
            public string MethodInfo
            {
                get
                {
                    if (Active)
                    {
                        return _event.MethodInfo;
                    }
                    return "";
                }
            }
            public bool CancelOnLoad
            {
                get
                {
                    if (Active)
                    {
                        return _event.CancelOnLoad;
                    }
                    return true;
                }
                set
                {
                    if (Active)
                    {
                        _event.CancelOnLoad = value;
                        return;
                    }
                    UnityEngine.Debug.LogWarning("!!!!!!!!!!");
                }
            }
            public void Cancel()
            {
                GameTimer.Cancel(this);
            }
            public void Execute()
            {
                _event.DueTime = Time.time;
            }
        }

















































        void OnApplicationQuit()
        {
            _appQuitting = true;
        }

    }
}
