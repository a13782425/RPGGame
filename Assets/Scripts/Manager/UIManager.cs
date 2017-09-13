using UnityEngine;
using System.Collections;
using RPGGame.UI;
using System.Collections.Generic;
using RPGGame.Enums;
using System;
using RPGGame.Global;

namespace RPGGame.Manager
{
    public sealed class UIManager : RPGGame.Tools.Singleton<UIManager>, IManager
    {
        #region 字段

        private MaskUIController _maskUI = null;
        /// <summary>
        /// 遮罩UI
        /// </summary>
        public MaskUIController MaskUI { get { return _maskUI; } }

        private DialogUIController _dialogUI = null;
        /// <summary>
        /// 对话框UI
        /// </summary>
        public DialogUIController DialogUI { get { return _dialogUI; } }

        private Canvas _mainCanvas = null;
        private Canvas _toolTipCanvas = null;
        private RectTransform _mainRoot = null;
        private RectTransform _toolTipRoot = null;

        private BaseUI _currentUI = null;
        /// <summary>
        /// 当前UI
        /// </summary>
        public BaseUI CurrentUI
        {
            get { return _currentUI; }
        }
        private BaseUI _beforeUI = null;

        private Dictionary<UIEnum, BaseUI> _currentUIDic;
        private Stack<BaseUI> _returnUiStack;

        #endregion
        /// <summary>
        /// 遮罩是否被激活
        /// </summary>
        public bool IsShowMask { get { return MaskUI.IsShowMask; } }
        /// <summary>
        /// 菜单UI是否被激活
        /// </summary>
        public bool IsMenuUIShow { get { return _returnUiStack.Count > 0; } }

        public bool Load()
        {
            _currentUIDic = new Dictionary<UIEnum, BaseUI>();
            _returnUiStack = new Stack<BaseUI>();
            CreateCanvas();
            CreateToolTipUI();

            return true;
        }

        public bool UnLoad()
        {
            return true;
        }

        public BaseUI GetBaseUI(UIEnum uiEnum)
        {
            if (!_currentUIDic.ContainsKey(uiEnum))
            {
                Debug.LogError(uiEnum.ToString() + "界面没有加载！");
                return null;
            }
            return _currentUIDic[uiEnum];
        }

        public BaseUI LoadUI(UIEnum uiEnum, UITypeEnum uiTypeEnum = UITypeEnum.Main)
        {
            if (_currentUIDic.ContainsKey(uiEnum))
            {
                Debug.LogError(uiEnum.ToString() + "已经打开！");
                return null;
            }
            GameObject obj = Resources.Load<GameObject>("Prefabs/UI/" + Global.GlobalData.UiPathDic[uiEnum]);
            GameObject uiObj = Instantiate(obj);
            uiObj.transform.localScale = Vector3.one;
            switch (uiTypeEnum)
            {
                case UITypeEnum.ToolTip:
                    uiObj.transform.SetParent(_toolTipRoot);
                    (uiObj.transform as RectTransform).SetAsFirstSibling();
                    break;
                case UITypeEnum.Main:
                default:
                    uiObj.transform.SetParent(_mainRoot);
                    break;
            }
            BaseUI baseUI = uiObj.GetComponent<BaseUI>();
            baseUI.Init();
            _currentUIDic.Add(uiEnum, baseUI);
            return baseUI;
        }

        /// <summary>
        /// 显示UI
        /// </summary>
        /// <param name="uiEnum"></param>
        public void ShowUI(UIEnum uiEnum)
        {
            Global.GlobalData.IsCanMove = false;
            BaseUI showUI = null;
            if (_currentUIDic.ContainsKey(uiEnum))
            {
                showUI = _currentUIDic[uiEnum];
            }
            else
            {
                showUI = LoadUI(uiEnum);
            }
            showUI.Show();
            if (_currentUI == null)
            {
                _currentUI = showUI;
            }
            else
            {
                _beforeUI = _currentUI;
                _currentUI = showUI;
            }
            if (_beforeUI != null)
            {
                _returnUiStack.Push(_beforeUI);
            }
            //_returnUiStack.Push(_currentUI);
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        /// <param name="uiEnum"></param>
        public void HideUI(UIEnum uiEnum)
        {
            BaseUI hideUI = null;
            if (_currentUIDic.ContainsKey(uiEnum))
            {
                hideUI = _currentUIDic[uiEnum];
            }
            hideUI.Hide();
        }

        /// <summary>
        /// 返回UI（主UI应用）
        /// </summary>
        /// <returns></returns>
        public bool ReturnUI()
        {
            if (_returnUiStack.Count > 0)
            {
                this.CurrentUI.Hide();
                this._currentUI = _returnUiStack.Pop();
                return true;
            }
            else
            {
                if (this.CurrentUI != null)
                {
                    this.CurrentUI.Hide();
                }
                this._currentUI = null;
                Global.GlobalData.IsCanMove = true;
            }
            return false;
        }

        #region 对话框

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="msg"></param>
        public void ShowDialog(string msg)
        {
            ShowDialog(msg, null);
        }
        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="msg"></param>
        public void ShowDialog(string msg, string headIcon)
        {
            ShowDialog(msg, headIcon, DialogEnum.Normal);
        }
        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="msg"></param>
        public void ShowDialog(string msg, string headIcon, DialogEnum dialogEnum)
        {
            ShowDialog(msg, headIcon, dialogEnum, null);
        }
        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="msg"></param>
        public void ShowDialog(string msg, string headIcon, DialogEnum dialogEnum, DialogBackDelegate callBack)
        {
            DialogUI.ShowDialog(msg, headIcon, dialogEnum, callBack);
        }

        public void HideDialog()
        {
            DialogUI.HideDialog();
        }

        #endregion
        /// <summary>
        /// 显示遮罩
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="atorCallBack"></param>
        /// <param name="maskEnum"></param>
        public void ShowMask(Action callBack = null, IEnumerator atorCallBack = null, RPGGame.UI.MaskUIController.MaskEnum maskEnum = RPGGame.UI.MaskUIController.MaskEnum.None)
        {
            MaskUI.ShowMask(callBack, atorCallBack, maskEnum);
        }

        /// <summary>
        /// 隐藏遮罩
        /// </summary>
        public void HideMask()
        {
            MaskUI.HideMask();
        }

        private void CreateCanvas()
        {
            if (_mainCanvas == null)
            {
                GameObject mcObj = Resources.Load<GameObject>("Prefabs/UI/Canvas/MainCanvas");
                _mainCanvas = Instantiate(mcObj).GetComponent<Canvas>();
                _mainCanvas.transform.SetParent(this.transform);
                _mainRoot = _mainCanvas.transform.Find("Root").GetComponent<RectTransform>();
                _mainCanvas.sortingOrder = 10;
            }
            if (_toolTipCanvas == null)
            {
                GameObject tcObj = Resources.Load<GameObject>("Prefabs/UI/Canvas/ToolTipCanvas");
                _toolTipCanvas = Instantiate(tcObj).GetComponent<Canvas>();
                _toolTipCanvas.transform.SetParent(this.transform);
                _toolTipRoot = _toolTipCanvas.transform.Find("Root").GetComponent<RectTransform>();
                _toolTipCanvas.sortingOrder = 100;
            }
        }

        private void CreateToolTipUI()
        {
            _dialogUI = (DialogUIController)LoadUI(UIEnum.DialogUI, UITypeEnum.ToolTip);
            _maskUI = (MaskUIController)LoadUI(UIEnum.MaskUI, UITypeEnum.ToolTip);
        }


        public void OnUpdate()
        {
        }
    }
}
