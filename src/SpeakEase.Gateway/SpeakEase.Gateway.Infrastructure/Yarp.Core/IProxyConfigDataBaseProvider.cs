using Yarp.ReverseProxy.Configuration;

namespace SpeakEase.Gateway.Infrastructure.Yarp.Core
{
    public interface IProxyConfigDataBaseProvider
    {
        /// <summary>
        /// 同步获取yarp配置
        /// </summary>
        /// <returns></returns>
        (List<RouteConfig> Routes, List<ClusterConfig> Clusters) GetYarpConfig();

        /// <summary>
        /// 异步获取yarp配置
        /// </summary>
        /// <returns></returns>
        Task<(List<RouteConfig> Routes, List<ClusterConfig> Clusters)> GetYarpConfigAsync();
    }
    
}