using RabbitMQ.Client;

namespace SpeakEase.MQ.RabbitMQ.Publisher
{
    /// <summary>
    /// 默认消息发布器
    /// </summary>
    public class DefaultMessagePublisher(): IChannelMessagePublisher, IMessagePublisher
    {
        public Task PublishAsync<TMessage>(string exchange, string routingKey, TMessage message, Action<BasicProperties> properties, CancellationToken cancellationToken = default) where TMessage : class
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync<TMessage>(string exchange, string routingKey, TMessage message, BasicProperties properties = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync<TMessage>(TMessage model, Action<BasicProperties> properties = null, CancellationToken cancellationToken = default) where TMessage : class
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync<TMessage>(TMessage message, BasicProperties properties = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task PublishChannelAsync<TMessage>(IChannel channel, string exchange, string routingKey, TMessage message, BasicProperties properties, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
