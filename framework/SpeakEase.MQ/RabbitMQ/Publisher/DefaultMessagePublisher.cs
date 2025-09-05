using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakEase.MQ.RabbitMQ.Publisher
{
    public class DefaultMessagePublisher : IChannelMessagePublisher, IMessagePublisher
    {
        public Task CustomPublishAsync<TMessage>(string exchange, string routingKey, TMessage message, BasicProperties properties = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

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
