using SpeakEase.Study.Domain.Shared;

namespace SpeakEase.Study.Domain.Article
{
    /// <summary>
    /// 文章段落实体类
    /// </summary>
    public class ParagraphEntity:Entity<string>,ICreation
    {

        /// <summary>
        /// 文章id
        /// </summary>
        public string ArticleId { get; set; }

        /// <summary>
        /// 英文内容
        /// </summary>
        public string SubStance { get; set; }

        /// <summary>
        /// 中文翻译
        /// </summary>
        public string Translation { get; set; }

        /// <summary>
        /// 段落顺序索引
        /// </summary>
        public int OrderIndex { get; set; }

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
