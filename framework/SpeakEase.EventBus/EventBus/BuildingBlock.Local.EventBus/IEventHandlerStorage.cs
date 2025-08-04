using System.Collections.Concurrent;
using SpeakEase.EventBus.EventBus.BuildingBlock.Local.EventBus;

namespace SpeakEase.Infrastructure.EventBus.BuildingBlock.Local.EventBus;

public interface IEventHandlerStorage
{
    public ConcurrentBag<EventDiscription> Events { get; }
}