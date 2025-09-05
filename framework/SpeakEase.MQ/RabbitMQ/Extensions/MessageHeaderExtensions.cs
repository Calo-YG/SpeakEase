using RabbitMQ.Client;


namespace SpeakEase.MQ.RabbitMQ.Extensions
{
    public static class MessageHeaderExtensions
    {
        /// <summary>
        /// Get message header from <see cref="IReadOnlyBasicProperties"/>.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns><see cref="MessageHeader"/>.</returns>
        public static MessageHeader GetMessageHeader(this IReadOnlyBasicProperties properties)
        {
            var header = new MessageHeader
            {
                Id = properties.MessageId ?? string.Empty,
                Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(properties.Timestamp.UnixTime),
                ContentType = properties.ContentType ?? string.Empty,
                ContentEncoding = properties.ContentEncoding ?? string.Empty,
                Type = properties.Type ?? string.Empty,
                UserId = properties.UserId ?? string.Empty,
                AppId = properties.AppId ?? string.Empty,
                Properties = properties
            };
            return header;
        }
    }
}
