namespace SpeakEase.MQ.RabbitMQ;

/// <summary>
/// Message identification.<br />
/// 消息标识.
/// </summary>
public struct MessageHeader
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageHeader"/> struct.
    /// </summary>
    public MessageHeader()
    {
        Id = Guid.NewGuid().ToString("N");
        Timestamp = DateTimeOffset.Now;
    }

    /// <summary>
    /// Message id.
    /// </summary>
    public string Id { get; init; } = default!;

    /// <summary>
    /// The time when the message was created.<br />
    /// 消息被创建的时间.
    /// </summary>
    public DateTimeOffset Timestamp { get; init; } = default!;

    /// <summary>
    /// The message comes with an attribute, which for RabbitMQ is IBasicProperties.<br />
    /// 消息附带属性，对于 RabbitMQ 是 IBasicProperties.
    /// </summary>
    public object Properties { get; init; } = default!;

    /// <summary>
    /// The content format of the message,ex: "application/json".
    /// </summary>
    public string ContentType { get; init; } = default!;

    /// <summary>
    /// Encoding of message,ex: "UTF-8".
    /// </summary>
    public string ContentEncoding { get; init; } = default!;

    /// <summary>
    /// The message type,ex: "order".
    /// </summary>
    public string Type { get; init; } = default!;

    /// <summary>
    /// The message is sent by which user.<br />
    /// </summary>
    public string UserId { get; init; } = default!;

    /// <summary>
    /// The message is sent by which application.<br />
    /// </summary>
    public string AppId { get; init; } = default!;
}
