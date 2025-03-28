namespace SpeakEase.Infrastructure.Redis
{
    public interface IRedisService
    {
        // String 操作
        void Set(string key, string value, int expireSeconds = 0);

        Task SetAsync(string key, string value, int expireSeconds = 0);

        void Delete(string key);

        Task DeleteAsync(string key);

        bool SetNx(string key, string value, int expireSeconds = 1000);
        string Get(string key);

        T Get<T>(string key);

        // Hash 操作
        void HashSet(string key, string field, string value);
        string HashGet(string key, string field);
        Dictionary<string, string> HashGetAll(string key);

        // List 操作
        void ListPush(string key, string value);
        string ListPop(string key);
        string[] ListRange(string key, int start, int stop);

        // Set 操作
        void SetAdd(string key, string value);
        bool SetExists(string key, string value);
        string[] SetMembers(string key);

        // Sorted Set 操作
        void SortedSetAdd(string key, decimal value, string score);
        string[] SortedSetRange(string key, int start, int stop);

        // 事务
        //void ExecuteTransaction(Action<RedisClientPipe> transaction);

        // 发布订阅
        void Publish(string channel, string message);
        void Subscribe(string channel, Action<string, object> onMessage);
    }
}
