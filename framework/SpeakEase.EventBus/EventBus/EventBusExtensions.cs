using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpeakEase.EventBus.EventBus.BuildingBlock.Local.EventBus;
using SpeakEase.Infrastructure.EventBus.BuildingBlock.Local.EventBus;
using SpeakEase.Infrastructure.EventBus.BuildingBlock.Integration.EventBus;
using SpeakEase.Infrastructure.EventBus.Contrib.Local.EventBus;
using SpeakEase.Infrastructure.EventBus.Contrib.Integration.EventBus;

namespace SpeakEase.Infrastructure.EventBus;

public static class EventBusExtensions
{
    /// <summary>
    /// 注册本地异步事件
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterLocalEventBus(this IServiceCollection services,Action<EventHandlerStorage> action = null)
    {
        EventHandlerStorage storage = new EventHandlerStorage(services);

        if(action != null)
        {
            action(storage);
        }
        services.AddKeyedSingleton<IEventBus, LocalEventBus>("LocalEventBus");
        return services;
    }

    /// <summary>
    /// 注册RabbitMQ事件总线
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <param name="configureOptions">配置选项</param>
    /// <returns></returns>
    public static IServiceCollection RegisterRabbitMQEventBus(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<RabbitMQOptions>? configureOptions = null)
    {
        return services.AddRabbitMQEventBus(configuration, configureOptions);
    }

    /// <summary>
    /// 注册RabbitMQ事件总线（使用配置节）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <param name="sectionName">配置节名称</param>
    /// <returns></returns>
    public static IServiceCollection RegisterRabbitMQEventBus(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName)
    {
        return services.AddRabbitMQEventBus(configuration, sectionName);
    }
}