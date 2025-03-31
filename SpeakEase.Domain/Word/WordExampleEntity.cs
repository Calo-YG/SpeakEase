using SpeakEase.Domain.Shared;

namespace SpeakEase.Domain.Word
{
    public class WordExampleEntity:Entity<long>,ICreation,IModify
    {
        /// <summary>
        /// 单词id
        /// </summary>
        public long WordId { get; set; }
        /// <summary>
        /// 例句
        /// </summary>
        public string Sentence { get; set; }

        /// <summary>
        /// 释义
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// 解析说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long CreateById { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string CreateByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public long ModifyById { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string ModifyByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime ModifyAt { get; set; }
    }
}
