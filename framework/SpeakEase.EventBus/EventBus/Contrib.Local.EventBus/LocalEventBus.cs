using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpeakEase.EventBus.EventBus.BuildingBlock.Local.EventBus;
using SpeakEase.EventBus.EventBus.Contrib.Local.EventBus;
using SpeakEase.Infrastructure.EventBus.BuildingBlock.Local.EventBus;

namespace SpeakEase.Infrastructure.EventBus.Contrib.Local.EventBus;

public class LocalEventBus(IServiceProvider serviceProvider, IEventHandlerStorage eventHandlerStorage, ILoggerFactory factory)
    : IEventBus, IDisposable
{
    private readonly JsonSerializerOptions DefaultSerializerDefaults = new JsonSerializerOptions()
    {
       PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly CancellationTokenSource _cts = new();

    private readonly ILogger _logger = factory.CreateLogger<IEventBus>();

    private ConcurrentDictionary<string, Channel<string>> _channels = new();

    private readonly AsyncServiceScope _scope = serviceProvider.CreateAsyncScope();

    private bool isInitConsumer = true;

    private Channel<string> Check(Type type)
    {
        var attribute = type.GetCustomAttributes()
            .OfType<EventDiscriptorAttribute>()
            .FirstOrDefault();

        if (attribute is null)
        {
            ThorwEventAttributeNullException.ThorwException();
        }

        var channel = _channels.GetValueOrDefault(attribute!.EventName);

        if (channel is null)
        {
            ThrowChannelNullException.ThrowException(attribute.EventName);
        }

        return channel!;
    }


    /// <summary>
    /// 生产者
    /// </summary>
    /// <typeparam name="TEto"></typeparam>
    /// <param name="eto"></param>
    /// <returns></returns> 
    public async Task EnqueueAsync<TEto>(TEto eto, CancellationToken cancellationToken = default)
        where TEto : class
    {
        var channel = Check(typeof(TEto));

        while (await channel.Writer.WaitToWriteAsync())
        {
            var data = JsonSerializer.Serialize(eto, DefaultSerializerDefaults);

            await channel.Writer.WriteAsync(data, _cts.Token);

            break;
        }
    }

    private void CreateChannles()
    {
        var eventDiscriptions = eventHandlerStorage.Events;

        foreach (var item in eventDiscriptions)
        {
            var attribute = item.EtoType
                .GetCustomAttributes()
                .OfType<EventDiscriptorAttribute>()
                .FirstOrDefault();

            if (attribute is null)
            {
                ThorwEventAttributeNullException.ThorwException();
            }

            var channel = _channels.GetValueOrDefault(attribute!.EventName);

            if (channel is not null)
            {
                return;
            }

            channel = Channel.CreateUnbounded<string>(
                new UnboundedChannelOptions()
                {
                    SingleWriter = true,
                    SingleReader = true,
                    AllowSynchronousContinuations = true
                }
            );

            _channels.TryAdd(attribute.EventName, channel);

            _logger.LogInformation($"创建通信管道{item.EtoType}--{attribute.EventName}");
        }
    }

    /// <summary>
    /// 同步实现
    /// </summary>
    /// <param name="eto"></param>
    /// <typeparam name="TEto"></typeparam>
    public async Task ExecuteAsync<TEto>(TEto eto, CancellationToken cancellationToken = default) where TEto : class
    {
        var handler = _scope.ServiceProvider.GetRequiredService<IEventHandler<TEto>>();

        await handler.HandlerAsync(eto, cancellationToken);
    }

    /// <summary>
    /// 消费者
    /// </summary>
    /// <returns></returns>
    public async Task Start()
    {
        if (!isInitConsumer)
        {
            return;
        }

        CreateChannles();

        foreach (var item in eventHandlerStorage.Events)
        {
            _ = Task.Factory.StartNew(async () =>
            {
                var attribute = item.EtoType
                    .GetCustomAttributes()
                    .OfType<EventDiscriptorAttribute>()
                    .FirstOrDefault();

                var channel = Check(item.EtoType);

                var handlerType = typeof(IEventHandler<>).MakeGenericType(item.EtoType);

                var handler = _scope.ServiceProvider.GetRequiredService(handlerType);

                var reader = channel.Reader;

                try
                {
                    while (await channel.Reader.WaitToReadAsync() && !_cts.IsCancellationRequested)
                    {
                        while (reader.TryRead(out string str))
                        {
                            var data = JsonSerializer.Deserialize(str!, item.EtoType, DefaultSerializerDefaults);

                            _logger.LogInformation($"Event Name：{attribute.EventName} --Start Execute Time：{DateTime.Now}");

                            await (Task)handlerType.GetMethod("HandlerAsync").Invoke(handler, new object[] { data, _cts.Token });

                            _logger.LogInformation($"Event Name：{attribute.EventName} --Complete Execute Time：{DateTime.Now}");
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogInformation($"Event Name：{attribute!.EventName} --Exception Execute Time：{DateTime.Now}");
                    _logger.LogInformation($"Event Name Error {e.Source}--{e.Message}--{e.Data}");
                }
            });
        }

        isInitConsumer = false;
        await Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            isInitConsumer = false;
            _cts.Dispose();
            _scope.Dispose();
            factory.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}