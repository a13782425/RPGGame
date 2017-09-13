using UnityEngine;
using System.Collections;
using RPGGame.Actor;
using RPGGame.Enums;
using RPGGame.UI;
using System.Collections.Generic;
using System;
using RPGGame.Controller;

namespace RPGGame.Global
{
    #region 委托

    public delegate void ArgCallBackDelegate(params object[] args);
    public delegate void MessageCallBackDelegate(object obj);
    public delegate void DialogBackDelegate(bool result);
    public delegate void CallBackDelegate();
    public delegate void ActionCallBackDelegate(Action act);
    public delegate void Vec2CallBackDelegate(Vector2 originPos);
    #endregion

    /// <summary>
    /// 全局常量
    /// </summary>
    public class GlobalConst
    {
        #region 常量

        /// <summary>
        /// 生活职业最高上限
        /// </summary>
        public const int MAX_LIFE_JOB_LVL = 9;
        /// <summary>
        /// 最大饱食度
        /// </summary>
        public const int MAX_FEED_DEGREE = 150;

        /// <summary>
        /// 战斗场景相机范围
        /// </summary>
        public const int BATTLE_SCENE_CAMERA_RANGE = 500;

        /// <summary>
        /// 系统语言
        /// </summary>
        public const string SYSTEM_LANGUAGE = "SYSTEM_LANGUAGE";
        /// <summary>
        /// 相机范围
        /// </summary>
        public const string CAMERA_RANGE = "CAMERA_RANGE";

        #endregion
    }

    /// <summary>
    /// 全局设定
    /// </summary>
    public class GlobalSetting
    {

        #region 存档

        private static int _currentCameraRange = 200;
        /// <summary>
        /// 相机范围
        /// </summary>
        public static int CurrentCameraRange
        {
            get { return _currentCameraRange; }
            set
            {
                if (_currentCameraRange != value)
                {
                    Utils.ArchivedUtils.Instance.SavePlayerSetting(GlobalConst.CAMERA_RANGE, value);
                }
                _currentCameraRange = value;
            }
        }

