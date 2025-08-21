using Microsoft.Extensions.DependencyInjection;

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
        public static IServiceCollection BuildMassTransitWithRabbitMq(IServiceCollection services)
        {
            return services;
        }
    }
}
