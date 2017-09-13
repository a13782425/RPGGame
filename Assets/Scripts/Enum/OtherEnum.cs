using RPGGame.Attr;

namespace RPGGame.Enums
{
    /// <summary>
    /// 场景
    /// </summary>
    public enum SceneEnum
    {
        Startup = 0,
        LoadScene = 1,
        HomeScene = 2,
        MainScene = 3,
        None = 99999
    }

    /// <summary>
    /// 消耗枚举
    /// </summary>
    public enum ConsumeEnum
    {
        None = 0,
        Mp = 1,
        Hp = 2,
        Money = 3
    }

    /// <summary>
    /// UI功能枚举
    /// </summary>
    public enum UIEnum
    {
        MaskUI,
        DialogUI,
        MoveUI,
        OperationUI,
        MenuUI,
        SettingUI,
        DialogBoxUI
    }

    /// <summary>
    /// UI类型枚举
    /// </summary>
    public enum UITypeEnum
    {
        Main,

        ToolTip
    }

    /// <summary>
    /// 按钮事件
    /// </summary>
    public enum ButtonEnum
    {
        X,
        Y,
        Cancel,
        OK
    }
    /// <summary>
    /// 游戏消息
    /// </summary>
    public enum GameMessageEnum
    {
        None
    }

    public enum DialogEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 对话
        /// </summary>
        Talk,
        /// <summary>
        /// 带有确定的
        /// </summary>
        WithOk,
        /// <summary>
        /// 带有确定和取消的
        /// </summary>
        WithOkOrCancel,
        /// <summary>
        /// 提示
        /// </summary>
        Tip
    }

    public enum NpcTypeEnum
    {
        None = 0,
        /// <summary>
        /// 正常对话NPC
        /// </summary>
        [BGameEditor("对话NPC")]
        Normal = 1,
        /// <summary>
        /// 商人
        /// </summary>
        [BGameEditor("商人")]
        Business = 2,
        /// <summary>
        /// 任务
        /// </summary>
        [BGameEditor("任务")]
        Task = 4,
        /// <summary>
        /// 制造商
        /// </summary>
        [BGameEditor("制造商")]
        Manufacture = 8,
        /// <summary>
        /// 老师（教学）
        /// </summary>
        [BGameEditor("教学")]
        Teacher = 16,
        /// <summary>
        /// 切磋（切磋NPC）
        /// </summary>
        [BGameEditor("切磋")]
        Exercise = 32

    }


    public enum WeatherEnum
    {
        [BGameEditor("随机")]
        None = 0,
        [BGameEditor("晴天")]
        Sunny = 1,
        [BGameEditor("下雨")]
        Rain = 2,
        [BGameEditor("下雪")]
        Snow = 3
    }

}
