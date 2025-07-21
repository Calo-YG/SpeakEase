using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpeakEase.Infrastructure.Options;

namespace SpeakEase.Infrastructure.Redis;

public static class RedisExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration,string section="RedisOptions")
    { 
        services.Configure<RedisOptions>(configuration.GetSection(section));
        services.AddSingleton<IRedisService, RedisService>();
        
        return services;
    }
}