using SpeakEase.Study.Domain.Shared;
using SpeakEase.Study.Domain.Shared.Enum;

namespace SpeakEase.Study.Domain.Article
{
    /// <summary>
    /// 文章实体表
    /// </summary>
    public class ArticleEntity:Entity<string>,ICreation
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章难度
        /// </summary>
        public LevelEnum Level { get; set; }

        /// <summary>
        /// 单词数量
        /// </summary>
        public int WordCount { get; set; }

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
