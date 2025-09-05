using RabbitMQ.Client;

namespace SpeakEase.MQ.Core
{
    public class MqOptions
    {
        /// <summary>
        /// App name.
        /// </summary>
        public string AppName { get; init; } = null!;

        /// <summary>
        /// Create queues on startup,see <see cref="RabbitMQ.Client.IChannel.QueueDeclareAsync"/>.<br />
        /// 是否自动创建队列.
        /// </summary>
        public bool AutoQueueDeclare { get; init; } = true;

        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string VirtualHost { get; set; } = "/";
    }
}
