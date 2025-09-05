namespace SpeakEase.MQ.Core
{
    /// <summary>
    /// 消息
    /// </summary>
    public interface IConsumerMessage
    {
        /// <summary>
        /// 唯一id
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        DateTimeOffset Timestamp { get; set; }
    }
}
