using SpeakEase.MQ.Core;

namespace SpeakEase.MQ.RabbitMQ
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConsumerAttribute : Attribute, IConsumerOptions
    {
        /// <summary>
        /// 是否自动创建队列
        /// </summary>
        public bool AutoQueueDeclare { get; set; }

        /// <summary>
        /// 是否启用消费失败消息回滚
        /// </summary>
        public bool UseRollBack { get; set; }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 交换机类型
        /// </summary>
        public string ExchangeType { get; set; }

        /// <summary>
        /// 交换机路由key
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public int Expiration { get; set; }

        /// <summary>
        /// 重试失败队列
        /// </summary>
        public bool RetryFaildRequeue { get; set; }

        /// <summary>
        /// 死信交换机
        /// </summary>
        public string DeadExchange { get; set; }

        /// <summary>
        /// 死信路由key
        /// </summary>
        public string DeadRoutingKey { get; set; }
    }
}
