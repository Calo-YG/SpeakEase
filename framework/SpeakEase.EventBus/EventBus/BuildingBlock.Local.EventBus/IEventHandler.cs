namespace SpeakEase.EventBus.EventBus.BuildingBlock.Local.EventBus;

public interface IEventHandler<TEvent>
{
    Task HandlerAsync(TEvent @event, CancellationToken cancellationToken = default);
}