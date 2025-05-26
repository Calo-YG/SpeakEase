using SpeakEase.Study.Domain.Shared;

namespace SpeakEase.Study.Domain.Article
{
    /// <summary>
    /// 文章理解问题实体
    /// </summary>
    public class ArticleQuestionEntity:Entity<string>,ICreation
    {

        /// <summary>
        /// 文章id
        /// </summary>
        public string ArticleId { get; set; }

        /// <summary>
        /// 问题内容
        /// </summary>
        public string SubStance { get; set; }

        /// <summary>
        /// 正确答案
        /// </summary>
        public string CorrectAnswer { get; set; }

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
