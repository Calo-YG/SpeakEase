using SpeakEase.MQ.RabbitMQ;

namespace SpeakEase.MQ.Core
{
    /// <summary>
    /// 消息类型提供器
    /// </summary>
    public interface IConsumerTypeProvider:IDictionary<string, ConsumerOptions>
    {
        /// <summary>
        /// 设置key,value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void  SetItem(string key, ConsumerOptions value);

        /// <summary>
        /// 设置key对应的value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void GetValue(string key, out ConsumerOptions value);
    }
}
