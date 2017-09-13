using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPGGame.Enums;
using RPGGame.GameWorld.Buff;

namespace RPGGame.WuShu
{
    public class WuShuBase
    {
        /// <summary>
        /// 消耗类型
        /// </summary>
        public ConsumeEnum CurrentConsumeEnum { get; set; }
        /// <summary>
        /// 消耗数量
        /// </summary>
        public int ConsumeCount { get; set; }

        public List<BuffBase> BuffList { get; set; }

        public int Id;

        public int Level = 0;

    }
}
