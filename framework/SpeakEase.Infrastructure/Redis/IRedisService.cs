namespace SpeakEase.Infrastructure.Redis
{
    /// <summary>
    /// 定义Redis服务接口，提供对Redis数据库的基本操作。
    /// </summary>
    public interface IRedisService
    {
        // String 操作
        /// <summary>
        /// 设置指定键的字符串值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expireSeconds">过期时间（秒），默认不过期。</param>
        void Set(string key, string value, int expireSeconds = 0);

        /// <summary>
        /// 异步设置指定键的字符串值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expireSeconds">过期时间（秒），默认不过期。</param>
        /// <returns>任务对象。</returns>
        Task SetAsync(string key, string value, int expireSeconds = 0);

        /// <summary>
        /// 异步设置指定键的序列化值。
        /// </summary>
        /// <typeparam name="T">值的类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expireSeconds">过期时间（秒）。</param>
        /// <returns>任务对象。</returns>
        Task SetAsync<T>(string key, T value, int expireSeconds);

        /// <summary>
        /// 删除指定键。
        /// </summary>
        /// <param name="key">键。</param>
        void Delete(string key);

        /// <summary>
        /// 异步删除指定键。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>任务对象。</returns>
        Task DeleteAsync(string key);

        /// <summary>
        /// 如果键不存在，则设置其字符串值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expireSeconds">过期时间（秒），默认1000秒。</param>
        /// <returns>如果设置成功，则返回true；否则返回false。</returns>
        bool SetNx(string key, string value, int expireSeconds = 1000);

        /// <summary>
        /// 如果键不存在，则设置其字符串值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expireSeconds">过期时间（秒），默认1000秒。</param>
        /// <returns>如果设置成功，则返回true；否则返回false。</returns>
        Task<bool> SetNxAsync(string key, string value, int expireSeconds = 1000);

        /// <summary>
        /// 获取指定键的字符串值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>值。</returns>
        string Get(string key);

        /// <summary>
        /// 获取指定键的序列化值。
        /// </summary>
        /// <typeparam name="T">值的类型。</typeparam>
        /// <param name="key">键。</param>
        /// <returns>值。</returns>
        T Get<T>(string key);

        // Hash 操作
        /// <summary>
        /// 在哈希表中设置指定字段的值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="field">字段。</param>
        /// <param name="value">值。</param>
        void HashSet(string key, string field, string value);

        /// <summary>
        /// 获取哈希表中指定字段的值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="field">字段。</param>
        /// <returns>值。</returns>
        string HashGet(string key, string field);

        /// <summary>
        /// 获取哈希表中所有字段和值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>包含所有字段和值的字典。</returns>
        Dictionary<string, string> HashGetAll(string key);

        // List 操作
        /// <summary>
        /// 在列表的尾部添加一个元素。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        void ListPush(string key, string value);

        /// <summary>
        /// 从列表的头部移除并返回一个元素。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>值。</returns>
        string ListPop(string key);

        /// <summary>
        /// 获取列表中指定范围的元素。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="start">起始索引。</param>
        /// <param name="stop">结束索引。</param>
        /// <returns>元素数组。</returns>
        string[] ListRange(string key, int start, int stop);

        // Set 操作
        /// <summary>
        /// 向集合中添加一个元素。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        void SetAdd(string key, string value);

        /// <summary>
        /// 判断集合中是否存在指定元素。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <returns>如果存在，则返回true；否则返回false。</returns>
        bool SetExists(string key, string value);

        /// <summary>
        /// 获取集合中的所有元素。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>元素数组。</returns>
        string[] SetMembers(string key);

        // Sorted Set 操作
        /// <summary>
        /// 向有序集合中添加一个元素及其分数。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="score">分数。</param>
        void SortedSetAdd(string key, decimal value, string score);

        /// <summary>
        /// 获取有序集合中指定范围的元素。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="start">起始索引。</param>
        /// <param name="stop">结束索引。</param>
        /// <returns>元素数组。</returns>
        string[] SortedSetRange(string key, int start, int stop);

        // 事务
        ///// <summary>
        ///// 执行一个事务，事务中的所有操作要么全部成功，要么全部失败。
        ///// </summary>
        ///// <param name="transaction">事务操作。</param>
        //void ExecuteTransaction(Action<RedisClientPipe> transaction);

        // 发布订阅
        /// <summary>
        /// 发布消息到指定频道。
        /// </summary>
        /// <param name="channel">频道。</param>
        /// <param name="message">消息。</param>
        void Publish(string channel, string message);

        /// <summary>
        /// 订阅指定频道的消息。
        /// </summary>
        /// <param name="channel">频道。</param>
        /// <param name="onMessage">接收到消息时的回调。</param>
        void Subscribe(string channel, Action<string, object> onMessage);

        /// <summary>
        /// 判断键是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
         bool Exists(string key);

        /// <summary>
        /// 判断键值是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);
    }
}
