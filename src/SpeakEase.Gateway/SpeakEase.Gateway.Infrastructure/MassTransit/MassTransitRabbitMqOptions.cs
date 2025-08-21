namespace SpeakEase.Gateway.Infrastructure.MassTransit
{
    /// <summary>
    /// MassTransit RabbitMQ options
    /// </summary>
    public class MassTransitRabbitMqOptions
    {
        /// <summary>
        /// RabbitMQ Host
        /// </summary>
        public string Host { get; set; }
        
        /// <summary>
        /// RabbitMQ Port
        /// </summary>
        public ushort Port { get; set; }
        
        /// <summary>
        /// RabbitMQ UserName
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// RabbitMQ Password
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// RabbitMQ VirtualHost
        /// </summary>
        public string VirtualHost { get; set; }
        
        /// <summary>
        /// RabbitMQ ConnectionName
        /// </summary>
        public string ConnectionName { get; set; }
    }
}
