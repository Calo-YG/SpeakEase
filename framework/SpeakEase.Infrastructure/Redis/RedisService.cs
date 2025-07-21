using FreeRedis;
using Microsoft.Extensions.Options;
using SpeakEase.Infrastructure.Options;

namespace SpeakEase.Infrastructure.Redis
{
    /// <summary>
    /// 提供 Redis 操作服务
    /// </summary>
    public class RedisService:IRedisService
    {
        /// <summary>
        /// Redis 客户端实例
        /// </summary>
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

        /// <summary>
        /// 设置字符串值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireSeconds">过期时间（秒）</param>
        public void Set(string key, string value, int expireSeconds = 0)
        {
            if (expireSeconds > 0)
                _redis.Set(key, value, expireSeconds);
            else
                _redis.Set(key, value);
        }

        /// <summary>
        /// 设置字符串值，如果键不存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireSeconds">过期时间（秒），默认1000秒</param>
        /// <returns>是否成功设置</returns>
        public bool SetNx(string key,string value, int expireSeconds = 1000)
        {
              return  _redis.SetNx(key, value, expireSeconds);
        }

        /// <summary>
        /// 设置字符串值，如果键不存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireSeconds">过期时间（秒），默认1000秒</param>
        /// <returns>是否成功设置</returns>
        public Task<bool> SetNxAsync(string key, string value, int expireSeconds = 1000)
        {
            return _redis.SetNxAsync(key, value, expireSeconds);
        }

        /// <summary>
        /// 获取字符串值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public string Get(string key) => _redis.Get(key);

        /// <summary>
        /// 获取字符串值并反序列化为指定类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public T Get<T>(string key)=> _redis.Get<T>(key);

        // =================== Hash 操作 ===================

        /// <summary>
        /// 在 Hash 中设置字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        public void HashSet(string key, string field, string value) => _redis.HSet(key, field, value);

        /// <summary>
        /// 在 Hash 中获取字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns>值</returns>
        public string HashGet(string key, string field) => _redis.HGet(key, field);

        /// <summary>
        /// 获取 Hash 中所有字段和值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>字段和值的字典</returns>
        public Dictionary<string, string> HashGetAll(string key) => _redis.HGetAll(key);

        // =================== List 操作 ===================

        /// <summary>
        /// 在 List 的尾部添加值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void ListPush(string key, string value) => _redis.RPush(key, value);

        /// <summary>
        /// 从 List 的头部移除并返回值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public string ListPop(string key) => _redis.LPop(key);

        /// <summary>
        /// 获取 List 中指定范围的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="start">起始索引</param>
        /// <param name="stop">结束索引</param>
        /// <returns>值的数组</returns>
        public string[] ListRange(string key, int start, int stop) => _redis.LRange(key, start, stop);

        // =================== Set 操作 ===================

        /// <summary>
        /// 在 Set 中添加值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void SetAdd(string key, string value) => _redis.SAdd(key, value);

        /// <summary>
        /// 检查 Set 中是否存在值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否存在</returns>
        public bool SetExists(string key, string value) => _redis.SIsMember(key, value);

        /// <summary>
        /// 获取 Set 中的所有值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值的数组</returns>
        public string[] SetMembers(string key) => _redis.SMembers(key);

        // =================== Sorted Set 操作 ===================

        /// <summary>
        /// 在 Sorted Set 中添加值和分数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="score">分数</param>
        public void SortedSetAdd(string key, decimal value, string score) => _redis.ZAdd(key, value, score);

        /// <summary>
        /// 获取 Sorted Set 中指定范围的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="start">起始索引</param>
        /// <param name="stop">结束索引</param>
        /// <returns>值的数组</returns>
        public string[] SortedSetRange(string key, int start, int stop) => _redis.ZRange(key, start, stop);

        // =================== 事务操作 ===================



        // =================== 发布订阅 ===================

        /// <summary>
        /// 发布消息到频道
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="message">消息</param>
        public void Publish(string channel, string message) => _redis.Publish(channel, message);

        /// <summary>
        /// 订阅频道并处理消息
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="onMessage">消息处理回调</param>
        public void Subscribe(string channel, Action<string, object> onMessage)
        {
            _redis.Subscribe(channel, (ch, msg) => onMessage(ch, msg));
        }

        /// <summary>
        /// 异步设置字符串值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireSeconds">过期时间（秒）</param>
        /// <returns>任务</returns>
        public Task SetAsync(string key, string value, int expireSeconds = 0)
        {
            return _redis.SetAsync(key, value, expireSeconds);
        }

        /// <summary>
        /// 异步设置序列化值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireSeconds">过期时间（秒）</param>
        /// <returns>任务</returns>
        public Task SetAsync<T>(string key,T value,int expireSeconds)
        {
            return _redis.SetAsync<T>(key, value, expireSeconds);
        }

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key">键</param>
        public void Delete(string key)
        {
            _redis.Del(key);
        }

        /// <summary>
        /// 异步删除键
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>任务</returns>
        public Task DeleteAsync(string key)
        {
            return _redis.DelAsync(key);
        }
    }
}
