using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RPGGame.Actor
{
    public class PlayerInfo
    {

        public List<RoleInfo> RoleList = new List<RoleInfo>();

        public int X = 76; //24.5f;

        public int Y = 75; //-23.9f;// -24f;

        public int Z = 0;

        public string Name = "扯淡";

        /// <summary>
        /// 金币
        /// </summary>
        public int Money = 0;

        /// <summary>
        /// 厨师升级
        /// </summary>
        public int CookLvl = 0;
        /// <summary>
        /// 厨师经验
        /// </summary>
        public int CookExp = 0;
        /// <summary>
        /// 铁匠等级
        /// </summary>
        public int BlacksmithLvl = 0;
        /// <summary>
        /// 铁匠经验
        /// </summary>
        public int BlacksmithExp = 0;
        /// <summary>
        /// 药师等级
        /// </summary>
        public int PharmacistLvl = 0;
        /// <summary>
        /// 药师经验
        /// </summary>
        public int PharmacistExp = 0;
    }
}

