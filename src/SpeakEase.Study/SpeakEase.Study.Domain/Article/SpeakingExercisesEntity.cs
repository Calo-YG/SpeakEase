using SpeakEase.Study.Domain.Shared;
using SpeakEase.Study.Domain.Shared.Enum;

namespace SpeakEase.Study.Domain.Article
{
    /// <summary>
    /// 口语练习实体
    /// </summary>
    public class SpeakingExercisesEntity:Entity<string>, ICreation  
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 联系描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 目标文本
        /// </summary>
        public string TargetText { get; set; }

        /// <summary>
        /// 难度等级
        /// </summary>
        public LevelEnum Level { get; set; }

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
