using UnityEngine;
using System.Collections;
using RPGGame.Enums;
using System;
using RPGGame.Utils;
using RPGGame.Manager;
using RPGGame.Global;

namespace RPGGame.UI
{
    public class SettingUIController : BaseUI
    {
        private GameObject _saveBtn;
        private GameObject _languageBtn;
        private AutoFindWidget _auto;
        public override void Init()
        {
            _auto = AutoFindWidget.MyGet(this.gameObject);
            _saveBtn = _auto.MyGetOneGameObject("SaveBtn");
            _languageBtn = _auto.MyGetOneGameObject("LanguageBtn");

            UIEventTriggerListener.Get(_saveBtn).onClick = OnButtonClick;
            UIEventTriggerListener.Get(_languageBtn).onClick = OnButtonClick;
        }

        private void OnButtonClick(GameObject go)
        {
            string name = go.name;
            Debug.LogError(name);
            switch (name)
            {
                case "SaveBtn":
                    Vector2 tilePos = MapManager.Instance.CurrentMap.GetTilePosByPos(GlobalData.CurrentPlayerController.transform.position);
                    GlobalData.CurrentPlayer.X = (int)tilePos.x;
                    GlobalData.CurrentPlayer.Y = (int)tilePos.y;
                    if (ArchivedUtils.Instance.SaveArchived())
                    {
                        UIManager.Instance.ShowDialog("存档完成");
                    }
                    break;
                case "LanguageBtn":
                    if (Global.GlobalSetting.CurrentLanguage == SystemLanguage.Chinese || Global.GlobalSetting.CurrentLanguage == SystemLanguage.ChineseSimplified)
                    {
                        Global.GlobalSetting.CurrentLanguage = SystemLanguage.English;
                    }
                    else
                    {
                        Global.GlobalSetting.CurrentLanguage = SystemLanguage.ChineseSimplified;
                    }
                    break;
                default:
                    break;
            }
        }

        public override UIEnum CurrentEnum
        {
            get
            {
                return UIEnum.SettingUI;
            }
        }
    }
}