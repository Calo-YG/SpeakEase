using Microsoft.Extensions.Logging;
using Yarp.ReverseProxy.Configuration;

namespace SpeakEase.Gateway.Infrastructure.Yarp.Core;

/// <summary>
/// 配置变更
/// </summary>
public class ProxyConfigChange(IProxyConfigDataBaseProvider proxyConfigDataBaseProvider, ILogger<IProxyConfigChange> logger,InMemoryConfigProvider proxyConfigProvider):IProxyConfigChange
{
    /// <summary>
    /// 同步更新
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void Refresh()
    {
       var proxyConfig =  proxyConfigDataBaseProvider.GetYarpConfig();
       
       proxyConfigProvider.Update(proxyConfig.Routes,proxyConfig.Clusters);
       
       logger.LogInformation("同步更新成功");
    }

    /// <summary>
    /// 异步更新
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task RefreshAsync()
    {
        var proxyConfig = await proxyConfigDataBaseProvider.GetYarpConfigAsync();
        
        proxyConfigProvider.Update(proxyConfig.Routes,proxyConfig.Clusters);
        
        logger.LogInformation("异步更新成功");
    }
}