using SpeakEase.Study.Domain.Shared;
using SpeakEase.Study.Domain.Shared.Enum;

namespace SpeakEase.Study.Domain.Word
{
    /// <summary>
    /// 词汇实体
    /// </summary>
    public class WordEntity : Entity<string>, ICreation, ISoftDeleted
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
        /// 等级难度
        /// </summary>
        public LevelEnum Level { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public string CreateById { get ; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 删除人id
        /// </summary>
        public string DeleteUserId { get; set; }

        /// <summary>
        /// 删除人名称
        /// </summary>
        public string DeleteUserName { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime DeleteTime { get; set; }
    }
}
