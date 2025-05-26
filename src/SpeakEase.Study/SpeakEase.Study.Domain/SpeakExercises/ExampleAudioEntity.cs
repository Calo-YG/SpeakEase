using SpeakEase.Study.Domain.Shared;

namespace SpeakEase.Study.Domain.SpeakExercises
{
    /// <summary>
    /// 示范音频实体
    /// </summary>
    public class ExampleAudioEntity:Entity<string>,ICreation
    {
        /// <summary>
        /// 联系id
        /// </summary>
        public string ExerciseId { get; set; }

        /// <summary>
        /// 音频url
        /// </summary>
        public string AudioUrl { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string Substance { get; set; }

        /// <summary>
        /// 音标
        /// </summary>
        public string Phonetic { get; set; }

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
