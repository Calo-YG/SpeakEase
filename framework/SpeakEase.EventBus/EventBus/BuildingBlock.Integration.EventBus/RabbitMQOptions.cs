namespace SpeakEase.Infrastructure.EventBus.BuildingBlock.Integration.EventBus;

/// <summary>
/// RabbitMQ配置选项
/// </summary>
public class RabbitMQOptions
{
    /// <summary>
    /// RabbitMQ服务器主机名
    /// </summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>
    /// RabbitMQ服务器端口
    /// </summary>
    public int Port { get; set; } = 5672;

    /// <summary>
    /// RabbitMQ用户名
    /// </summary>
    public string UserName { get; set; } = "guest";

    /// <summary>
    /// RabbitMQ密码
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// RabbitMQ虚拟主机
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// 连接超时时间（秒）
    /// </summary>
    public int RequestedConnectionTimeout { get; set; } = 30;

    /// <summary>
    /// 心跳间隔（秒）
    /// </summary>
    public int RequestedHeartbeat { get; set; } = 60;

    /// <summary>
    /// 是否启用自动重连
    /// </summary>
    public bool AutomaticRecoveryEnabled { get; set; } = true;

    /// <summary>
    /// 网络恢复间隔（秒）
    /// </summary>
    public int NetworkRecoveryInterval { get; set; } = 10;

    /// <summary>
    /// 默认交换机名称
    /// </summary>
    public string DefaultExchange { get; set; } = "speakease.events";

    /// <summary>
    /// 默认队列名称前缀
    /// </summary>
    public string DefaultQueuePrefix { get; set; } = "speakease.queue.";

    /// <summary>
    /// 是否启用消息持久化
    /// </summary>
    public bool MessagePersistent { get; set; } = true;

    /// <summary>
    /// 是否启用发布确认
    /// </summary>
    public bool PublisherConfirms { get; set; } = true;

    /// <summary>
    /// 消费者预取数量
    /// </summary>
    public ushort ConsumerPrefetchCount { get; set; } = 1;

    /// <summary>
    /// 最大重试次数
    /// </summary>
    public int MaxRetryCount { get; set; } = 3;

    /// <summary>
    /// 重试间隔（毫秒）
    /// </summary>
    public int RetryIntervalMs { get; set; } = 1000;
} 