using SpeakEase.Study.Domain.Shared;

namespace SpeakEase.Study.Domain.Word
{
    /// <summary>
    /// 用户词汇实体
    /// </summary>
    public class UserWordEntity:Entity<string>,ICreation
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 词汇id
        /// </summary>
        public string WordId { get; set; }

        /// <summary>
        /// 是否掌握
        /// </summary>
        public bool IsMastered { get; set; }

        /// <summary>
        /// 上次复习时间
        /// </summary>
        public DateTime? LastReviewAt { get; set; }

        /// <summary>
        /// 复习次数
        /// </summary>
        public int ReviewCount { get; set; }

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

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateAt { get; set; }
    }
}
