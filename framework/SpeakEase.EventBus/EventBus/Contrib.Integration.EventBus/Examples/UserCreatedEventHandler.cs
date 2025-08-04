using Microsoft.Extensions.Logging;
using SpeakEase.Infrastructure.EventBus.BuildingBlock.Integration.EventBus;

namespace SpeakEase.Infrastructure.EventBus.Contrib.Integration.EventBus.Examples;

/// <summary>
/// 用户创建事件处理器示例
/// </summary>
public class UserCreatedEventHandler : IRabbitMQEventHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger;

    public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 处理用户创建事件
    /// </summary>
    /// <param name="event">用户创建事件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>处理任务</returns>
    public async Task HandleAsync(UserCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing UserCreatedEvent: UserId={UserId}, UserName={UserName}, Email={Email}", 
            @event.UserId, @event.UserName, @event.Email);

        // 模拟异步处理
        await Task.Delay(100, cancellationToken);

        // 这里可以添加具体的业务逻辑
        // 例如：发送欢迎邮件、创建用户档案、初始化用户设置等

        _logger.LogInformation("UserCreatedEvent processed successfully for UserId: {UserId}", @event.UserId);
    }
} 