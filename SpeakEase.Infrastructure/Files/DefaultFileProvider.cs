using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SpeakEase.Domain.Users;
using SpeakEase.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.Filters;
using SpeakEase.Infrastructure.SpeakEase.Core;

namespace SpeakEase.Infrastructure.Files
{
    /// <summary>
    /// 默认文件存储(本地文件存储)
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="options"></param>
    /// <param name="loggerFactory"></param>
    public class DefaultFileProvider(IDbContext dbContext,IOptions<FileOption> options,ILoggerFactory loggerFactory,IWebHostEnvironment webHostEnvironment):IFileProvider
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<IFileProvider>();

        private readonly FileOption _fileOption = options.Value;

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new NotImplementedException();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            throw new NotImplementedException();
        }

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

            var userrootpath = $"{user.Account}_{user.Name}";

            var currentpath = Path.Join(webHostEnvironment.ContentRootPath,rootPath,userrootpath,entity.Path);

            if (entity.Folder)
            {
                if (Directory.Exists(currentpath))
                {
                    Directory.CreateDirectory(currentpath);
                }
            }
            else
            {
                var key = LongToStringConverter.Convert(entity.Id);

                var storage = $"{entity.Name}_{key}.{entity.Type}";

                using var filestream = new FileStream(currentpath, FileMode.Create);

                await stream.CopyToAsync(filestream);
            }
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotImplementedException();
        }
    }
}
