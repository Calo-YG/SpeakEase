using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpeakEase.Gateway.Domain.Entity.Gateway;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using Yarp.ReverseProxy.Configuration;

namespace SpeakEase.Gateway.Infrastructure.Yarp.Core;

/// <summary>
/// 初始化
/// </summary>
/// <param name="context"></param>
/// <param name="logger"></param>
public class ProxyConfigDataBaseProvider(IDbContext context,ILogger<IProxyConfigDataBaseProvider> logger):IProxyConfigDataBaseProvider
{

    /// <summary>
    /// 构建路由配置·
    /// </summary>
    /// <param name="routes"></param>
    /// <returns></returns>
    private List<RouteConfig> BuildRouteConfig(List<RouterEntity> routes)
    {
        return routes.Select(p => new RouteConfig
            {
                RouteId = p.Id,
                ClusterId = p.ClusterId,
                Match = new RouteMatch
                {
                    Path = p.Prefix,
                },
                Transforms =
                [
                    new Dictionary<string, string>
                    {
                        { "X-App-Id", p.AppId },
                        { "X-App-Name", p.AppName },
                    }
                ],
                AuthorizationPolicy = p.AuthorizationPolicy,
                Metadata = new Dictionary<string, string>
                {
                    { "RateLimiterPolicy", p.RateLimiterPolicy },
                    { "OutputCachePolicy", p.OutputCachePolicy },
                    { "TimeoutPolicy", p.TimeoutPolicy },
                    { "CorsPolicy", p.CorsPolicy },
                },
                Timeout = p.Timeout,
                MaxRequestBodySize = p.MaxRequestBodySize,
                RateLimiterPolicy = p.RateLimiterPolicy,
                OutputCachePolicy = p.OutputCachePolicy,
            }
        ).ToList();
    }

    /// <summary>
    /// 构建集群配置
    /// </summary>
    /// <param name="cluster"></param>
    /// <returns></returns>
    private List<ClusterConfig> BuildClusterConfig(List<ClusterEntity> cluster)
    {
        List<ClusterConfig> clusters = new List<ClusterConfig>(cluster.Count);

        if (!cluster.Any())
        {
            return clusters;
        }

        foreach (var item in  cluster)
        {
            var destinations = new Dictionary<string, DestinationConfig>();

            var index = 1;
            
            foreach (var address in item.Address.Split(','))
            {
                var key = $"{item.ClusterId}-{index}";
                DestinationConfig destinationConfig = new DestinationConfig()
                {
                    Address = address,
                    Health = item.Path
                };

                destinations.TryAdd(key, destinationConfig);
            }
            
            var clusterConfig = new ClusterConfig
            {
                ClusterId = item.ClusterId,
                LoadBalancingPolicy = item.LoadBalance,
                Destinations = destinations,
                HealthCheck = new HealthCheckConfig
                {
                    AvailableDestinationsPolicy = item.AvailableDestinationsPolicy,
                }
            };

            clusters.Add(clusterConfig);
        }
        
        return clusters;
    }
    /// <summary>
    /// 从数据库获取路由配置
    /// </summary>
    /// <returns></returns>
    private  List<RouteConfig> GetRouteConfig()
    {
        logger.LogInformation($"同步从数据库获取路由配置:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")})");

        var routes =  context.QueryNoTracking<RouterEntity>().ToList();

        return BuildRouteConfig(routes);
    }
    
    /// <summary>
    /// 从数据库获取路由配置
    /// </summary>
    /// <returns></returns>
    private  async Task<List<RouteConfig>> GetRouteConfigAsync()
    {
        logger.LogInformation($"异步从数据库获取路由配置:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")})");

        var routes = await context.QueryNoTracking<RouterEntity>().ToListAsync();

        return BuildRouteConfig(routes);
    }

    /// <summary>
    /// 从数据库获取集群配置
    /// </summary>
    /// <returns></returns>
    private async Task<List<ClusterConfig>> GetClusterConfigAsync()
    {
        logger.LogInformation($"异步从数据库获取集群配置:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")})");

        var cluster =await  context.QueryNoTracking<ClusterEntity>().ToListAsync();

        return BuildClusterConfig(cluster);
    }
    
    /// <summary>
    /// 从数据库获取集群配置
    /// </summary>
    /// <returns></returns>
    private  List<ClusterConfig> GetClusterConfig()
    {
        logger.LogInformation($"同步从数据库获取集群配置:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")})");

        var cluster =  context.QueryNoTracking<ClusterEntity>().ToList();

        return BuildClusterConfig(cluster);
    }

    /// <summary>
    /// 获取Yarp配置
    /// </summary>
    /// <returns></returns>
    public (List<RouteConfig> Routes, List<ClusterConfig> Clusters) GetYarpConfig()
    {
        return (GetRouteConfig(), GetClusterConfig());
    }
    
    /// <summary>
    /// 异步获取
    /// </summary>
    /// <returns></returns>
    public async Task<(List<RouteConfig> Routes, List<ClusterConfig> Clusters)> GetYarpConfigAsync()
    {
        return (await GetRouteConfigAsync(), await GetClusterConfigAsync());
    }
}