using Microsoft.Extensions.Logging;
using SpeakEase.Infrastructure.EventBus.BuildingBlock.Integration.EventBus;
using System.Collections.Concurrent;

namespace SpeakEase.Infrastructure.EventBus.Contrib.Integration.EventBus.Examples;

/// <summary>
/// 性能测试事件处理器
/// </summary>
public class PerformanceTestEventHandler : IRabbitMQEventHandler<PerformanceTestEvent>
{
    private readonly ILogger<PerformanceTestEventHandler> _logger;
    private static readonly ConcurrentDictionary<int, DateTimeOffset> _processedMessages = new();
    private static int _totalProcessed = 0;
    private static readonly object _lockObject = new object();

    public PerformanceTestEventHandler(ILogger<PerformanceTestEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 处理性能测试事件
    /// </summary>
    /// <param name="event">性能测试事件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>处理任务</returns>
    public async Task HandleAsync(PerformanceTestEvent @event, CancellationToken cancellationToken = default)
    {
        var startTime = DateTimeOffset.UtcNow;
        
        // 模拟处理时间
        await Task.Delay(10, cancellationToken);
        
        var endTime = DateTimeOffset.UtcNow;
        var processingTime = endTime - startTime;
        
        _processedMessages[@event.MessageId] = endTime;
        
        lock (_lockObject)
        {
            _totalProcessed++;
        }

        if (_totalProcessed % 1000 == 0)
        {
            _logger.LogInformation("Processed {TotalProcessed} messages. Current message: {MessageId}, Processing time: {ProcessingTime}ms", 
                _totalProcessed, @event.MessageId, processingTime.TotalMilliseconds);
        }

        // 验证消息顺序（可选）
        if (@event.MessageId % 10000 == 0)
        {
            _logger.LogInformation("Message {MessageId} processed successfully", @event.MessageId);
        }
    }

    /// <summary>
    /// 获取已处理的消息总数
    /// </summary>
    /// <returns>已处理的消息数量</returns>
    public static int GetTotalProcessed() => _totalProcessed;
    
    /// <summary>
    /// 重置计数器
    /// </summary>
    public static void ResetCounters()
    {
        lock (_lockObject)
        {
            _totalProcessed = 0;
            _processedMessages.Clear();
        }
    }
} 