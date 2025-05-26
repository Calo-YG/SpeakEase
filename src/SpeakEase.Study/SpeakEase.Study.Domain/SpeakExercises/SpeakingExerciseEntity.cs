using SpeakEase.Study.Domain.Shared;

namespace SpeakEase.Study.Domain.SpeakExercises
{
    /// <summary>
    /// 口语练习实体类
    /// </summary>
    public class SpeakingExerciseEntity:Entity<string>,ICreation
    {
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
