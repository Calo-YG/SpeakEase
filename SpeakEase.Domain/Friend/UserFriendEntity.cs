using System.ComponentModel.DataAnnotations.Schema;
using SpeakEase.Domain.Friend.Enum;


namespace SpeakEase.Domain.Friend
{
    [Table("user_friend")]
    public class UserFriendEntity:Entity<long>
    {
        /// <summary>
        /// 发出好友请求的用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 发出好友请求的用户备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 目标用户ID
        /// </summary>
        public long FriendUserId { get; set; }

        /// <summary>
        /// 目标用户ID 好友备注
        /// </summary>
        public string FriendUserRemark { get; set; }

        /// <summary>
        /// 好友状态：0 待确认，1 已成为好友，2 拒绝，3 屏蔽
        /// </summary>
        public FriendStatus Status { get; set; }

        /// <summary>
        /// 请求创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最后更新时间，自动更新
        /// </summary>
        public DateTime ModifyAt { get; set; }
    }
}
