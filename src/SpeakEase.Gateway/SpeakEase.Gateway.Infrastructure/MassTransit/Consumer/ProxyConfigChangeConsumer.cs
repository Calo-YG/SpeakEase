using MassTransit;
using SpeakEase.Gateway.Infrastructure.MassTransit.Message;

namespace SpeakEase.Gateway.Infrastructure.MassTransit.Consumer;

/// <summary>
/// 代理配置变更消费者
/// </summary>
public class ProxyConfigChangeConsumer:IConsumer<ProxyConfigChangeMessage>
{
    public Task Consume(ConsumeContext<ProxyConfigChangeMessage> context)
    {
        throw new NotImplementedException();
    }
}