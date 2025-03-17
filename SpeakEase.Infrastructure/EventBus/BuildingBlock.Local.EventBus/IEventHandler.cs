namespace SpeakEase.Infrastructure.EventBus.BuildingBlock.Local.EventBus;

public interface IEventHandler<TEvent>
{
     Task HandlerAsync(TEvent @event, CancellationToken cancellationToken = default);
}