// SpeakEase项目中用于扩展Entity Framework Core的DbContext注册功能的静态类

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;

public static class DbContextExtensions
{
    /// <summary>
    /// 注册Entity Framework Core的DbContext。
    /// </summary>
    /// <param name="serviceCollection">IServiceCollection实例，用于注册服务。</param>
    /// <param name="configuration">IConfiguration实例，用于获取配置信息。</param>
    /// <returns>IServiceCollection实例，用于进一步注册服务。</returns>
    public static IServiceCollection RegisterEntityFrameworkCoreContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // 设置AppContext开关，以启用Npgsql的遗留时间戳行为
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        // 设置AppContext开关，以禁用DateTime的无穷大转换
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

        // 添加DbContext到服务集合中，指定为Scoped生命周期
        serviceCollection.AddDbContext<IDbContext, SpeakEaseGatewayContext>((builder) =>
        {
            // 使用Npgsql作为数据库提供程序，并从配置中获取连接字符串
            builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), options =>
            {
                // 此处可根据需要配置Npgsql选项
            });
        }, contextLifetime: ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Scoped);

        // 返回服务集合，以便进一步配置
        return serviceCollection;
    }
}