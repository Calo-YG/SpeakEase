using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpeakEase.Infrastructure.EventBus.BuildingBlock.Integration.EventBus;

namespace SpeakEase.Infrastructure.EventBus.Contrib.Integration.EventBus;

/// <summary>
/// RabbitMQ事件总线后台服务
/// </summary>
public class RabbitMQEventBusHostedService : BackgroundService
{
    private readonly IRabbitMQEventBus _eventBus;
    private readonly ILogger<RabbitMQEventBusHostedService> _logger;

    public RabbitMQEventBusHostedService(
        IRabbitMQEventBus eventBus,
        ILogger<RabbitMQEventBusHostedService> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    /// <summary>
    /// 执行后台服务
    /// </summary>
    /// <param name="stoppingToken">停止令牌</param>
    /// <returns>执行任务</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Starting RabbitMQ event bus hosted service");
            
            await _eventBus.StartAsync(stoppingToken);
            
            _logger.LogInformation("RabbitMQ event bus hosted service started successfully");
            
            // 等待停止信号
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("RabbitMQ event bus hosted service is stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in RabbitMQ event bus hosted service");
            throw;
        }
    }

    /// <summary>
    /// 停止后台服务
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>停止任务</returns>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Stopping RabbitMQ event bus hosted service");
            
            await _eventBus.StopAsync(cancellationToken);
            
            _logger.LogInformation("RabbitMQ event bus hosted service stopped successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error stopping RabbitMQ event bus hosted service");
        }
        finally
        {
            await base.StopAsync(cancellationToken);
        }
    }
} 