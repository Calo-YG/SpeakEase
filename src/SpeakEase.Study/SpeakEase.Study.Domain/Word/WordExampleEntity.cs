using SpeakEase.Study.Domain.Shared;

namespace SpeakEase.Study.Domain.Word
{
    /// <summary>
    /// 单词例句实体类
    /// </summary>
    public class WordExampleEntity:Entity<string>,ICreation
    {
        /// <summary>
        /// 单词id
        /// </summary>
        public string WordId { get; set; }

        /// <summary>
        /// 英文例句
        /// </summary>
        public string En { get; set; }

        /// <summary>
        /// 中文翻译
        /// </summary>
        public string Cn { get; set; }

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
