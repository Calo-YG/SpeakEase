using System.Text.Json;

namespace SpeakEase.MQ.Core
{

    /// <summary>
    /// 默认序列化
    /// </summary>
    public class DefaultSearializer
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        /// <summary>
        /// 默认序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] Searializer<T>(T obj) where T : class
        {
            if (obj == null)
            {
                return Array.Empty<byte>();
            }

            return JsonSerializer.SerializeToUtf8Bytes(obj,options);
        }

        /// <summary>
        /// 默认反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T Desearializer<T>(byte[] bytes) where T : class
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(bytes,options);
        }
    }
}
