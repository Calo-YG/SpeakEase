# SpeakEase RabbitMQ Event Bus

基于C# RabbitMQ的高性能事件总线实现，支持分布式事件发布和订阅。

## 特性

- 🚀 **高性能**: 基于RabbitMQ.Client，支持连接池和通道复用
- 🔄 **自动重连**: 支持网络断开自动重连和连接恢复
- 📨 **消息持久化**: 支持消息持久化，确保消息不丢失
- 🔒 **发布确认**: 支持发布确认机制，确保消息可靠投递
- 🔁 **重试机制**: 内置重试机制，支持失败重试
- 🎯 **路由支持**: 支持Topic交换机，灵活的路由策略
- 📊 **监控日志**: 完整的日志记录和监控支持
- 🏗️ **依赖注入**: 完全集成.NET依赖注入容器

## 安装

确保项目引用了以下NuGet包：

```xml
<PackageReference Include="RabbitMQ.Client" Version="6.8.1" />
<PackageReference Include="System.Text.Json" Version="9.0.1" />
<PackageReference Include="Microsoft.Extensions.Hosting.Abstractions" Version="9.0.1" />
<PackageReference Include="Microsoft.Extensions.Configuration.Abstractions" Version="9.0.1" />
```

## 配置

### 1. 在appsettings.json中配置RabbitMQ

```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/",
    "RequestedConnectionTimeout": 30,
    "RequestedHeartbeat": 60,
    "AutomaticRecoveryEnabled": true,
    "NetworkRecoveryInterval": 10,
    "DefaultExchange": "speakease.events",
    "DefaultQueuePrefix": "speakease.queue.",
    "MessagePersistent": true,
    "PublisherConfirms": true,
    "ConsumerPrefetchCount": 1,
    "MaxRetryCount": 3,
    "RetryIntervalMs": 1000
  }
}
```

### 2. 在Program.cs中注册服务

```csharp
using SpeakEase.Infrastructure.EventBus;

var builder = WebApplication.CreateBuilder(args);

// 注册RabbitMQ事件总线
builder.Services.RegisterRabbitMQEventBus(builder.Configuration);

// 注册事件处理器
builder.Services.AddEventHandler<UserCreatedEvent, UserCreatedEventHandler>();

var app = builder.Build();
```

## 使用示例

### 1. 定义事件

```csharp
public class UserCreatedEvent : IEvent
{
    public Guid EventId { get; set; }
    public DateTimeOffset CreationTime { get; set; }
    
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }

    public Guid GetEventId() => EventId;
    public void SetEventId(Guid eventId) => EventId = eventId;
    public DateTimeOffset GetCreationTime() => CreationTime;
    public void SetCreationTime(DateTimeOffset creationTime) => CreationTime = creationTime;
}
```

### 2. 实现事件处理器

```csharp
public class UserCreatedEventHandler : IRabbitMQEventHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger;

    public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(UserCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing UserCreatedEvent: UserId={UserId}", @event.UserId);
        
        // 处理业务逻辑
        await SendWelcomeEmailAsync(@event.Email, cancellationToken);
        await CreateUserProfileAsync(@event.UserId, cancellationToken);
        
        _logger.LogInformation("UserCreatedEvent processed successfully");
    }
}
```

### 3. 发布事件

```csharp
public class UserService
{
    private readonly IRabbitMQEventBus _eventBus;

    public UserService(IRabbitMQEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task CreateUserAsync(CreateUserRequest request)
    {
        // 创建用户的业务逻辑
        var user = await CreateUserInDatabaseAsync(request);
        
        // 发布用户创建事件
        var userCreatedEvent = new UserCreatedEvent
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _eventBus.PublishAsync(userCreatedEvent, "user.created");
    }
}
```

### 4. 订阅事件

```csharp
public class EventSubscriptionService : IHostedService
{
    private readonly IRabbitMQEventBus _eventBus;

    public EventSubscriptionService(IRabbitMQEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // 订阅用户创建事件
        await _eventBus.SubscribeAsync<UserCreatedEvent, UserCreatedEventHandler>(
            "user-created-queue", 
            "user.created", 
            cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
```

## 高级用法

### 1. 自定义配置

```csharp
builder.Services.RegisterRabbitMQEventBus(builder.Configuration, options =>
{
    options.HostName = "rabbitmq.example.com";
    options.Port = 5672;
    options.UserName = "myuser";
    options.Password = "mypassword";
    options.DefaultExchange = "myapp.events";
    options.MaxRetryCount = 5;
    options.RetryIntervalMs = 2000;
});
```

### 2. 发布到自定义交换机

```csharp
await _eventBus.PublishToExchangeAsync(
    userCreatedEvent, 
    "custom.exchange", 
    "user.created");
```

### 3. 单例事件处理器

```csharp
builder.Services.AddEventHandlerSingleton<UserCreatedEvent, UserCreatedEventHandler>();
```

## 配置选项说明

| 选项 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| HostName | string | "localhost" | RabbitMQ服务器主机名 |
| Port | int | 5672 | RabbitMQ服务器端口 |
| UserName | string | "guest" | 用户名 |
| Password | string | "guest" | 密码 |
| VirtualHost | string | "/" | 虚拟主机 |
| RequestedConnectionTimeout | int | 30 | 连接超时时间（秒） |
| RequestedHeartbeat | int | 60 | 心跳间隔（秒） |
| AutomaticRecoveryEnabled | bool | true | 是否启用自动重连 |
| NetworkRecoveryInterval | int | 10 | 网络恢复间隔（秒） |
| DefaultExchange | string | "speakease.events" | 默认交换机名称 |
| DefaultQueuePrefix | string | "speakease.queue." | 默认队列前缀 |
| MessagePersistent | bool | true | 消息是否持久化 |
| PublisherConfirms | bool | true | 是否启用发布确认 |
| ConsumerPrefetchCount | ushort | 1 | 消费者预取数量 |
| MaxRetryCount | int | 3 | 最大重试次数 |
| RetryIntervalMs | int | 1000 | 重试间隔（毫秒） |

## 性能优化建议

1. **连接池**: 系统自动管理连接池，无需手动配置
2. **通道复用**: 发布消息时自动复用通道，提高性能
3. **批量处理**: 对于高吞吐量场景，考虑批量处理消息
4. **预取设置**: 根据处理能力调整ConsumerPrefetchCount
5. **持久化**: 生产环境建议启用消息持久化

## 监控和日志

系统提供完整的日志记录，包括：
- 连接状态变化
- 消息发布和消费
- 错误和重试信息
- 性能指标

可以通过配置日志级别来控制日志输出：

```json
{
  "Logging": {
    "LogLevel": {
      "SpeakEase.Infrastructure.EventBus": "Information"
    }
  }
}
```

## 故障排除

### 常见问题

1. **连接失败**: 检查RabbitMQ服务是否运行，网络连接是否正常
2. **消息丢失**: 确保启用了消息持久化和发布确认
3. **处理失败**: 检查事件处理器是否正确注册，依赖注入是否配置正确
4. **性能问题**: 调整预取数量、重试间隔等配置参数

### 调试模式

启用调试日志以获取更详细的信息：

```json
{
  "Logging": {
    "LogLevel": {
      "SpeakEase.Infrastructure.EventBus": "Debug"
    }
  }
}
```

## 许可证

本项目采用MIT许可证。 