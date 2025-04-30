using SpeakEase.Contract.DictionaryWord;
using SpeakEase.Contract.DictionaryWord.Dto;
using SpeakEase.Infrastructure.EntityFrameworkCore;

namespace SpeakEase.Services
{
    /// <summary>
    /// 词典服务
    /// </summary>
    /// <param name="context"></param>
    /// <param name="loggerFactory"></param>
    public sealed class DictionaryWordService(IDbContext context,ILoggerFactory loggerFactory) : IDictionaryWordService
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger logger = loggerFactory.CreateLogger<DictionaryWordService>();


        /// <summary>
        /// 获取单词
        /// </summary>
        /// <param name="id">单词</param>
        /// <returns></returns>
        public Task<DictionaryWordResponse> GetWord(long id)
        {
            var ctx = context;
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取单词示例
        /// </summary>
        /// <param name="id">单词</param>
        /// <returns></returns>
        public Task<List<WordExampleResponse>> GetWordExample(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取单词列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">页大小</param>
        /// <returns></returns>
        public Task<List<DictionaryWordResponse>> GetWordList(int page, int size)
        {
            throw new NotImplementedException();
        }
    }
}
