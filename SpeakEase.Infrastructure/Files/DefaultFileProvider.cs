using IdGen;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpeakEase.Domain.Users;
using SpeakEase.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.Shared;

namespace SpeakEase.Infrastructure.Files
{
    /// <summary>
    /// 默认文件存储(本地文件存储)
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="idgenerator"></param>
    /// <param name="hostEnvironment"></param>
    /// <param name="options"></param>
    /// <param name="loggerFactory"></param>
    public class DefaultFileProvider(IDbContext dbContext,IdGenerator idgenerator,IWebHostEnvironment hostEnvironment,IOptions<FileOption> options,ILoggerFactory loggerFactory):IFileProvider
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<IFileProvider>();

        private readonly FileOption _fileOption = options.Value;

        /// <summary>
        /// 文件存储
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="size"></param>
        /// <param name="filename"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public  async Task SaveAsync(FileEntity entity,Stream stream)
        {
            //文件存储根目录
            var rootPath = _fileOption.RootPath;

            //设置用户存储路径
            var user = dbContext.GetUser();

            var userrootpath = "{}";

            var currentpath = Path.Join(rootPath,userrootpath,entity.Path);

            await Task.CompletedTask;
        }
    }
}
