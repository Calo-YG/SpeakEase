using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SpeakEase.Infrastructure.EventBus.BuildingBlock.Integration.EventBus;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using SpeakEase.EventBus.EventBus.BuildingBlock.Local.EventBus;

namespace SpeakEase.Infrastructure.EventBus.Contrib.Integration.EventBus;

/// <summary>
/// RabbitMQ事件总线实现
/// </summary>
public class RabbitMQEventBus : IRabbitMQEventBus, IDisposable
{
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMQOptions _options;
    private readonly ILogger<RabbitMQEventBus> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ConcurrentDictionary<string, IModel> _consumerChannels = new();
    private readonly ConcurrentDictionary<string, EventingBasicConsumer> _consumers = new();
    private readonly SemaphoreSlim _publishSemaphore = new(1, 1);
    private bool _disposed;

    public RabbitMQEventBus(
        IRabbitMQConnectionManager connectionManager,
        IServiceProvider serviceProvider,
        IOptions<RabbitMQOptions> options,
        ILogger<RabbitMQEventBus> logger)
    {
        _connectionManager = connectionManager;
        _serviceProvider = serviceProvider;
        _options = options.Value;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        _connectionManager.ConnectionChanged += OnConnectionChanged;
    }

    /// <summary>
    /// 发布事件到默认交换机
    /// </summary>
    /// <typeparam name="TEvent">事件类型，必须实现IEvent接口</typeparam>
    /// <param name="event">要发布的事件对象</param>
    /// <param name="routingKey">路由键，用于消息路由</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>发布任务</returns>
    /// <exception cref="ArgumentNullException">当事件对象为null时抛出</exception>
    /// <exception cref="InvalidOperationException">当发布确认超时时抛出</exception>
    public async Task PublishAsync<TEvent>(TEvent @event, string routingKey = "", CancellationToken cancellationToken = default) 
        where TEvent : class, IEvent
    {
        await PublishToExchangeAsync(@event, _options.DefaultExchange, routingKey, cancellationToken);
    }

