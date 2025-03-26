using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace SpeakEase.Infrastructure.EntityFrameworkCore
{
    internal class SpeakEaseDesignFactory: IDesignTimeDbContextFactory<SpeakEaseContext>
    {
        public SpeakEaseContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "SpeakEase");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                // 从 appsettings.json 文件加载配置
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                // 也可以添加其他配置源，例如环境变量
                .AddEnvironmentVariables()
                // 或者命令行参数
                //.AddCommandLine(args)
                .Build();

            var options = new OptionsWrapper<DbContextOptions<SpeakEaseContext>>(new DbContextOptionsBuilder<SpeakEaseContext>()
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .Options);

            return new SpeakEaseContext(options.Value);
        }
    }
}
