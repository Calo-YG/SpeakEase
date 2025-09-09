using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SpeakEase.MQ.Core;
using SpeakEase.MQ.RabbitMQ.Pool;

namespace SpeakEase.MQ.RabbitMQ.Publisher
{
    /// <summary>
    /// 默认消息发布器
    /// </summary>
    /// <param name="object">连接对象池</param>
    /// <param name="logger"></param>
    /// <param name="consumerTypeProvider"></param>
    public class DefaultMessagePublisher(IConnectionObject @object,ILogger<DefaultMessagePublisher> logger,IConsumerTypeProvider consumerTypeProvider): IChannelMessagePublisher, IMessagePublisher
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task PublishAsync<TMessage>(string exchange, string routingKey, TMessage message, Action<BasicProperties> properties, CancellationToken cancellationToken = default) where TMessage : class
        {
            var bytes = DefaultSearializer.Searializer(message);

            var propertiesModel = new BasicProperties();

            properties?.Invoke(propertiesModel);

            consumerTypeProvider.GetValue(typeof(TMessage).Name,out var options);


            return BasePublishAsync(@object.DefaultChannel,options.QueueName, exchange, routingKey, bytes, null, cancellationToken);
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

        /// <summary>
        /// 消息发布基础方法
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <param name="properties"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task BasePublishAsync(IChannel channel, string queue, string exchange, string routingKey, byte[] body, IBasicProperties properties, CancellationToken cancellationToken = default)
        {
           await  channel.QueueDeclareAsync(queue:);
        }
    }
}
