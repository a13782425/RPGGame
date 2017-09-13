using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGGame.Enums
{
    enum DeBuffEnum
    {
        None = 1,
        /// <summary>
        /// 减少血
        /// </summary>
        ReduceHp,
        /// <summary>
        /// 减血
        /// </summary>
        ReplyReduceHp,
        /// <summary>
        /// 减少蓝
        /// </summary>
        ReduceMp,
        /// <summary>
        /// 减血
        /// </summary>
        ReplyReduceMp,
        /// <summary>
        /// 减少臂力
        /// </summary>
        ReduceMuscle,
        /// <summary>
        /// 减臂力
        /// </summary>
        ReplyReduceMuscle,
        /// <summary>
        /// 减少智力
        /// </summary>
        ReduceIntell,
        /// <summary>
        /// 减智力
        /// </summary>
        ReplyReduceIntell,
        /// <summary>
        /// 减少体魄
        /// </summary>
        ReducePhysical,
        /// <summary>
        /// 减体魄
        /// </summary>
        ReplyReducePhysical,
        /// <summary>
        /// 减少身法
        /// </summary>
        ReduceAgility,
        /// <summary>
        /// 减身法
        /// </summary>
        ReplyReduceAgility,
        /// <summary>
        /// 减少罡气
        /// </summary>
        ReduceGangAir,
        /// <summary>
        /// 减罡气
        /// </summary>
        ReplyReduceGangAir
    }
}
