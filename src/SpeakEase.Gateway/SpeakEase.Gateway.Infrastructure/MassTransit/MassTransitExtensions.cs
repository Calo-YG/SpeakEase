using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SpeakEase.Gateway.Infrastructure.MassTransit.Consumer;

namespace SpeakEase.Gateway.Infrastructure.MassTransit
{
    /// <summary>
    /// MassTransit Extensions
    /// </summary>
    public static class MassTransitExtensions
    {
        /// <summary>
        /// Build MassTransit with RabbitMQ
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProxyConfigChangeConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    using var scope = context.CreateScope();
                    
                    var options = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<MassTransitRabbitMqOptions>>();
                    
                    cfg.Host(options.Value.Host, options.Value.Port,options.Value.VirtualHost,options.Value.ConnectionName,op =>
                    {
                        op.Username(options.Value.UserName);
                        op.Password(options.Value.Password);
                    });
                    
                    cfg.ConfigureEndpoints(context);
                    //x.AddTransactionalEnlistmentBus();

                });
            });
            return services;
        }
    }
}