    /// <summary>
    /// 发布事件到指定交换机
    /// </summary>
    /// <typeparam name="TEvent">事件类型，必须实现IEvent接口</typeparam>
    /// <param name="event">要发布的事件对象</param>
    /// <param name="exchange">交换机名称</param>
    /// <param name="routingKey">路由键，用于消息路由</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>发布任务</returns>
    /// <exception cref="ArgumentNullException">当事件对象为null时抛出</exception>
    /// <exception cref="ArgumentException">当交换机名称为空时抛出</exception>
    /// <exception cref="InvalidOperationException">当发布确认超时时抛出</exception>
    public async Task PublishToExchangeAsync<TEvent>(TEvent @event, string exchange, string routingKey = "", CancellationToken cancellationToken = default) 
        where TEvent : class, IEvent
    {
        if (@event == null)
            throw new ArgumentNullException(nameof(@event));

        if (string.IsNullOrEmpty(exchange))
            throw new ArgumentException("Exchange name cannot be null or empty", nameof(exchange));

        await _publishSemaphore.WaitAsync(cancellationToken);
        try
        {
            var channel = _connectionManager.GetChannel();
            try
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true, autoDelete: false);

                var eventType = typeof(TEvent).Name;
                var message = JsonSerializer.Serialize(@event, _jsonOptions);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = _options.MessagePersistent;
                properties.MessageId = @event.GetEventId().ToString();
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                properties.Type = eventType;
                properties.Headers = new Dictionary<string, object>
                {
                    ["EventType"] = eventType,
                    ["CreationTime"] = @event.GetCreationTime().ToString("O")
                };

                channel.BasicPublish(
                    exchange: exchange,
                    routingKey: routingKey,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);

                if (_options.PublisherConfirms)
                {
                    if (!channel.WaitForConfirms(TimeSpan.FromSeconds(5)))
                    {
                        throw new InvalidOperationException("Message publish confirmation timeout");
                    }
                }

                _logger.LogDebug("Event {EventType} published to exchange {Exchange} with routing key {RoutingKey}", 
                    eventType, exchange, routingKey);
            }
            finally
            {
                _connectionManager.ReleaseChannel(channel);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {EventType} to exchange {Exchange}", 
                typeof(TEvent).Name, exchange);
            throw;
        }
        finally
        {
            _publishSemaphore.Release();
        }
    }

    /// <summary>
    /// 订阅事件到指定队列
    /// </summary>
    /// <typeparam name="TEvent">事件类型，必须实现IEvent接口</typeparam>
    /// <typeparam name="THandler">事件处理器类型，必须实现IRabbitMQEventHandler接口</typeparam>
    /// <param name="queueName">队列名称</param>
    /// <param name="routingKey">路由键，用于消息过滤</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>订阅任务</returns>
    /// <exception cref="ArgumentException">当队列名称为空时抛出</exception>
    /// <exception cref="InvalidOperationException">当队列已存在消费者时抛出</exception>
    public async Task SubscribeAsync<TEvent, THandler>(string queueName, string routingKey = "", CancellationToken cancellationToken = default) 
        where TEvent : class, IEvent 
        where THandler : class, IRabbitMQEventHandler<TEvent>
    {
        if (string.IsNullOrEmpty(queueName))
            throw new ArgumentException("Queue name cannot be null or empty", nameof(queueName));

        if (_consumerChannels.ContainsKey(queueName))
        {
            _logger.LogWarning("Consumer for queue {QueueName} already exists", queueName);
            return;
        }

        var channel = _connectionManager.CreateChannel();
        try
        {
            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueBind(
                queue: queueName,
                exchange: _options.DefaultExchange,
                routingKey: routingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) => await OnMessageReceived<TEvent, THandler>(ea, cancellationToken);

            var consumerTag = channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: consumer);

            _consumerChannels[queueName] = channel;
            _consumers[queueName] = consumer;

            _logger.LogInformation("Started consuming from queue {QueueName} with routing key {RoutingKey}", 
                queueName, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start consumer for queue {QueueName}", queueName);
            _connectionManager.ReleaseChannel(channel);
            throw;
        }
    }

    /// <summary>
    /// 启动事件总线
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>启动任务</returns>
    /// <exception cref="ObjectDisposedException">当事件总线已释放时抛出</exception>
    /// <exception cref="InvalidOperationException">当连接失败时抛出</exception>
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(RabbitMQEventBus));

        try
        {
            _connectionManager.GetConnection();
            _logger.LogInformation("RabbitMQ event bus started");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start RabbitMQ event bus");
            throw;
        }
    }

    /// <summary>
    /// 停止事件总线
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>停止任务</returns>
    /// <exception cref="InvalidOperationException">当停止过程中发生错误时抛出</exception>
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var kvp in _consumerChannels)
            {
                try
                {
                    var channel = kvp.Value;
                    var queueName = kvp.Key;

                    if (channel.IsOpen)
                    {
                        var consumerTag = _consumers[queueName].ConsumerTags?.FirstOrDefault();
                        if (!string.IsNullOrEmpty(consumerTag))
                        {
                            channel.BasicCancel(consumerTag);
                        }
                    }

                    _connectionManager.ReleaseChannel(channel);
                    _logger.LogInformation("Stopped consumer for queue {QueueName}", queueName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error stopping consumer for queue {QueueName}", kvp.Key);
                }
            }

            _consumerChannels.Clear();
            _consumers.Clear();

            _logger.LogInformation("RabbitMQ event bus stopped");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error stopping RabbitMQ event bus");
            throw;
        }
    }

    /// <summary>
    /// 处理接收到的消息
    /// </summary>
    /// <typeparam name="TEvent">事件类型</typeparam>
    /// <typeparam name="THandler">处理器类型</typeparam>
    /// <param name="ea">消息传递事件参数</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>处理任务</returns>
    private async Task OnMessageReceived<TEvent, THandler>(BasicDeliverEventArgs ea, CancellationToken cancellationToken) 
        where TEvent : class, IEvent 
        where THandler : class, IRabbitMQEventHandler<TEvent>
    {
        var channel = ea.ConsumerTag != null ? _consumerChannels.Values.FirstOrDefault(c => c.IsOpen) : null;
        if (channel == null)
        {
            _logger.LogError("No available channel for message processing");
            return;
        }

        try
        {
            var message = Encoding.UTF8.GetString(ea.Body.Span);
            var eventType = ea.BasicProperties.Type ?? typeof(TEvent).Name;

            _logger.LogDebug("Received message: {MessageId}, Type: {EventType}", 
                ea.BasicProperties.MessageId, eventType);

            var @event = JsonSerializer.Deserialize<TEvent>(message, _jsonOptions);
            if (@event == null)
            {
                _logger.LogError("Failed to deserialize event of type {EventType}", eventType);
                channel.BasicNack(ea.DeliveryTag, false, false);
                return;
            }

            if (Guid.TryParse(ea.BasicProperties.MessageId, out var eventId))
            {
                @event.SetEventId(eventId);
            }

            if (ea.BasicProperties.Timestamp.UnixTime > 0)
            {
                @event.SetCreationTime(DateTimeOffset.FromUnixTimeSeconds(ea.BasicProperties.Timestamp.UnixTime));
            }

            await ProcessEventAsync<TEvent, THandler>(@event, cancellationToken);

            channel.BasicAck(ea.DeliveryTag, false);
            _logger.LogDebug("Message {MessageId} processed successfully", ea.BasicProperties.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message {MessageId}", ea.BasicProperties.MessageId);
            
            var shouldRequeue = ShouldRequeue(ea);
            channel.BasicNack(ea.DeliveryTag, false, shouldRequeue);
        }
    }

    /// <summary>
    /// 处理事件，包含重试机制
    /// </summary>
    /// <typeparam name="TEvent">事件类型</typeparam>
    /// <typeparam name="THandler">处理器类型</typeparam>
    /// <param name="event">要处理的事件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>处理任务</returns>
    private async Task ProcessEventAsync<TEvent, THandler>(TEvent @event, CancellationToken cancellationToken) 
        where TEvent : class, IEvent 
        where THandler : class, IRabbitMQEventHandler<TEvent>
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<THandler>();

        var retryCount = 0;
        while (retryCount < _options.MaxRetryCount)
        {
            try
            {
                await handler.HandleAsync(@event, cancellationToken);
                return;
            }
            catch (Exception ex)
            {
                retryCount++;
                _logger.LogWarning(ex, "Error handling event {EventType}, retry {RetryCount}/{MaxRetries}", 
                    typeof(TEvent).Name, retryCount, _options.MaxRetryCount);

                if (retryCount >= _options.MaxRetryCount)
                {
                    throw;
                }

                await Task.Delay(_options.RetryIntervalMs * retryCount, cancellationToken);
            }
        }
    }

    /// <summary>
    /// 判断是否应该重新入队
    /// </summary>
    /// <param name="ea">消息传递事件参数</param>
    /// <returns>是否重新入队</returns>
    private bool ShouldRequeue(BasicDeliverEventArgs ea)
    {
        if (ea.BasicProperties.Headers != null && 
            ea.BasicProperties.Headers.TryGetValue("RetryCount", out var retryCountObj) &&
            retryCountObj is int retryCount)
        {
            return retryCount < _options.MaxRetryCount;
        }

        return true;
    }

    /// <summary>
    /// 连接状态变化事件处理
    /// </summary>
    /// <param name="sender">事件发送者</param>
    /// <param name="isConnected">是否已连接</param>
    private void OnConnectionChanged(object? sender, bool isConnected)
    {
        if (isConnected)
        {
            _logger.LogInformation("RabbitMQ connection restored");
        }
        else
        {
            _logger.LogWarning("RabbitMQ connection lost");
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        try
        {
            _connectionManager.ConnectionChanged -= OnConnectionChanged;
            _publishSemaphore.Dispose();
            
            foreach (var channel in _consumerChannels.Values)
            {
                try
                {
                    _connectionManager.ReleaseChannel(channel);
                }
                catch
                {
                    // Ignore disposal errors
                }
            }

            _consumerChannels.Clear();
            _consumers.Clear();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disposing RabbitMQ event bus");
        }
    }
} 