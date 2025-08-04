using SpeakEase.EventBus.EventBus.BuildingBlock.Local.EventBus;

namespace SpeakEase.Infrastructure.EventBus.BuildingBlock.Integration.EventBus;

/// <summary>
/// RabbitMQ事件处理器接口
/// </summary>
/// <typeparam name="TEvent">事件类型</typeparam>
public interface IRabbitMQEventHandler<in TEvent> where TEvent : class, IEvent
{
    /// <summary>
    /// 处理事件
    /// </summary>
    /// <param name="event">事件对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    /// <summary>
    /// 处理事件
    /// </summary>
    /// <param name="event">要处理的事件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>处理任务</returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
} 