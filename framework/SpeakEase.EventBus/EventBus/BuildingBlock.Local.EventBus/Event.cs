using System.Text.Json.Serialization;
using SpeakEase.Infrastructure.EventBus.BuildingBlockEventBus;

namespace SpeakEase.Infrastructure.EventBus.BuildingBlock.Local.EventBus;

public abstract record Event(Guid EventId, DateTimeOffset EvenCreateTime, string EventName) : IEvent
{
    [JsonInclude] public Guid EventId { private get; set; } = EventId;

    [JsonInclude] public DateTimeOffset EvenCreateTime { private get; set; } = EvenCreateTime;

    [JsonInclude] public string EventName { private get; set; } = EventName;

    protected Event() : this(Guid.NewGuid(), DateTime.UtcNow, string.Empty)
    {
    }

    public Guid GetEventId() => EventId;

    public void SetEventId(Guid eventId) => EventId = eventId;

    public DateTimeOffset GetCreationTime() => EvenCreateTime;

    public void SetCreationTime(DateTimeOffset creationTime) => EvenCreateTime = creationTime;

    public string GetEventName() => EventName;

    public void SetEventName(string eventName) => EventName = eventName;
}