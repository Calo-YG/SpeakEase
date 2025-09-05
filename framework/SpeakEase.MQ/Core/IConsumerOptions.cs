namespace SpeakEase.MQ.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConsumerOptions
    {
        /// <summary>
        /// 是否自动创建队列
        /// </summary>
        bool AutoQueueDeclare { get; set; }

        /// <summary>
        /// 是否启用消费失败消息回滚
        /// </summary>
        bool UseRollBack { get; set; }

        /// <summary>
        /// 交换机名称
        /// </summary>
        string ExchangeName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        string QueueName { get; set; }

        /// <summary>
        /// 交换机类型
        /// </summary>
        string ExchangeType { get; set; }

        /// <summary>
        /// 交换机路由key
        /// </summary>
        string RoutingKey { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        int Expiration { get; set; }

        /// <summary>
        /// 重试失败队列
        /// </summary>
        bool RetryFaildRequeue { get; set; }

        /// <summary>
        /// 死信交换机
        /// </summary>
        string DeadExchange { get; set; }

        /// <summary>
        /// 死信路由key
        /// </summary>
        string DeadRoutingKey { get; set; }
    }
}
