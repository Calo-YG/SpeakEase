using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SpeakEase.Infrastructure.EventBus.BuildingBlock.Integration.EventBus;
using System.Collections.Concurrent;

namespace SpeakEase.Infrastructure.EventBus.Contrib.Integration.EventBus;

/// <summary>
/// RabbitMQ连接管理器实现
/// </summary>
public class RabbitMQConnectionManager : IRabbitMQConnectionManager, IDisposable
{
    private readonly RabbitMQOptions _options;
    private readonly ILogger<RabbitMQConnectionManager> _logger;
    private readonly object _lockObject = new object();
    private readonly ConcurrentQueue<IModel> _channelPool = new();
    private readonly int _maxChannelPoolSize = 10;

    private IConnection? _connection;
    private bool _disposed;

    public event EventHandler<bool>? ConnectionChanged;

    public bool IsConnected => _connection?.IsOpen == true;

    public RabbitMQConnectionManager(IOptions<RabbitMQOptions> options, ILogger<RabbitMQConnectionManager> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// 获取RabbitMQ连接
    /// </summary>
    /// <returns>RabbitMQ连接对象</returns>
    /// <exception cref="ObjectDisposedException">当连接管理器已释放时抛出</exception>
    /// <exception cref="InvalidOperationException">当连接创建失败时抛出</exception>
    public IConnection GetConnection()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(RabbitMQConnectionManager));

        if (_connection?.IsOpen == true)
            return _connection;

        lock (_lockObject)
        {
            if (_connection?.IsOpen == true)
                return _connection;

            _connection = CreateConnection();
            return _connection;
        }
    }

    /// <summary>
    /// 从通道池获取通道，如果池中没有可用通道则创建新通道
    /// </summary>
    /// <returns>RabbitMQ通道对象</returns>
    /// <exception cref="ObjectDisposedException">当连接管理器已释放时抛出</exception>
    public IModel GetChannel()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(RabbitMQConnectionManager));

        if (_channelPool.TryDequeue(out var channel) && channel?.IsOpen == true)
        {
            return channel;
        }

        return CreateChannel();
    }

    /// <summary>
    /// 创建新的RabbitMQ通道
    /// </summary>
    /// <returns>新创建的RabbitMQ通道对象</returns>
    /// <exception cref="ObjectDisposedException">当连接管理器已释放时抛出</exception>
    public IModel CreateChannel()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(RabbitMQConnectionManager));

        var connection = GetConnection();
        var channel = connection.CreateModel();

        // 设置QoS
        channel.BasicQos(0, _options.ConsumerPrefetchCount, false);

        // 启用发布确认
        if (_options.PublisherConfirms)
        {
            channel.ConfirmSelect();
        }

        return channel;
    }

    /// <summary>
    /// 释放通道，如果通道池未满则回收到池中，否则关闭并释放
    /// </summary>
    /// <param name="channel">要释放的通道</param>
    public void ReleaseChannel(IModel channel)
    {
        if (channel == null || _disposed)
            return;

        try
        {
            if (channel.IsOpen && _channelPool.Count < _maxChannelPoolSize)
            {
                _channelPool.Enqueue(channel);
            }
            else
            {
                channel.Close();
                channel.Dispose();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error releasing channel");
            try
            {
                channel?.Dispose();
            }
            catch
            {
                // Ignore disposal errors
            }
        }
    }

    /// <summary>
    /// 创建RabbitMQ连接
    /// </summary>
    /// <returns>新创建的RabbitMQ连接</returns>
    /// <exception cref="InvalidOperationException">当连接创建失败时抛出</exception>
    private IConnection CreateConnection()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(_options.RequestedConnectionTimeout),
                RequestedHeartbeat = TimeSpan.FromSeconds(_options.RequestedHeartbeat),
                AutomaticRecoveryEnabled = _options.AutomaticRecoveryEnabled,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(_options.NetworkRecoveryInterval)
            };

            var connection = factory.CreateConnection();
            
            connection.ConnectionShutdown += OnConnectionShutdown;
            connection.ConnectionRecoveryError += OnConnectionRecoveryError;
            connection.Recovery += OnConnectionRecovery;

            _logger.LogInformation("RabbitMQ connection established");
            ConnectionChanged?.Invoke(this, true);

            return connection;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create RabbitMQ connection");
            throw;
        }
    }

    /// <summary>
    /// 连接关闭事件处理
    /// </summary>
    /// <param name="sender">事件发送者</param>
    /// <param name="e">关闭事件参数</param>
    private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogWarning("RabbitMQ connection shutdown: {Reason}", e.Initiator);
        ConnectionChanged?.Invoke(this, false);
    }

    /// <summary>
    /// 连接恢复错误事件处理
    /// </summary>
    /// <param name="sender">事件发送者</param>
    /// <param name="e">恢复错误事件参数</param>
    private void OnConnectionRecoveryError(object? sender, ConnectionRecoveryErrorEventArgs e)
    {
        _logger.LogError(e.Exception, "RabbitMQ connection recovery error");
    }

    /// <summary>
    /// 连接恢复事件处理
    /// </summary>
    /// <param name="sender">事件发送者</param>
    /// <param name="e">恢复事件参数</param>
    private void OnConnectionRecovery(object? sender, EventArgs e)
    {
        _logger.LogInformation("RabbitMQ connection recovered");
        ConnectionChanged?.Invoke(this, true);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        try
        {
            // 释放通道池中的所有通道
            while (_channelPool.TryDequeue(out var channel))
            {
                try
                {
                    channel?.Close();
                    channel?.Dispose();
                }
                catch
                {
                    // Ignore disposal errors
                }
            }

            // 释放连接
            if (_connection != null)
            {
                _connection.ConnectionShutdown -= OnConnectionShutdown;
                _connection.ConnectionRecoveryError -= OnConnectionRecoveryError;
                _connection.Recovery -= OnConnectionRecovery;

                try
                {
                    _connection.Close();
                    _connection.Dispose();
                }
                catch
                {
                    // Ignore disposal errors
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disposing RabbitMQ connection manager");
        }
    }
} 