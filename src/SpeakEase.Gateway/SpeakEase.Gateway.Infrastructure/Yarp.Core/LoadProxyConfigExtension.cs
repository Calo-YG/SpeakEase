using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Transforms;

namespace SpeakEase.Gateway.Infrastructure.Yarp.Core;

public static class LoadProxyConfigExtension
{
    public static void AddReverseProxyWithDatabase(this IServiceCollection service)
    {        
        service.AddReverseProxy().AddTransforms(ctx =>
        {
            ctx.AddRequestTransform(async transformContext =>
            {
                using var scope = transformContext.HttpContext.RequestServices.CreateScope();
                
                var transformProvider = scope.ServiceProvider.GetRequiredService<ITransformProvider>();
                
                await transformProvider.DatabaseTransformAsync(transformContext);
            });
        });
        service.AddTransient<IProxyConfigDataBaseProvider, ProxyConfigDataBaseProvider>();
        service.AddTransient<IProxyConfigChange, ProxyConfigChange>();
        service.AddSingleton(sp =>
        {
            using var scope = sp.CreateScope();
            
            var provider = scope.ServiceProvider.GetRequiredService<IProxyConfigDataBaseProvider>();

            var proxyConfig = provider.GetYarpConfig();
            
            return new InMemoryConfigProvider(proxyConfig.Routes, proxyConfig.Clusters);
        });

        service.AddSingleton<IProxyConfigProvider>(sp => sp.GetRequiredService<InMemoryConfigProvider>());
    }
}