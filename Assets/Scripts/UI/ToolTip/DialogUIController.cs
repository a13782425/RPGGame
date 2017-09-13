using UnityEngine;
using System.Collections;
using RPGGame.Enums;
using RPGGame.Manager;
using RPGGame.Global;
using System;
using UnityEngine.UI;
using RPGGame.Utils;

namespace RPGGame.UI
{
    public class DialogUIController : BaseUI
    {
        #region 字段

        public override UIEnum CurrentEnum
        {
            get { return UIEnum.DialogUI; }
        }
        public override UITypeEnum CurrentTypeEnum
        {
            get
            {
                return UITypeEnum.ToolTip;
            }
        }

        private DialogEnum _currentDialogEnum = DialogEnum.Tip;
        /// <summary>
        /// 对象池
        /// </summary>
        private BelongPool _myPool;

        private AutoFindWidget _auto;
        /// <summary>
        /// 提示
        /// </summary>
        private Transform _tooltipObj;
        /// <summary>
        /// 遮罩
        /// </summary>
        private GameObject _mask;

        #region 对话框
        /// <summary>
        /// 对话框
        /// </summary>
        private GameObject _talkDig;
        /// <summary>
        /// 对话框文字
        /// </summary>
        private Text _talkContext;
        /// <summary>
        /// 对话框头像
        /// </summary>
        private Image _talkHeadIcon;
        /// <summary>
        /// 对话框按钮
        /// </summary>
        private GameObject _talkBtn;
        #endregion

        #region 正常对话框    
        /// <summary>
        /// 正常对话框
        /// </summary>
        private GameObject _normalDig;
        /// <summary>
        /// 正常对话框文字
        /// </summary>
        private Text _normalText;
        /// <summary>
        /// 正常对话框背景
        /// </summary>
        private Image _normalImage;

        #endregion

        #region 带确定和取消键的对话框
        /// <summary>
        /// 带确定和取消键的对话框
        /// </summary>
        private GameObject _okWithCancelDig;

        private DialogBackDelegate _callBack = null;

        #endregion

        #endregion

        public override void Init()
        {
            _auto = AutoFindWidget.MyGet(this.gameObject);
            _tooltipObj = _auto.MyGetOneGameObject("TooltipObj").transform;
            _mask = _auto.MyGetOneGameObject("Mask");
            _talkDig = _auto.MyGetOneGameObject("TalkDialog");
            _talkContext = _auto.MyGetComponentByName<Text>("TalkContent");
            _talkHeadIcon = _auto.MyGetComponentByName<Image>("TalkHeadIcon");
            _talkBtn = _auto.MyGetOneGameObject("TalkContentBtn");

            _normalDig = _auto.MyGetOneGameObject("NormalDialog");
            _normalText = _auto.MyGetComponentByName<Text>("NormalContent");
            _normalImage = _auto.MyGetComponentByName<Image>("NormalImage");
            _okWithCancelDig = _auto.MyGetOneGameObject("OkWithCancelDialog");

            _myPool = BelongManager.Pools["Tooltip"];
            BelongPrefabPool b1 = new BelongPrefabPool(_tooltipObj);
            b1.InitCount = 5;
            b1.MaxCount = 20;
            b1.ShowName = "ToolTipObj";
            _myPool.CreatePrefabPool(b1);
            _tooltipObj.gameObject.SetActive(false);
            _talkDig.gameObject.SetActive(false);
            _normalDig.gameObject.SetActive(false);
            _normalImage.color = new Color(1, 1, 1, 0);
            _okWithCancelDig.gameObject.SetActive(false);
            _mask.SetActive(false);

            UIEventTriggerListener.Get(_talkBtn).onClick = OnButtonClick;
            base.Init();
        }

        #region 公共方法

        public void ShowDialog(string msg)
        {
            ShowDialog(msg, null);
        }
        public void ShowDialog(string msg, string headIcon)
        {
            ShowDialog(msg, headIcon, DialogEnum.Normal);
        }
        public void ShowDialog(string msg, string headIcon, DialogEnum dialogEnum)
        {
            ShowDialog(msg, headIcon, dialogEnum, null);
        }
        public void ShowDialog(string msg, string headIcon, DialogEnum dialogEnum, DialogBackDelegate callBack)
        {
            _currentDialogEnum = dialogEnum;
            switch (dialogEnum)
            {

                case DialogEnum.Talk:
                    ShowTalkDialog(msg, headIcon, callBack);
                    break;
                case DialogEnum.WithOk:
                    ShowOkDialog(msg, callBack);
                    break;
                case DialogEnum.WithOkOrCancel:
                    ShowOkOrCancelDialog(msg, callBack);
                    break;
                case DialogEnum.Tip:
                    ShowTipDialog(msg);
                    break;
                case DialogEnum.Normal:
                default:
                    ShowNormalDialog(msg);
                    break;
            }
        }

        public void HideDialog()
        {
            _talkDig.SetActive(false);

            _callBack = null;
        }



        #endregion

        #region Unity

        #endregion

        #region 私有方法

        private void OnButtonClick(GameObject go)
        {
            string name = go.name;
            switch (name)
            {
                case "CancelBtn":
                    if (_callBack != null)
                    {
                        _callBack(false);
                    }
                    break;
                case "TalkContentBtn":
                case "OkayBtn":
                case "OkBtn":
                default:
                    if (_callBack != null)
                    {
                        _callBack(true);
                    }
                    break;
            }
        }
        private void ShowTipDialog(string msg)
        {
            Transform toolTran = _myPool.Extract("ToolTipObj");
            toolTran.GetComponent<TooltipObjController>().ShowContent(msg);
        }
        private void ShowNormalDialog(string msg)
        {
            if (_normalDig.activeInHierarchy)
            {
                return;
            }
            _normalText.text = msg;
            _normalDig.SetActive(true);

            _normalImage.FadeIn(0, 1f, 0.5f, 1f, () =>
             {
                 _normalImage.FadeIn(1, 0, 1, 0, () => { _normalDig.SetActive(false); });
             });
            StartCoroutine(NormalDialog());
        }
        private void ShowOkOrCancelDialog(string msg, DialogBackDelegate callBack)
        {
        }
        private void ShowOkDialog(string msg, DialogBackDelegate callBack)
        {
        }

        private void ShowTalkDialog(string msg, string headIcon, DialogBackDelegate callBack)
        {

            if (_callBack != callBack)
            {
                _callBack = callBack;
            }
            Sprite iconSp = null;
            if (!string.IsNullOrEmpty(headIcon))
            {
                iconSp = Resources.Load<Sprite>("Textures/NpcIcon/" + headIcon);
            }
            else
            {
                iconSp = null;
            }
            _talkContext.text = msg;
            if (iconSp == null)
            {
                _talkHeadIcon.gameObject.SetActive(false);
            }
            else
            {
                _talkHeadIcon.sprite = iconSp;
                _talkHeadIcon.gameObject.SetActive(true);
            }
            _talkDig.SetActive(true);
        }

        IEnumerator NormalDialog()
        {
            while (_normalDig.activeInHierarchy)
            {
                Color textColor = _normalText.color;
                Color imageColor = _normalImage.color;
                textColor.a = imageColor.a;
                _normalText.color = textColor;
                yield return null;
            }
        }
        #endregion
    }

}