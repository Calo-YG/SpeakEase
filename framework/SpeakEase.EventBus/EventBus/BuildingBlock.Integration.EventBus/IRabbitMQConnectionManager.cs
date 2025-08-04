using RabbitMQ.Client;

namespace SpeakEase.Infrastructure.EventBus.BuildingBlock.Integration.EventBus;

/// <summary>
/// RabbitMQ连接管理器接口
/// </summary>
public interface IRabbitMQConnectionManager
{
    /// <summary>
    /// 获取连接
    /// </summary>
    /// <returns></returns>
    /// <summary>
    /// 获取RabbitMQ连接
    /// </summary>
    /// <returns>RabbitMQ连接对象</returns>
    IConnection GetConnection();

    /// <summary>
    /// 获取通道
    /// </summary>
    /// <returns></returns>
    /// <summary>
    /// 从通道池获取通道
    /// </summary>
    /// <returns>RabbitMQ通道对象</returns>
    IModel GetChannel();

    /// <summary>
    /// 创建通道
    /// </summary>
    /// <returns></returns>
    /// <summary>
    /// 创建新的通道
    /// </summary>
    /// <returns>新创建的RabbitMQ通道对象</returns>
    IModel CreateChannel();

    /// <summary>
    /// 释放通道
    /// </summary>
    /// <param name="channel">通道</param>
    /// <summary>
    /// 释放通道
    /// </summary>
    /// <param name="channel">要释放的通道</param>
    void ReleaseChannel(IModel channel);

    /// <summary>
    /// 连接是否可用
    /// </summary>
    /// <summary>
    /// 连接是否可用
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// 连接事件
    /// </summary>
    /// <summary>
    /// 连接状态变化事件
    /// </summary>
    event EventHandler<bool> ConnectionChanged;
} 