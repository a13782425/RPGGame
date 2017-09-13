using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;

namespace RPGGame.Manager
{
    public sealed class DeviceManager : RPGGame.Tools.Singleton<DeviceManager>, IManager
    {

        #region 私有字段

        private bool _gameCenterState = false;
        /// <summary>
        /// GameCenter状态
        /// </summary>
        public bool GameCenterState { get { return _gameCenterState; } }

        private string _userName = string.Empty;
        /// <summary>
        /// 用户名字
        /// </summary>
        public string UserName
        {
            get
            {
                return _userName;
            }
        }

        private string _userId = string.Empty;
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId
        {
            get
            {
                return _userId;
            }
        }

        private bool _underage = false;
        /// <summary>
        /// 是否未成年
        /// </summary>
        public bool Underage
        {
            get
            {
                return _underage;
            }
        }

        #endregion


#if UNITY_IPHONE
        /// <summary>  
        /// 初始化 GameCenter 结果回调函数  
        /// </summary>  
        /// <param name="success">If set to <c>true</c> success.</param>  
        private void HandleAuthenticated(bool success)
        {
            _gameCenterState = success;
            Debug.Log("*** HandleAuthenticated: success = " + success);
            ///初始化成功  
            if (success)
            {
                _userName = Social.localUser.userName;
                _userId = Social.localUser.id;
                _underage = Social.localUser.underage;
                string userInfo = "Username: " + Social.localUser.userName +
                    "\nUser ID: " + Social.localUser.id +
                    "\nIsUnderage: " + Social.localUser.underage;
                Debug.Log(userInfo);
            }
            else
            {
                ///初始化失败  

            }
        }

        #region 测试

        void OnGUI()
        {

            GUI.TextArea(new Rect(Screen.width - 200, 0, 200, 100), "GameCenter:" + GameCenterState);
            GUI.TextArea(new Rect(Screen.width - 200, 100, 200, 100), "userInfo:" + UserName);

            if (GUI.Button(new Rect(0, 0, 110, 75), "打开成就"))
            {

                if (Social.localUser.authenticated)
                {
                    Social.ShowAchievementsUI();
                }
            }

            if (GUI.Button(new Rect(0, 150, 110, 75), "打开排行榜"))
            {

                if (Social.localUser.authenticated)
                {
                    Social.ShowLeaderboardUI();
                }
            }

            if (GUI.Button(new Rect(0, 300, 110, 75), "排行榜设置分数"))
            {

                if (Social.localUser.authenticated)
                {
                    Social.ReportScore(1000, "XXXX", HandleScoreReported);
                }
            }

            if (GUI.Button(new Rect(0, 300, 110, 75), "设置成就"))
            {

                if (Social.localUser.authenticated)
                {
                    Social.ReportProgress("XXXX", 15, HandleProgressReported);
                }
            }


        }

        //上传排行榜分数  
        public void HandleScoreReported(bool success)
        {
            Debug.Log("*** HandleScoreReported: success = " + success);
        }
        //设置 成就  
        private void HandleProgressReported(bool success)
        {
            Debug.Log("*** HandleProgressReported: success = " + success);
        }

        /// <summary>  
        /// 加载好友回调  
        /// </summary>  
        /// <param name="success">If set to <c>true</c> success.</param>  
        private void HandleFriendsLoaded(bool success)
        {
            Debug.Log("*** HandleFriendsLoaded: success = " + success);
            foreach (IUserProfile friend in Social.localUser.friends)
            {
                Debug.Log("* friend = " + friend.ToString());
            }
        }

        /// <summary>  
        /// 加载成就回调  
        /// </summary>  
        /// <param name="achievements">Achievements.</param>  
        private void HandleAchievementsLoaded(IAchievement[] achievements)
        {
            Debug.Log("* HandleAchievementsLoaded");
            foreach (IAchievement achievement in achievements)
            {
                Debug.Log("* achievement = " + achievement.ToString());
            }
        }

        /// <summary>  
        ///   
        /// 成就回调描述  
        /// </summary>  
        /// <param name="achievementDescriptions">Achievement descriptions.</param>  
        private void HandleAchievementDescriptionsLoaded(IAchievementDescription[] achievementDescriptions)
        {
            Debug.Log("*** HandleAchievementDescriptionsLoaded");
            foreach (IAchievementDescription achievementDescription in achievementDescriptions)
            {
                Debug.Log("* achievementDescription = " + achievementDescription.ToString());
            }
        }


        #endregion
#endif

        public bool Load()
        {
#if UNITY_IPHONE
            Social.localUser.Authenticate(HandleAuthenticated);
#else
            _gameCenterState = false;
#endif
            return true;
        }

        public bool UnLoad()
        {
            return true;
        }


        public void OnUpdate()
        {
        }
    }
}