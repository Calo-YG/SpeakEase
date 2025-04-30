using FreeRedis;
using Microsoft.Extensions.Options;
using SpeakEase.Infrastructure.Options;

namespace SpeakEase.Infrastructure.Redis
{
    public class RedisService:IRedisService
    {
        private readonly RedisClient _redis;

        /// <summary>
        /// 通过 DI 注入 Redis 连接配置
        /// </summary>
        /// <param name="options">Redis 连接配置</param>
        public RedisService(IOptions<RedisOptions> options)
        {
            _redis = new RedisClient(options.Value.ConnectionString);
        }
        // =================== String 操作 ===================
        public void Set(string key, string value, int expireSeconds = 0)
        {
            if (expireSeconds > 0)
                _redis.Set(key, value, expireSeconds);
            else
                _redis.Set(key, value);
        }

        public bool SetNx(string key,string value, int expireSeconds = 1000)
        {
              return  _redis.SetNx(key, value, expireSeconds);
        }

        public string Get(string key) => _redis.Get(key);

        public T Get<T>(string key)=> _redis.Get<T>(key);

        // =================== Hash 操作 ===================

        public void HashSet(string key, string field, string value) => _redis.HSet(key, field, value);
        public string HashGet(string key, string field) => _redis.HGet(key, field);
        public Dictionary<string, string> HashGetAll(string key) => _redis.HGetAll(key);

        // =================== List 操作 ===================

        public void ListPush(string key, string value) => _redis.RPush(key, value);
        public string ListPop(string key) => _redis.LPop(key);
        public string[] ListRange(string key, int start, int stop) => _redis.LRange(key, start, stop);

        // =================== Set 操作 ===================

        public void SetAdd(string key, string value) => _redis.SAdd(key, value);
        public bool SetExists(string key, string value) => _redis.SIsMember(key, value);
        public string[] SetMembers(string key) => _redis.SMembers(key);

        // =================== Sorted Set 操作 ===================

        public void SortedSetAdd(string key, decimal value, string score) => _redis.ZAdd(key, value, score);
        public string[] SortedSetRange(string key, int start, int stop) => _redis.ZRange(key, start, stop);

        // =================== 事务操作 ===================



        // =================== 发布订阅 ===================

        public void Publish(string channel, string message) => _redis.Publish(channel, message);
        public void Subscribe(string channel, Action<string, object> onMessage)
        {
            _redis.Subscribe(channel, (ch, msg) => onMessage(ch, msg));
        }

        public Task SetAsync(string key, string value, int expireSeconds = 0)
        {
            return _redis.SetAsync(key, value, expireSeconds);
        }

        public Task SetAsync<T>(string key,T value,int expireSeconds)
        {
            return _redis.SetAsync<T>(key, value, expireSeconds);
        }

        public void Delete(string key)
        {
            _redis.Del(key);
        }

        public Task DeleteAsync(string key)
        {
            return _redis.DelAsync(key);
        }
    }
}
