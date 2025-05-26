using System.Collections.Concurrent;
using SpeakEase.Infrastructure.EventBus.BuildingBlockEventBus;

namespace SpeakEase.Infrastructure.EventBus.BuildingBlock.Local.EventBus;

public interface IEventBus
{
    Task EnqueueAsync<TEto>(TEto eto, CancellationToken cancellationToken = default)
        where TEto : class;

    Task ExecuteAsync<TEto>(TEto eto, CancellationToken cancellationToken = default) where TEto : class;

    Task Start();
}