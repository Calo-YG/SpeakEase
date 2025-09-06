using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SpeakEase.MQ.Core;

namespace SpeakEase.MQ.RabbitMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    internal class ConnectionObject:IConnectionObject
    {
        protected Lazy<IConnection> _connection;
        protected Lazy<IChannel> _channel;
        private bool disposedValue;

        /// <summary>
        /// IConnection.
        /// </summary>
        public IConnection Connection => _connection.Value;

        /// <summary>
        /// IChannel.
        /// </summary>
        public IChannel DefaultChannel => _channel.Value;

        private readonly IConnectionFactory connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionObject"/> class.
        /// </summary>
        /// <param name="mqOptions"></param>
        public ConnectionObject(IOptions<MqOptions> options)
        {
            connectionFactory = new ConnectionFactory
            {
                HostName = options.Value.Host,
                Port = options.Value.Port,
                UserName = options.Value.UserName,
                Password = options.Value.Password,
                VirtualHost = options.Value.VirtualHost,
            };
            _connection = new Lazy<IConnection>(() => connectionFactory.CreateConnectionAsync().Result);
            _channel = new Lazy<IChannel>(() => _connection.Value.CreateChannelAsync().Result);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionObject"/> class.
        /// </summary>
        /// <param name="connectionObject"></param>
        protected ConnectionObject(ConnectionObject connectionObject)
        {
            _connection = connectionObject._connection;
            _channel = connectionObject._channel;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Connection.Dispose();
                }

                disposedValue = true;
            }
        }

        ~ConnectionObject()
        {
            Dispose(disposing: false);
        }
    }
}
