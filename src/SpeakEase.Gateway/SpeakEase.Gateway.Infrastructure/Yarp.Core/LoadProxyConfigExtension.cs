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
                // 获取路由匹配时捕获的 {catch-all} 参数值
                if (transformContext.HttpContext.Request.RouteValues.TryGetValue("catch-all", out var catchAllValue) ||
                    transformContext.HttpContext.Request.RouteValues.TryGetValue("remainder", out catchAllValue))
                {
                    var catchAllPath = catchAllValue?.ToString();
            
                    // 构建新的路径，将捕获的路径片段拼接到 /api/ 后面
                    var newPath = $"/api/{catchAllPath}";
            
                    // 安全地更新路径
                    transformContext.Path = new PathString(newPath);
                }
        
                await Task.CompletedTask;
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