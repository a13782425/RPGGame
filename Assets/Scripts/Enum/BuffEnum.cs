using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGGame.Enums
{
    /// <summary>
    /// buff枚举：
    /// 1-2500为回合前增益buff
    /// 2501-5000为回合后增益buff
    /// 5001-7500为回合前减益buff
    /// 7501-10000为回合后减益buff
    /// </summary>
    public enum BuffEnum : int
    {
        None = 0,
        /// <summary>
        /// 增加臂力
        /// </summary>
        AddMuscle = 1,
        /// <summary>
        /// 加臂力
        /// </summary>
        ReplyAddMuscle,
        /// <summary>
        /// 增加智力
        /// </summary>
        AddIntell,
        /// <summary>
        /// 加智力
        /// </summary>
        ReplyAddIntell,
        /// <summary>
        /// 增加体魄
        /// </summary>
        AddPhysical,
        /// <summary>
        /// 加体魄
        /// </summary>
        ReplyAddPhysical,
        /// <summary>
        /// 增加身法
        /// </summary>
        AddAgility,
        /// <summary>
        /// 加身法
        /// </summary>
        ReplyAddAgility,
        /// <summary>
        /// 增加罡气
        /// </summary>
        AddGangAir,
        /// <summary>
        /// 加罡气
        /// </summary>
        ReplyAddGangAir,
        /// <summary>
        /// 增加血
        /// </summary>
        AddHp = 2501,
        /// <summary>
        /// 加血
        /// </summary>
        ReplyAddHp,
        /// <summary>
        /// 增加蓝
        /// </summary>
        AddMp,
        /// <summary>
        /// 加血
        /// </summary>
        ReplyAddMp,




        /// <summary>
        /// 减少血
        /// </summary>
        ReduceHp = 5000,
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
        ReduceMuscle = 10000,
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
        ReplyReduceGangAir,
    }
}
