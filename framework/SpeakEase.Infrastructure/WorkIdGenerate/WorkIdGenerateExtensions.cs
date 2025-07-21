using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SpeakEase.Infrastructure.WorkIdGenerate;

public static class WorkIdGenerateExtensions
{
    /// <summary>
    /// 添加Id生成器
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="section"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IServiceCollection AddIdGenerate(this IServiceCollection services, IConfiguration configuration,string section="WorkIdGenerateOptions")
    {
        var options = configuration.GetSection(section).Get<WorkIdGenerateOptions>();

        if (options.WorkerIdBitLength > 19)
        {
            throw new ArgumentException("WorkerIdBitLength 最大支持19位");
        }

        if (string.IsNullOrEmpty(options.AppName))
        {
            throw new ArgumentException("AppName 不能为空");    
        }

        if (string.IsNullOrEmpty(options.RedisKeyPrefix))
        {
            options.RedisKeyPrefix = "WorkIdGenerate:SpeakEase";
        }
        
        services.Configure<WorkIdGenerateOptions>(configuration.GetSection(section));
        services.AddSingleton<IWorkIdGenerate, WorkIdGenerate>();
        services.AddSingleton<IIdGenerate, IdGenerate>();
        services.AddHostedService(sp => sp.GetRequiredService<IWorkIdGenerate>() as WorkIdGenerate);
        return services;
    }
}