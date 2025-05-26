using SpeakEase.Study.Domain.Shared;

namespace SpeakEase.Study.Domain.Article
{
    /// <summary>
    /// 问题选项实体
    /// </summary>
    public class QuestionOptionEntity:Entity<string>,ICreation
    {
        /// <summary>
        /// 问题id
        /// </summary>
        public string QuestionId { get; set; }
        /// <summary>
        /// 选项内容
        /// </summary>
        public string SubStance { get; set; }
        /// <summary>
        /// 是否正确答案
        /// </summary>
        public bool IsCorrect { get; set; }
        /// <summary>
        /// 创建人id
        /// </summary>
        public string CreateById { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateByName { get; set; }
        /// <summary>
        /// 创建时��
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
