using UnityEngine;
using System.Collections;
using RPGGame.Enums;
using RPGGame.Manager;

namespace RPGGame.UI
{
    public class OperationController : BaseUI
    {
        private Transform _okBtn;
        private Transform _cancelBtn;

        public override void Init()
        {
            _okBtn = this.CurrentTransform.Find("OperationPanel/OKBtn");
            _cancelBtn = this.CurrentTransform.Find("OperationPanel/CancelBtn");
            UIEventTriggerListener.Get(_okBtn.gameObject).onClick = OnButtonClick;
            UIEventTriggerListener.Get(_cancelBtn.gameObject).onClick = OnButtonClick;
        }

        private void OnButtonClick(GameObject go)
        {
            string name = go.name;
            switch (name)
            {
                case "OKBtn":
                    if (InputManager.Instance.OnOkButtonClick != null)
                    {
                        InputManager.Instance.OnOkButtonClick(ButtonEnum.OK);
                    }
                    else if (!UIManager.Instance.IsMenuUIShow)
                    {
                        UIManager.Instance.ShowUI(UIEnum.MenuUI);
                    }
                    //if (Global.GlobalData.CurrentPlayerController.CurrentNpcController != null)
                    //{
                    //    if (UIManager.Instance.CurrentUI is DialogBoxController)
                    //    {
                    //        DialogBoxController dialog = (UIManager.Instance.CurrentUI as DialogBoxController);
                    //        dialog.ShowNext();
                    //    }
                    //    else
                    //    {
                    //        UIManager.Instance.ShowUI(UIEnum.DialogBoxUI);
                    //        DialogBoxController dialog = (UIManager.Instance.CurrentUI as DialogBoxController);
                    //        dialog.ShowContent(Global.GlobalData.CurrentPlayerController.CurrentNpcController);
                    //    }
                    //}
                    //else if (!UIManager.Instance.IsMenuUIShow)
                    //{
                    //    UIManager.Instance.ShowUI(UIEnum.MenuUI);
                    //}
                    break;
                case "CancelBtn":
                    UIManager.Instance.ReturnUI();// HideUI(UIEnum.MenuUI);
                    break;
                default:
                    break;
            }
        }


        public override UIEnum CurrentEnum
        {
            get { return UIEnum.OperationUI; }
        }
    }
}
