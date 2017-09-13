using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPGGame.GameWorld.Buff;

namespace RPGGame.Actor
{
    public class RoleInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name = string.Empty;
        /// <summary>
        /// 身上的所有Buff
        /// </summary>
        public List<BuffBase> CurrentBuff = new List<BuffBase>();

        /// <summary>
        /// 生命值
        /// </summary>
        public int Hp = 1;

        /// <summary>
        /// 魔法值
        /// </summary>
        public int Mp = 1;

        /// <summary>
        /// 臂力
        /// </summary>
        public int Muscle = 1;
        /// <summary>
        /// 智力
        /// </summary>
        public int Intell = 1;
        /// <summary>
        /// 体魄
        /// </summary>
        public int Physical = 1;
        /// <summary>
        /// 身法
        /// </summary>
        public int Agility = 1;
        /// <summary>
        /// 罡气
        /// </summary>
        public int GangAir = 1;

        /// <summary>
        /// 饱食度
        /// </summary>
        public int FeedDegree = 100;

    }
}