using SpeakEase.EventBus.EventBus.BuildingBlock.Local.EventBus;

namespace SpeakEase.Infrastructure.EventBus.Contrib.Integration.EventBus.Examples;

/// <summary>
/// 用户创建事件示例
/// </summary>
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