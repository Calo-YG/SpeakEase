using Microsoft.Extensions.DependencyInjection;
using SpeakEase.Infrastructure.EventBus.BuildingBlock.Local.EventBus;
using SpeakEase.Infrastructure.EventBus.BuildingBlockEventBus;
using SpeakEase.Infrastructure.EventBus.Contrib.Local.EventBus;

namespace SpeakEase.Infrastructure.EventBus;

public static class EventBusExtensions
{
    /// <summary>
    /// 注册本地异步事件
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterLocalEventBus(this IServiceCollection services)
    {
        services.AddKeyedSingleton<IEventBus, LocalEventBus>("LocalEventBus");
        return services;
    }
}