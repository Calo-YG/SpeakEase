namespace SpeakEase.Infrastructure.EventBus.BuildingBlockEventBus;

public interface IEvent
{
      Guid GetEventId();

      void SetEventId(Guid eventId);

      DateTimeOffset GetCreationTime();

      void SetCreationTime(DateTimeOffset creationTime);
}