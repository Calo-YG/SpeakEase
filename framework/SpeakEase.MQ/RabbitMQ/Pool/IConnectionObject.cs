using RabbitMQ.Client;

namespace SpeakEase.MQ.RabbitMQ.Pool
{
    public interface IConnectionObject
    {
        /// <summary>
        /// IConnection.
        /// </summary>
        IConnection Connection { get; }

        /// <summary>
        /// IChannel.
        /// </summary>
        IChannel DefaultChannel { get; }

        /// <summary>
        /// 创建一个新的通道.
        /// </summary>
        /// <returns></returns>
        Task<IChannel> CreateChannelAsync();
    }

    /// <summary>
    /// IConnection,IChannel pool.<br />
    /// TCP 连接和通道.
    /// </summary>
    public interface IDisposeConnectionObject : IConnectionObject, IDisposable
    {
    }
}
