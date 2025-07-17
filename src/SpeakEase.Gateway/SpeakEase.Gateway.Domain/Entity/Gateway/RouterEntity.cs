using System.ComponentModel.DataAnnotations.Schema;
using SpeakEase.Domain.Contract.Entity;

namespace SpeakEase.Gateway.Domain.Entity.Gateway;

/// <summary>
/// 路由实体
/// </summary>
[Table("route")]
public class RouterEntity:SpeakEaseEntity
{
    /// <summary>
    /// 应用id
    /// </summary>
    public string AppId { get;private set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get;private set; }

    /// <summary>
    /// 路由前缀
    /// </summary>
    public string Prefix { get;private set; }

    //
    // 摘要:排序序号
    //
    public int? Sort { get;private set; }

    //
    // 摘要:路由集群id
    //
    public string ClusterId { get; private set; }

    //
    // 摘要:授权策略
    public string AuthorizationPolicy { get;private set; }

    //
    // 摘要:限流策略
    //
    public string RateLimiterPolicy { get; private set; }

    //
    // 摘要:响应缓存策略
    public string OutputCachePolicy { get; private set; }

    //
    // 摘要:超时策略
    //
    public string TimeoutPolicy { get; private set; }

    //
    // 摘要:超时时间
    //
    public TimeSpan? Timeout { get; private  set; }

    //
    // 摘要:跨域策略
    //
    public string CorsPolicy { get; private set; }

    //
    // 摘要:设置请求体最大大小
    //
    public long? MaxRequestBodySize { get; private set; }
    
    protected RouterEntity()
    {
        
    }
    
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="id"></param>
    /// <param name="appId">appId</param>
    /// <param name="appName">app名称</param>
    /// <param name="prefix">路由前缀</param>
    /// <param name="sort">排序</param>
    /// <param name="authorizationPolicy">授权策略</param>
    /// <param name="rateLimiterPolicy">限流测率</param>
    /// <param name="outputCachePolicy">响应缓存策略</param>
    /// <param name="timeoutPolicy">超时策略</param>
    /// <param name="timeout">超时时间</param>
    /// <param name="corsPolicy">跨域策略</param>
    /// <param name="maxRequestBodySize">请求体body限制</param>
    public RouterEntity(string id,string appId, string appName, string prefix, int? sort, string authorizationPolicy, string rateLimiterPolicy, string outputCachePolicy, string timeoutPolicy, TimeSpan? timeout, string corsPolicy, long? maxRequestBodySize)
    { 
        Id = id;
        AppId = appId;
        AppName = appName;
        Prefix = prefix;
        Sort = sort;
        AuthorizationPolicy = authorizationPolicy;
        RateLimiterPolicy = rateLimiterPolicy;
        OutputCachePolicy = outputCachePolicy;
        TimeoutPolicy = timeoutPolicy;
        Timeout = timeout;
        CorsPolicy = corsPolicy;
        MaxRequestBodySize = maxRequestBodySize;
    }

    /// <summary>
    /// 设置集群id
    /// </summary>
    /// <param name="clusterId"></param>
    public void SetClusterId(string clusterId)
    {
        ClusterId = clusterId;
    }
    
    public void Update(string prefix, int? sort, string authorizationPolicy, string rateLimiterPolicy, string outputCachePolicy, string timeoutPolicy, TimeSpan? timeout, string corsPolicy, long? maxRequestBodySize)
    {
        Prefix = prefix;
        Sort = sort;
        AuthorizationPolicy = authorizationPolicy;
        RateLimiterPolicy = rateLimiterPolicy;
        OutputCachePolicy = outputCachePolicy;
        TimeoutPolicy = timeoutPolicy;
        Timeout = timeout;
        CorsPolicy = corsPolicy;
        MaxRequestBodySize = maxRequestBodySize;
    }
}