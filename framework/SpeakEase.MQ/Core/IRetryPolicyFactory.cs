using Polly.Retry;

namespace SpeakEase.MQ.Core;

public interface IRetryPolicyFactory
{
    /// <summary>
    /// Create retry policy.<br />
    /// 创建策略.
    /// </summary>
    /// <param name="queue">Queue name.<br />队列名称.</param>
    /// <param name="id">Event id.</param>
    /// <returns><see cref="Task{AsyncRetryPolicy}"/>.</returns>
    Task<AsyncRetryPolicy> CreatePolicy(string queue, string id);
}