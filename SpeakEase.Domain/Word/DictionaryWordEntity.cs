using SpeakEase.Domain.Shared;
using SpeakEase.Domain.Word.Enum;

namespace SpeakEase.Domain.Word
{
    /// <summary>
    /// 词典单词实体类
    /// </summary>
    public class DictionaryWordEntity:ICreation,IModify
    {
        /// <summary>
        ///  // 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 单词
        /// </summary>
        public string Word { get; set; }  // 单词

        /// <summary>
        /// 音标
        /// </summary>
        public string Phonetic { get; set; }  // 音标

        /// <summary>
        /// 释义（中文）
        /// </summary>
        public string ChineseDefinition { get; set; }

        /// <summary>
        /// 例句
        /// </summary>
        public string ExampleSentence { get; set; }

        /// <summary>
        /// 收藏数量
        /// </summary>
        public long CollectCount { get; set; }

        /// <summary>
        /// 词汇等级 2/4/6/8 级
        /// </summary>
        public WordLevel Level { get; set; }

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

        protected DictionaryWordEntity() { }

        public DictionaryWordEntity(string word, string phonetic,string chineseDefinition, string exampleSentence, WordLevel level)
        {
            Word = word;
            Phonetic = phonetic;
            ChineseDefinition = chineseDefinition;
            ExampleSentence = exampleSentence;
            Level = level;
        }
    }

}
