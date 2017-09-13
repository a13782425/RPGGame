using UnityEngine;
using System.Collections;
using System;
using RPGGame.Utils;
using RPGGame.Manager;
using RPGGame.Enums;
using UnityEngine.UI;

namespace RPGGame.UI
{
    public sealed class MaskUIController : BaseUI
    {
        public override UIEnum CurrentEnum
        {
            get { return UIEnum.MaskUI; }
        }
        public override UITypeEnum CurrentTypeEnum
        {
            get
            {
                return UITypeEnum.ToolTip;
            }
        }
        public enum MaskEnum
        {
            None = 0,
            Scale = 1,
            FadeIn,
            Filled
        }

        private AutoFindWidget _autoFindWidget = null;

        private MaskEnum _currentMask = MaskEnum.None;

        private Transform _scaleMask = null;
        private Transform _moveMask = null;
        private Image _fadeInMask = null;
        private Image _filledMask = null;

        public bool IsShowMask { get { return _currentMask != MaskEnum.None; } }

        public override void Init()
        {
            _autoFindWidget = AutoFindWidget.MyGet(this.gameObject);
            _scaleMask = _autoFindWidget.MyGetOneGameObject("ScaleMask").transform;
            _moveMask = _autoFindWidget.MyGetOneGameObject("MoveMask").transform;
            _fadeInMask = _autoFindWidget.MyGetComponentByName<Image>("FadeInMask");
            _filledMask = _autoFindWidget.MyGetComponentByName<Image>("FilledMask");
            _scaleMask.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Global.GlobalData.ScreenWidth + 60, Global.GlobalData.ScreenHeight + 60);
            _moveMask.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Global.GlobalData.ScreenWidth + 60, Global.GlobalData.ScreenHeight + 60);
            _scaleMask.gameObject.SetActive(false);
            _moveMask.gameObject.SetActive(false);
            _fadeInMask.gameObject.SetActive(false);
            _filledMask.gameObject.SetActive(false);
        }

        public void ShowMask(Action callBack = null, IEnumerator atorCallBack = null, MaskEnum maskEnum = MaskEnum.None)
        {
            if (_currentMask != MaskEnum.None)
            {
                return;
            }
            if (maskEnum == MaskEnum.None)
            {
                Array maskArray = Enum.GetValues(typeof(MaskEnum));
                int maskIndex = UnityEngine.Random.Range(1, maskArray.Length);
                _currentMask = (MaskEnum)maskArray.GetValue(maskIndex);
            }
            else
            {
                _currentMask = maskEnum;
            }
            InputManager.Instance.IsClick = false;
            Debug.LogError("显示遮罩类型：" + _currentMask);
            switch (_currentMask)
            {
                case MaskEnum.FadeIn:
                    _fadeInMask.gameObject.SetActive(true);
                    _fadeInMask.FadeIn(0, 1, 1, 0.5f, callBack, atorCallBack);
                    break;
                case MaskEnum.Scale:
                    _scaleMask.gameObject.SetActive(true);
                    _scaleMask.ScaleIn(Vector3.zero, Vector3.one, 1, 0.5f, callBack, atorCallBack);
                    break;
                case MaskEnum.Filled:
                    _filledMask.gameObject.SetActive(true);
                    SelectFilled(callBack, atorCallBack);
                    break;
                default:
                    break;
            }
        }

        public void HideMask()
        {
            switch (_currentMask)
            {
                case MaskEnum.FadeIn:
                    _fadeInMask.FadeIn(1, 0, 1, 0f, () => { _fadeInMask.gameObject.SetActive(false); });
                    break;
                case MaskEnum.Scale:
                    _scaleMask.ScaleIn(Vector3.one, Vector3.zero, 1, 0f, () => { _scaleMask.gameObject.SetActive(false); });
                    break;
                case MaskEnum.Filled:
                    _filledMask.FilledIn(1, 0, 1, 0f, () => { _filledMask.gameObject.SetActive(false); });
                    break;
                default:
                    break;
            }
            _currentMask = MaskEnum.None;
            InputManager.Instance.IsClick = true;
        }

        private void SelectFilled(Action callBack, IEnumerator atorCallBack)
        {
            Array fillArray = Enum.GetValues(typeof(Image.FillMethod));
            int fillIndex = UnityEngine.Random.Range(1, fillArray.Length);
            _filledMask.fillMethod = (Image.FillMethod)fillArray.GetValue(fillIndex);
            Debug.LogError("FillMethod：" + _filledMask.fillMethod);
            int origin = 0;
            switch (_filledMask.fillMethod)
            {
                case Image.FillMethod.Radial180:
                case Image.FillMethod.Radial360:
                case Image.FillMethod.Radial90:
                    origin = UnityEngine.Random.Range(0, 5);
                    break;
                case Image.FillMethod.Horizontal:
                case Image.FillMethod.Vertical:
                default:
                    origin = UnityEngine.Random.Range(0, 2);
                    break;
            }
            _filledMask.FilledIn(0, 1, 1, 0.5f, callBack, atorCallBack);
        }
    }
}


#region 保留代码

//                   case MaskEnum.Move:
//                   {
//                       _moveMask.gameObject.SetActive(true);
//                       if (dirEnum == DirEnum.None)
//                       {
//                           Array dirArray = Enum.GetValues(typeof(DirEnum));
//                           int dirIndex = UnityEngine.Random.Range(1, dirArray.Length);
//                           _currentDir = (DirEnum)dirArray.GetValue(dirIndex);
//                       }
//                       else
//                       {
//                           _currentDir = dirEnum;
//                       }
//                       int height = Global.GlobalData.ScreenHeight;
//                       int wight = Global.GlobalData.ScreenWidth;
//                       float beginX = 0;
//                       float beginY = 0;
//                       float endX = 0;
//                       float endY = 0;
//                       switch (_currentDir)
//                       {
//                           case DirEnum.Up:
//                               beginY = 0 - height - 100;
//                               //endY = height + 100;
//                               break;
//                           case DirEnum.Down:
//                               beginY = height + 100;
//                               //endY = 0 - height - 100;
//                               break;
//                           case DirEnum.Left:
//                               beginX = 0 - wight - 100;
//                               //endY = wight + 100;
//                               break;
//                           case DirEnum.Right:
//                               beginX = wight + 100;
//                               //endY = 0 - wight - 100;
//                               break;
//                           case DirEnum.None:
//                           default:
//                               break;
//                       }
//                       _moveMask.MoveIn(new Vector3(beginX, beginY), new Vector3(endX, endY), 1, 0.5f);
//                   }
//                   break;

//                  case MaskEnum.Move:
//                    {
//                        int height = Global.GlobalData.ScreenHeight;
//                        int wight = Global.GlobalData.ScreenWidth;
//                        float beginX = 0;
//                        float beginY = 0;
//                        float endX = 0;
//                        float endY = 0;
//                        switch (_currentDir)
//                        {
//                            case DirEnum.Up:
//                                endY = 0 - height - 100;
//                                break;
//                            case DirEnum.Down:
//                                endY = height + 100;
//                                break;
//                            case DirEnum.Left:
//                                endX = 0 - wight - 100;
//                                break;
//                            case DirEnum.Right:
//                                endX = wight + 100;
//                                break;
//                            case DirEnum.None:
//                            default:
//                                break;
//                        }
//                        _moveMask.MoveIn(new Vector3(beginX, beginY), new Vector3(endX, endY), 1, 0, () => { _moveMask.gameObject.SetActive(false); });
//                    }
//                    break;

//public enum DirEnum
//{
//    None = 0,
//    Up = 1,
//    Down,
//    Left,
//    Right
//}

#endregion