using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SpeakEase.Infrastructure.EventBus.BuildingBlock.Integration.EventBus;

namespace SpeakEase.Infrastructure.EventBus.Contrib.Integration.EventBus;

/// <summary>
/// RabbitMQ事件总线扩展方法
/// </summary>
public static class RabbitMQEventBusExtensions
{
    /// <summary>
    /// 注册RabbitMQ事件总线
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <param name="configureOptions">配置选项</param>
    /// <returns></returns>
    /// <summary>
    /// 注册RabbitMQ事件总线服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置对象</param>
    /// <param name="configureOptions">可选的配置选项委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddRabbitMQEventBus(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<RabbitMQOptions>? configureOptions = null)
    {
        // 配置选项
        services.Configure<RabbitMQOptions>(configuration.GetSection("RabbitMQ"));
        
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        // 注册连接管理器
        services.AddSingleton<IRabbitMQConnectionManager, RabbitMQConnectionManager>();

        // 注册事件总线
        services.AddSingleton<IRabbitMQEventBus, RabbitMQEventBus>();

        // 注册后台服务
        services.AddHostedService<RabbitMQEventBusHostedService>();

        return services;
    }

    /// <summary>
    /// 注册RabbitMQ事件总线（使用配置节）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <param name="sectionName">配置节名称</param>
    /// <returns></returns>
    /// <summary>
    /// 注册RabbitMQ事件总线服务（使用指定配置节）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置对象</param>
    /// <param name="sectionName">配置节名称</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddRabbitMQEventBus(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName)
    {
        return services.AddRabbitMQEventBus(configuration, options =>
        {
            configuration.GetSection(sectionName).Bind(options);
        });
    }

    /// <summary>
    /// 注册事件处理器
    /// </summary>
    /// <typeparam name="TEvent">事件类型</typeparam>
    /// <typeparam name="THandler">处理器类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <returns></returns>
    /// <summary>
    /// 注册事件处理器（作用域生命周期）
    /// </summary>
    /// <typeparam name="TEvent">事件类型</typeparam>
    /// <typeparam name="THandler">处理器类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddEventHandler<TEvent, THandler>(this IServiceCollection services)
        where TEvent : class, IEvent
        where THandler : class, IRabbitMQEventHandler<TEvent>
    {
        services.AddScoped<IRabbitMQEventHandler<TEvent>, THandler>();
        return services;
    }

    /// <summary>
    /// 注册事件处理器（单例）
    /// </summary>
    /// <typeparam name="TEvent">事件类型</typeparam>
    /// <typeparam name="THandler">处理器类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <returns></returns>
    /// <summary>
    /// 注册事件处理器（单例生命周期）
    /// </summary>
    /// <typeparam name="TEvent">事件类型</typeparam>
    /// <typeparam name="THandler">处理器类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddEventHandlerSingleton<TEvent, THandler>(this IServiceCollection services)
        where TEvent : class, IEvent
        where THandler : class, IRabbitMQEventHandler<TEvent>
    {
        services.AddSingleton<IRabbitMQEventHandler<TEvent>, THandler>();
        return services;
    }
} 