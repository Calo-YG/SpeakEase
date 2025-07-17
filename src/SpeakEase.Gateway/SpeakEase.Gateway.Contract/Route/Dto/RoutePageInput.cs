using SpeakEase.Infrastructure.Shared;

namespace SpeakEase.Gateway.Contract.Route.Dto;

/// <summary>
/// 路由分页输入
/// </summary>
public sealed class RoutePageInput
{
    /// <summary>
    /// 应用id
    /// </summary>
    public string AppId { get; set; }
    
    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }
    
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }
    
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
    
    /// <summary>
    /// 分页
    /// </summary>
    public Pagination Pagination { get; set; }
}