using SpeakEase.Contract.DictionaryWord.Dto;

namespace SpeakEase.Contract.DictionaryWord
{
    /// <summary>
    /// 词典服务接口
    /// </summary>
    public interface IDictionaryWordService
    {
        /// <summary>
        /// 获取单词
        /// </summary>
        /// <param name="id">单词</param>
        /// <returns></returns>
        Task<DictionaryWordResponse> GetWord(long id);
        /// <summary>
        /// 获取单词示例
        /// </summary>
        /// <param name="id">单词</param>
        /// <returns></returns>
        Task<List<WordExampleResponse>> GetWordExample(long id);
        /// <summary>
        /// 获取单词列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">页大小</param>
        /// <returns></returns>
        Task<List<DictionaryWordResponse>> GetWordList(int page, int size);
    }
}
