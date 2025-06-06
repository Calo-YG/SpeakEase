﻿using SpeakEase.Study.Domain.Shared;
using SpeakEase.Study.Domain.Shared.Enum;

namespace SpeakEase.Study.Domain.SpeakExercises
{
    /// <summary>
    /// 口语练习实体类
    /// </summary>
    public class SpeakingExerciseEntity:Entity<string>,ICreation
    {

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 练习描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 目标文本
        /// </summary>
        public string TargetText { get; set; }

        /// <summary>
        /// 难度级别
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
