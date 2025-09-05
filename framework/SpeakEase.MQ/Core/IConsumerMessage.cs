namespace SpeakEase.MQ.Core;

/// <summary>
/// 消费者
/// </summary>
/// <typeparam name="TMessage"></typeparam>
public interface IConsumerMessage<in TMessage>
{
    /// <summary>
    /// 消费
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task ExecuteAsync(TMessage message);

    /// <summary>
    /// 回滚
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task RollBackAsync(TMessage message);
}