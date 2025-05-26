using SpeakEase.Study.Domain.Shared;

namespace SpeakEase.Study.Domain.SpeakExercises
{
    /// <summary>
    /// 用户口语尝试实体
    /// </summary>
    public class UserSpeakingAttemptEntity:Entity<string>,ICreation
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 练习id
        /// </summary>
        public string ExerciseId { get; set; }

        /// <summary>
        /// 录音url
        /// </summary>
        public string RecordingUrl { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 发音准确度分数
        /// </summary>
        public int PronunciationScore {get;set;}

        /// <summary>
        /// 流畅度分数
        /// </summary>
        public int FluencyScore {get;set;}
        
        /// <summary>
        /// 改进建议
        /// </summary>
        public int Feedback { get; set; }

        /// <summary>
        /// 尝试次数
        /// </summary>
        public int AttemptNumber { get; set; }

        /// <summary>
        /// 上次尝试时间
        /// </summary>
        public DateTime AttemptDate { get; set; }

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
