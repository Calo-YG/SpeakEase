using SpeakEase.Domain.Shared;

namespace SpeakEase.Domain.Word
{
    public class WordEntity :Entity<long>, ICreation
    {
        /// <summary>
        /// 单词
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// 音标
        /// </summary>
        public string Phonetic { get; set; }

        /// <summary>
        /// 释义
        /// </summary>
        public LevelEnum Level { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public long CreateById { get; set; }
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
