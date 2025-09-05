using RabbitMQ.Client;

namespace SpeakEase.MQ.RabbitMQ.Publisher
{
    internal interface IChannelMessagePublisher
    {
        /// <summary>
        /// Publish messagge.<br />
        /// 发布消息.
        /// </summary>
        /// <typeparam name="TMessage">Event model.<br />事件模型类.</typeparam>
        /// <param name="channel"></param>
        /// <param name="exchange">Exchange name.<br />交换器名称.</param>
        /// <param name="routingKey">Queue name.<br />队列名称.</param>
        /// <param name="message">Event object.<br />事件对象.</param>
        /// <param name="properties"><see href="https://rabbitmq.github.io/rabbitmq-dotnet-client/api/RabbitMQ.Client.IBasicProperties.html"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Task"/>.</returns>
        Task PublishChannelAsync<TMessage>(IChannel channel, string exchange, string routingKey, TMessage message, BasicProperties properties, CancellationToken cancellationToken = default);
    }
}
