using SpeakEase.Study.Domain.Shared;

namespace SpeakEase.Study.Domain.Word
{
    /// <summary>
    /// 单词翻译实体
    /// </summary>
    public class WordTranslationEntity : Entity<string>, ICreation
    {
        /// <summary>
        /// 单词id
        /// </summary>
        public string WordId { get; set; }

        /// <summary>
        /// 词性
        /// </summary>
        public string Pos { get; set; }

        /// <summary>
        /// 释义
        /// </summary>
        public string Meaning { get; set; }
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

        protected WordTranslationEntity() { }

        public WordTranslationEntity(string id, string wordId, string pos, string meaning)
        {
            WordId = wordId;
            Pos = pos;
            Meaning = meaning;
        }
    }
}
