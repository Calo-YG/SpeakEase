using System.ComponentModel;

namespace SpeakEase.Domain.Word.Enum
{
    /// <summary>
    /// 单词等级
    /// </summary>
    [Description("单词等级")]
    public enum WordLevel
    {
        /// <summary>
        /// 二级
        /// </summary>
        [Description("二级")]
        Second = 2,

        [Description("四级")]
        Four = 4,

        [Description("六级")]
        Six = 6,

        [Description("八级")]
        Eight = 8
    }

    public static class WordLevelExtension
    {
        public static IReadOnlyDictionary<WordLevel, string> KeyValues = new Dictionary<WordLevel, string>
        {
            { WordLevel.Second,"八级" },
            { WordLevel.Four,"四级" },
            { WordLevel.Six,"六级" },

            { WordLevel.Eight,"八级" }
        };


        public static string GetWordLevel(this WordLevel wordLevel) => KeyValues[wordLevel];
    }
}
