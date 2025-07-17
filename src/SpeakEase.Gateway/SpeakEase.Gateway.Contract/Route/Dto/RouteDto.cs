namespace SpeakEase.Gateway.Contract.Route.Dto;

public class RouteDto
{
    /// <summary>
    /// 主键
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// 应用id
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// 路由前缀
    /// </summary>
    public string Prefix { get; set; }

    //
    // 摘要:排序序号
    //
    public int? Sort { get; init; }

    //
    // 摘要:路由集群id
    //
    public string ClusterId { get;  set; }

    //
    // 摘要:授权策略
    public string AuthorizationPolicy { get; set; }

    //
    // 摘要:限流策略
    //
    public string RateLimiterPolicy { get;  set; }

    //
    // 摘要:响应缓存策略
    public string OutputCachePolicy { get;  set; }

    //
    // 摘要:超时策略
    //
    public string TimeoutPolicy { get;  set; }

    //
    // 摘要:超时时间
    //
    public TimeSpan? Timeout { get;   set; }

    //
    // 摘要:跨域策略
    //
    public string CorsPolicy { get;  set; }

    //
    // 摘要:设置请求体最大大小
    //
    public long? MaxRequestBodySize { get;  set; }
}