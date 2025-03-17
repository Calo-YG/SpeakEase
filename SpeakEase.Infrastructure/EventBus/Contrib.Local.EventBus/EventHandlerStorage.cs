using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using SpeakEase.Infrastructure.EventBus.BuildingBlock.Local.EventBus;

namespace SpeakEase.Infrastructure.EventBus.Contrib.Local.EventBus;

public class EventHandlerStorage: IEventHandlerStorage
{
      public ConcurrentBag<EventDiscription> Events { get; }
      
      private readonly IServiceCollection Services;
      
      public EventHandlerStorage(IServiceCollection services)
      {
            Services = services;
            Events = new ConcurrentBag<EventDiscription>();
            services.AddSingleton<IEventHandlerStorage>(this);
      }
      
      private bool Check(Type type)
      {
            var discription = Events.FirstOrDefault(p=>p.EtoType == type);

            return discription is null;
      }
        
      ///订阅并且注入EventHandler
      public void Subscribe(Type eto,Type handler)
      {
            if(!Check(eto))
            {
                  return;
            }

            Events.Add(new EventDiscription(eto, handler));

            var handlerbaseType = typeof(IEventHandler<>);

            var handlertype = handlerbaseType.MakeGenericType(eto);

            if(Services.Any(P=>P.ServiceType==handlertype))
            {
                  return;
            }

            Services.AddTransient(handlertype, handler);
      }

      public void Subscribe<TEto, THandler>() 
            where TEto : class
            where THandler :IEventHandler<TEto>
      {
            Subscribe(typeof(TEto),typeof(THandler));  
      }
}