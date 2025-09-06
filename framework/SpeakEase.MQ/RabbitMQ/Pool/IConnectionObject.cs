using RabbitMQ.Client;

namespace SpeakEase.MQ.RabbitMQ.Pool
{
    internal interface IConnectionObject
    {
        /// <summary>
        /// IConnection.
        /// </summary>
        IConnection Connection { get; }

        /// <summary>
        /// IChannel.
        /// </summary>
        IChannel DefaultChannel { get; }
    }

    /// <summary>
    /// IConnection,IChannel pool.<br />
    /// TCP 连接和通道.
    /// </summary>
    internal interface IDisposeConnectionObject : IConnectionObject, IDisposable
    {
    }
}
