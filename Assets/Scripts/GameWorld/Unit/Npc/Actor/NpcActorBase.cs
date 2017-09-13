using RPGGame.Enums;
using RPGGame.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.GameWorld.Unit.Npc
{
    public abstract class NpcActorBase : MonoBehaviour
    {

        private NpcController _currentNpcController = null;

        public NpcController CurrentNpcController
        {
            get { return _currentNpcController; }
        }

        public NpcStatus CurrentStatus { get { return CurrentNpcController == null ? null : CurrentNpcController.CurrentStatus; } }

        /// <summary>
        /// Npc的表
        /// </summary>
        public NpcTable CurrentTable { get { return CurrentNpcController == null ? null : CurrentNpcController.CurrentTable; } }

        protected bool _isTakeEnd = false;
        /// <summary>
        /// 分部执行是否执行完毕
        /// </summary>
        public bool IsTakeEnd { get { return _isTakeEnd; } }

        #region 抽象方法和属性

        /// <summary>
        /// 该控制器是什么Npc
        /// </summary>
        public abstract NpcTypeEnum NpcType { get; }

        #endregion


        #region 虚方法

        public virtual void Init(NpcController controller)
        {
            this._currentNpcController = controller;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public virtual bool TakeUp()
        {
            return true;
        }

        /// <summary>
        /// 当玩家第一次进入
        /// </summary>
        public virtual void OnPlayerEnter() { }
        /// <summary>
        /// 当玩家第一次退出
        /// </summary>
        public virtual void OnPlayerExit() { _isTakeEnd = false; }
        /// <summary>
        /// 重置
        /// </summary>
        public virtual void OnReset() { _isTakeEnd = false; }
        //public virtual void ButtonClick(Enums.ButtonEnum buttonEnum, params object[] data)
        //{ }
        #endregion

    }
}
