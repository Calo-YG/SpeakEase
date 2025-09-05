namespace SpeakEase.MQ.Core
{
    public class ConsumerMessage : IConsumerMessage
    {
        /// <summary>
        /// 唯一id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
    }
}
