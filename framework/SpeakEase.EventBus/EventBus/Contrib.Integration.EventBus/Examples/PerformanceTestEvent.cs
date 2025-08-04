using SpeakEase.EventBus.EventBus.BuildingBlock.Local.EventBus;

namespace SpeakEase.Infrastructure.EventBus.Contrib.Integration.EventBus.Examples;

/// <summary>
/// 性能测试事件
/// </summary>
public class PerformanceTestEvent : IEvent
{
    public Guid EventId { get; set; }
    public DateTimeOffset CreationTime { get; set; }
    
    public int MessageId { get; set; }
    public string Payload { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }

    public Guid GetEventId() => EventId;
    public void SetEventId(Guid eventId) => EventId = eventId;
    public DateTimeOffset GetCreationTime() => CreationTime;
    public void SetCreationTime(DateTimeOffset creationTime) => CreationTime = creationTime;
} 