using UnityEngine;
using System.Collections;
using RPGGame.Enums;
using RPGGame.Manager;

namespace RPGGame.UI
{
    public class MenuUIController : BaseUI
    {
        private GameObject _roleBtn;
        private GameObject _propBtn;
        private GameObject _bagBtn;
        private GameObject _skillBtn;
        private GameObject _lifeSkillBtn;
        private GameObject _settingBtn;

        private AutoFindWidget _auto;
        public override void Init()
        {
            _auto = AutoFindWidget.MyGet(this.gameObject);
            _roleBtn = _auto.MyGetOneGameObject("RoleBtn");
            _propBtn = _auto.MyGetOneGameObject("PropBtn");
            _bagBtn = _auto.MyGetOneGameObject("BagBtn");
            _skillBtn = _auto.MyGetOneGameObject("SkillBtn");
            _lifeSkillBtn = _auto.MyGetOneGameObject("LifeSkillBtn");
            _settingBtn = _auto.MyGetOneGameObject("SettingBtn");
            UIEventTriggerListener.Get(_roleBtn).onClick = OnButtonClick;
            UIEventTriggerListener.Get(_propBtn).onClick = OnButtonClick;
            UIEventTriggerListener.Get(_bagBtn).onClick = OnButtonClick;
            UIEventTriggerListener.Get(_skillBtn).onClick = OnButtonClick;
            UIEventTriggerListener.Get(_lifeSkillBtn).onClick = OnButtonClick;
            UIEventTriggerListener.Get(_settingBtn).onClick = OnButtonClick;
        }

        public override void Show()
        {
            base.Show();
            Global.GlobalData.IsCanMove = false;
        }

        public override void Hide()
        {
            base.Hide();
            Global.GlobalData.IsCanMove = true;
        }

        private void OnButtonClick(GameObject go)
        {
            string name = go.name;
            switch (name)
            {
                case "RoleBtn":
                    UIManager.Instance.ShowDialog("点击" + name + "按钮，功能正在开发...");
                    break;
                case "PropBtn":
                    UIManager.Instance.ShowDialog("点击" + name + "按钮，功能正在开发...");
                    break;
                case "BagBtn":
                    UIManager.Instance.ShowDialog("点击" + name + "按钮，功能正在开发...");
                    break;
                case "SkillBtn":
                    UIManager.Instance.ShowDialog("点击" + name + "按钮，功能正在开发...");
                    break;
                case "LifeSkillBtn":
                    UIManager.Instance.ShowDialog("点击" + name + "按钮，功能正在开发...");
                    break;
                case "SettingBtn":
                    UIManager.Instance.ShowUI(UIEnum.SettingUI);
                    break;
                default:
                    break;
            }
        }



        public override UIEnum CurrentEnum
        {
            get { return UIEnum.MenuUI; }
        }
    }
}