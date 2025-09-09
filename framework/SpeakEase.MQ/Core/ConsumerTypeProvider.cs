using SpeakEase.MQ.RabbitMQ;

namespace SpeakEase.MQ.Core
{
    public class ConsumerTypeProvider : Dictionary<string,ConsumerOptions> ,IConsumerTypeProvider
    {

        /// <summary>
        /// 设置key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetItem(string key, ConsumerOptions value)
        {
            if(ContainsKey(key))
            {
                this[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }

        /// <summary>
        /// 获取key对应的value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void GetValue(string key, out ConsumerOptions value)
        {
            TryGetValue(key, out value);
        }
    }
}
