using System.Collections.Concurrent;

namespace SpeakEase.Infrastructure.EventBus.BuildingBlock.Local.EventBus;

public interface IEventHandlerStorage
{
      public ConcurrentBag<EventDiscription> Events { get; }
}
