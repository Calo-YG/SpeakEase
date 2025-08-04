namespace SpeakEase.EventBus.EventBus.BuildingBlock.Local.EventBus;

public interface IEvent
{
    Guid GetEventId();

    void SetEventId(Guid eventId);

    DateTimeOffset GetCreationTime();

    void SetCreationTime(DateTimeOffset creationTime);
}