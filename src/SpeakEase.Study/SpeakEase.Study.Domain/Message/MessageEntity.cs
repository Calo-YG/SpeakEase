using SpeakEase.Study.Domain.Message.Enum;
using SpeakEase.Study.Domain.Shared;

namespace SpeakEase.Study.Domain.Message
{
    /// <summary>
    /// 消息通知实体
    /// </summary>
    public class MessageEntity:Entity<string>,ICreation
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Substance {  get; set; }

        /// <summary>
        /// 消息枚举类型
        /// </summary>
        public MessageEnum MessageEnum { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public string CreateById { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
