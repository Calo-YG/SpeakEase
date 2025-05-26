using SpeakEase.Study.Domain.Shared;

namespace SpeakEase.Study.Domain.Article
{
    /// <summary>
    /// 用户阅读进度实体
    /// </summary>
    public class UserReadingProgressEntity : Entity<string>, ICreation
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 文章id
        /// </summary>
        public string ArticleId { get; set; }
        /// <summary>
        /// 阅读进度百分比
        /// </summary>
        public decimal ProgressPercentage { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 上次阅读时间
        /// </summary>
        public DateTime? LastReadAt { get; set; }
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
