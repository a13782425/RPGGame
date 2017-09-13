using UnityEngine;
using System.Collections;
using RPGGame.Global;
using LitJson;
using RPGGame.Actor;
using RPGGame.Manager;

namespace RPGGame.Utils
{
    /// <summary>
    /// 存档Util
    /// </summary>
    public class ArchivedUtils : Singleton<ArchivedUtils>
    {

        public bool NewGame()
        {
            //if (!FileUtils.Instance.ExistsFile(GlobalData.ArchivedPath))
            //{
            //    FirstGame();
            //    SaveArchived();
            //    return true;
            //    //第一次
            //}
            FirstGame();
            SaveArchived();
            return true;
        }

        /// <summary>
        /// 加载存档
        /// </summary>
        /// <returns></returns>
        public bool LoadArchived()
        {
            if (!FileUtils.Instance.ExistsFile(GlobalPath.ArchivedPath))
            {
                FirstGame();
                SaveArchived();
                return true;
                //第一次
            }
            string content = FileUtils.Instance.ReadFile(GlobalPath.ArchivedPath);
            try
            {
                content = EncryptUtils.Instance.Decipher(content, EncryptUtils.ArchivedEncryptKey);
                GlobalData.CurrentPlayer = JsonMapper.ToObject<PlayerInfo>(content);
                GlobalSetting.CurrentCameraRange = GetPlayerSetting(GlobalConst.CAMERA_RANGE);
                GlobalSetting.CurrentLanguage = (SystemLanguage)GetPlayerSetting(GlobalConst.SYSTEM_LANGUAGE);
                Debug.LogError(GlobalData.CurrentPlayer.Name);
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
                FirstGame();
                SaveArchived();
                return true;
            }
            //return false;
        }

        /// <summary>
        /// 保存存档
        /// </summary>
        /// <returns></returns>
        public bool SaveArchived()
        {
            string content = JsonMapper.ToJson(GlobalData.CurrentPlayer);
            content = EncryptUtils.Instance.Encryption(content, EncryptUtils.ArchivedEncryptKey);
            return FileUtils.Instance.WriteFile(content, GlobalPath.ArchivedPath);
        }

        public void SavePlayerSetting(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public int GetPlayerSetting(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
            else
            {
                switch (key)
                {
                    case GlobalConst.CAMERA_RANGE:
                        SavePlayerSetting(key, 200);
                        break;
                    case GlobalConst.SYSTEM_LANGUAGE:
                        SavePlayerSetting(key, (int)SystemLanguage.ChineseSimplified);
                        break;
                    default:
                        throw new System.Exception("Key不存在！");
                }
            }
            return GetPlayerSetting(key);
        }


        private void FirstGame()
        {
            GlobalData.CurrentPlayer = new PlayerInfo();
            GlobalSetting.CurrentLanguage = Application.systemLanguage;
            GlobalSetting.CurrentCameraRange = 200;
            SavePlayerSetting(GlobalConst.CAMERA_RANGE, GlobalSetting.CurrentCameraRange);
            SavePlayerSetting(GlobalConst.SYSTEM_LANGUAGE, (int)GlobalSetting.CurrentLanguage);
        }
    }
}
