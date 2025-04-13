using System.ComponentModel;


namespace SpeakEase.Domain.Friend.Enum
{
    /// <summary>
    /// 好友状态枚举
    /// </summary>
    [Description("好友状态枚举")]
    public enum FriendStatus
    {
        /// <summary>
        /// 待确认（好友请求）
        /// </summary>
        [Description("待确认（好友请求）")]
        Pending = 0,
        /// <summary>
        /// 已接受
        /// </summary>
        [Description("已接受")]
        Accepted = 1,
        /// <summary>
        /// 拒绝
        /// </summary>
        [Description("拒绝")]
        Rejected = 2,
        /// <summary>
        /// 屏蔽
        /// </summary>
        [Description("屏蔽")]
        Blocked = 3 
    }
}
