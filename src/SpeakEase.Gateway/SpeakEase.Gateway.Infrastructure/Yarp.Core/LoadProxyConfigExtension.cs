using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy.Configuration;

namespace SpeakEase.Gateway.Infrastructure.Yarp.Core;

public static class LoadProxyConfigExtension
{
    public static void AddReverseProxyWithDatabase(this IServiceCollection service)
    {        
        service.AddReverseProxy();
        service.AddTransient<IProxyConfigDataBaseProvider, ProxyConfigDataBaseProvider>();
        service.AddTransient<IProxyConfigChange, ProxyConfigChange>();
        service.AddSingleton(sp =>
        {
            var provider = sp.GetRequiredService<IProxyConfigDataBaseProvider>();

            var proxyConfig = provider.GetYarpConfig();
            
            return new InMemoryConfigProvider(proxyConfig.Routes, proxyConfig.Clusters);
        });

        service.AddSingleton<IProxyConfigProvider>(sp => sp.GetRequiredService<InMemoryConfigProvider>());
    }
}