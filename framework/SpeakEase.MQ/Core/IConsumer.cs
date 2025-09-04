namespace SpeakEase.MQ.Core;

/// <summary>
/// 消费者
/// </summary>
/// <typeparam name="TMessage"></typeparam>
public interface IConsumer<in TMessage>
{
    /// <summary>
    /// 消费
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task ExecuteAsync(TMessage message);
}