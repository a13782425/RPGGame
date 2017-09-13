using UnityEngine;
using System.Collections;
using RPGGame.Enums;

namespace RPGGame.Manager
{
    public sealed class InputManager : RPGGame.Tools.Singleton<InputManager>, IManager
    {
        public delegate void EventOnButtonClick(ButtonEnum buttonEnum, params object[] data);

        public delegate void EventOnAxis(ButtonEnum buttonEnum, float val);

        /// <summary>
        /// 移动回调
        /// </summary>
        public EventOnAxis OnPlayerMove;
        /// <summary>
        /// 确定按钮回调
        /// </summary>
        public EventOnButtonClick OnOkButtonClick;
        /// <summary>
        /// 取消按钮回调
        /// </summary>
        public EventOnButtonClick OnCancelButtonClick;


        private bool _isClick = true;
        /// <summary>
        /// 是否可以点击
        /// </summary>
        public bool IsClick
        {
            get { return _isClick; }
            set { _isClick = value; }
        }

        /// <summary>
        /// 移动回调执行
        /// </summary>
        /// <param name="buttonEnum"></param>
        /// <param name="val"></param>
        public void OnPlayerMoveClick(ButtonEnum buttonEnum, float val)
        {
            if (Global.GlobalData.IsCanMove)
            {
                if (OnPlayerMove != null)
                {
                    OnPlayerMove(buttonEnum, val);
                }
            }
        }


        public bool Load()
        {
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