        private static SystemLanguage _currentLanguage = Application.systemLanguage;
        /// <summary>
        /// 当前语言
        /// </summary>
        public static SystemLanguage CurrentLanguage
        {
            get { return _currentLanguage; }
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    Utils.ArchivedUtils.Instance.SavePlayerSetting(GlobalConst.SYSTEM_LANGUAGE, (int)value);
                    if (LanguageCallBack != null)
                    {
                        LanguageCallBack.Invoke();
                    }
                }

            }
        }

        private static CallBackDelegate _languageCallBack = null;

        public static CallBackDelegate LanguageCallBack { get { return _languageCallBack; } set { _languageCallBack = value; } }

        #endregion
    }

    /// <summary>
    /// 全局路径
    /// </summary>
    public class GlobalPath
    {
        #region 各种路径
        public static string StreamingAssetsPath
        {
            get
            {
                string filepath = string.Empty;
#if UNITY_EDITOR
                filepath = Application.dataPath + "/StreamingAssets";
#elif UNITY_IPHONE
	         filepath = Application.dataPath +"/Raw";
#elif UNITY_ANDROID
	         filepath = "jar:file://" + Application.dataPath + "!/assets/";
#endif
                return filepath;
            }
        }

        private static string _gamePath = "";
        /// <summary>
        /// 游戏路径
        /// </summary>
        public static string GamePath
        {
            get
            {
                if (string.IsNullOrEmpty(GlobalData.SelectGame))
                {
                    return "";
                }
                return Application.persistentDataPath + "/" + GlobalData.SelectGame;
            }
        }
        /// <summary>
        /// 存档路径
        /// </summary>
        public static string ArchivedPath
        {
            get
            {
                if (string.IsNullOrEmpty(GlobalPath.GamePath))
                {
                    return "";
                }
                return GlobalPath.GamePath + "/" + "archived.setting";
            }
        }
        /// <summary>
        /// 脚本路径
        /// </summary>
        public static string ScriptPath
        {
            get
            {
                if (string.IsNullOrEmpty(GlobalPath.GamePath))
                {
                    return "";
                }
                return GlobalPath.GamePath + "/Data/Script/";
            }
        }
        /// <summary>
        /// 技能脚本路径
        /// </summary>
        public static string SkillScriptPath
        {
            get
            {
                if (string.IsNullOrEmpty(GlobalPath.ScriptPath))
                {
                    return "";
                }
                return GlobalPath.ScriptPath + "Skill/";
            }
        }

        /// <summary>
        /// 场景脚本路径
        /// </summary>
        public static string MapScriptPath
        {
            get
            {
                if (string.IsNullOrEmpty(GlobalPath.ScriptPath))
                {
                    return "";
                }
                return GlobalPath.ScriptPath + "Map/";
            }
        }

        /// <summary>
        /// 任务脚本路径
        /// </summary>
        public static string TaskScriptPath
        {
            get
            {
                if (string.IsNullOrEmpty(GlobalPath.ScriptPath))
                {
                    return "";
                }
                return GlobalPath.ScriptPath + "Task/";
            }
        }
        /// <summary>
        /// 物品脚本路径
        /// </summary>
        public static string ItemScriptPath
        {
            get
            {
                if (string.IsNullOrEmpty(GlobalPath.ScriptPath))
                {
                    return "";
                }
                return GlobalPath.ScriptPath + "Item/";
            }
        }

        /// <summary>
        /// 表格路径
        /// </summary>
        public static string TablePath
        {
            get
            {
                if (string.IsNullOrEmpty(GlobalPath.GamePath))
                {
                    return "";
                }
                return GlobalPath.GamePath + "/Data/Table/";
            }
        }
        /// <summary>
        /// 地图路径
        /// </summary>
        public static string MapPath
        {
            get
            {
                if (string.IsNullOrEmpty(GlobalPath.GamePath))
                {
                    return "";
                }
                return GlobalPath.GamePath + "/Data/Maps/";
            }
        }

        #endregion
    }

    /// <summary>
    /// 全局数据
    /// </summary>
    public class GlobalData
    {
        public static int ScreenHeight
        {
            get { return Screen.height; }
        }
        public static int ScreenWidth
        {
            get { return Screen.width; }
        }

        /// <summary>
        /// 选择的游戏
        /// </summary>
        private static string _selectGame = "";
        /// <summary>
        /// 选择的游戏
        /// </summary>
        public static string SelectGame { get { return _selectGame; } set { _selectGame = value; } }

        /// <summary>
        /// 游戏开始到现在的时间，不受scale影响
        /// </summary>
        public static float Realtime
        {
            get { return Time.realtimeSinceStartup; }
        }

        /// <summary>
        /// 游戏开始到现在的时间，受scale影响
        /// </summary>
        public static float GameTime
        {
            get { return Time.time; }
        }



        private static PlayerInfo _currentPlayer = null;
        /// <summary>
        /// 当前用户数据
        /// </summary>
        public static PlayerInfo CurrentPlayer { get { return _currentPlayer; } set { _currentPlayer = value; } }

        private static PlayerController _currentPlayerController = null;
        /// <summary>
        /// 当前玩家控制器
        /// </summary>
        public static PlayerController CurrentPlayerController { get { return _currentPlayerController; } set { _currentPlayerController = value; } }

        private static bool _isCanMove = true;

        public static bool IsCanMove
        {
            get { return GlobalData._isCanMove; }
            set { GlobalData._isCanMove = value; }
        }



        private static Dictionary<UIEnum, string> _uiPathDic = new Dictionary<UIEnum, string>()
        {
            {UIEnum.MaskUI , "Tooltip/MaskUI"},
            {UIEnum.DialogUI , "Tooltip/DialogUI"},
            {UIEnum.MoveUI , "Main/MoveUI"},
            {UIEnum.OperationUI , "Main/OperationUI"},
            {UIEnum.MenuUI , "Main/MenuUI"},
            {UIEnum.SettingUI , "Menu/SettingMenuUI"},
            {UIEnum.DialogBoxUI , "Menu/DialogBoxUI"}
        };
        public static Dictionary<UIEnum, string> UiPathDic { get { return _uiPathDic; } }


    }
}
