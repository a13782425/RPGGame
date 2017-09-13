using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGGame.Enums
{
    class MapEnum
    {
    }

    public enum PortalEnum
    {
        None = 0,
        /// <summary>
        /// 本场景传送
        /// </summary>
        ThisScene = 1,
        /// <summary>
        /// 其他场景传送
        /// </summary>
        OtherScene = 2,
    }
}
