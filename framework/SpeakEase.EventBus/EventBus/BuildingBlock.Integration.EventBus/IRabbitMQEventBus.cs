using SpeakEase.EventBus.EventBus.BuildingBlock.Local.EventBus;

namespace SpeakEase.Infrastructure.EventBus.BuildingBlock.Integration.EventBus;

/// <summary>
/// RabbitMQ事件总线接口
/// </summary>
public interface IRabbitMQEventBus
{
    /// <summary>
    /// 发布事件到指定队列
    /// </summary>
    /// <typeparam name="TEvent">事件类型</typeparam>
    /// <param name="event">事件对象</param>
    /// <param name="routingKey">路由键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    /// <summary>
    /// 发布事件到默认交换机
    /// </summary>
    /// <typeparam name="TEvent">事件类型，必须实现IEvent接口</typeparam>
    /// <param name="event">要发布的事件对象</param>
    /// <param name="routingKey">路由键，用于消息路由</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>发布任务</returns>
    Task PublishAsync<TEvent>(TEvent @event, string routingKey = "", CancellationToken cancellationToken = default) 
        where TEvent : class, IEvent;

    /// <summary>
    /// 发布事件到指定交换机
    /// </summary>
    /// <typeparam name="TEvent">事件类型</typeparam>
    /// <param name="event">事件对象</param>
    /// <param name="exchange">交换机名称</param>
    /// <param name="routingKey">路由键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    /// <summary>
    /// 发布事件到指定交换机
    /// </summary>
    /// <typeparam name="TEvent">事件类型，必须实现IEvent接口</typeparam>
    /// <param name="event">要发布的事件对象</param>
    /// <param name="exchange">交换机名称</param>
    /// <param name="routingKey">路由键，用于消息路由</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>发布任务</returns>
    Task PublishToExchangeAsync<TEvent>(TEvent @event, string exchange, string routingKey = "", CancellationToken cancellationToken = default) 
        where TEvent : class, IEvent;

    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <typeparam name="TEvent">事件类型</typeparam>
    /// <typeparam name="THandler">事件处理器类型</typeparam>
    /// <param name="queueName">队列名称</param>
    /// <param name="routingKey">路由键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    /// <summary>
    /// 订阅事件到指定队列
    /// </summary>
    /// <typeparam name="TEvent">事件类型，必须实现IEvent接口</typeparam>
    /// <typeparam name="THandler">事件处理器类型，必须实现IRabbitMQEventHandler接口</typeparam>
    /// <param name="queueName">队列名称</param>
    /// <param name="routingKey">路由键，用于消息过滤</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>订阅任务</returns>
    Task SubscribeAsync<TEvent, THandler>(string queueName, string routingKey = "", CancellationToken cancellationToken = default) 
        where TEvent : class, IEvent 
        where THandler : class, IRabbitMQEventHandler<TEvent>;

    /// <summary>
    /// 启动事件总线
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    /// <summary>
    /// 启动事件总线
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>启动任务</returns>
    Task StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 停止事件总线
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    /// <summary>
    /// 停止事件总线
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>停止任务</returns>
    Task StopAsync(CancellationToken cancellationToken = default);
} 